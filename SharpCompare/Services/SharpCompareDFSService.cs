using SharpCompare.Extensions;
using SharpCompare.Interfaces;
using System.Collections;
using System.Reflection;

namespace SharpCompare.Services
{
    internal class SharpCompareDFSService : ISharpCompare
    {
        public bool IsEqual(object firstObject, object secondObject)
        {
            if (firstObject == null || secondObject == null)
                return firstObject == secondObject;

            if (firstObject.GetType() != secondObject.GetType())
                return false;

            var stack = new Stack<(object, object)>();
            stack.Push((firstObject, secondObject));

            while (stack.Count > 0)
            {
                var (obj1, obj2) = stack.Pop();

                if (obj1 == null || obj2 == null)
                {
                    if (obj1 != obj2) return false;
                    continue;
                }

                if (obj1.GetType().IsPrimitive || obj1 is string)
                {
                    if (!obj1.Equals(obj2)) return false;
                    continue;
                }

                if (obj1 is IEnumerable firstEnumerable && obj2 is IEnumerable secondEnumerable)
                {
                    if (!CompareCollections(firstEnumerable, secondEnumerable))
                        return false;
                    continue;
                }

                var properties = obj1.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => !Attribute.IsDefined(p, typeof(IgnoreComparisonAttribute)));

                foreach (var property in properties)
                {
                    var value1 = property.GetValue(obj1);
                    var value2 = property.GetValue(obj2);

                    if (value1 == null || value2 == null)
                    {
                        if (value1 != value2) return false;
                        continue;
                    }

                    stack.Push((value1, value2));
                }
            }

            return true;
        }

        public List<string> GetDifferences(object firstObject, object secondObject, string path = "")
        {
            var differences = new List<string>();
            var stack = new Stack<(object firstObject, object secondObject, string path)>();
            stack.Push((firstObject, secondObject, path));

            while (stack.Count > 0)
            {
                var (firstObj, secondObj, currentPath) = stack.Pop();

                if (firstObj == null || secondObj == null)
                {
                    if (firstObj != secondObj)
                        differences.Add($"{currentPath}: {firstObj?.ToString() ?? "null"} → {secondObj?.ToString() ?? "null"}");
                    continue;
                }

                if (firstObj.GetType() != secondObj.GetType())
                {
                    if (string.IsNullOrEmpty(currentPath))
                        differences.Add("Objects are of different types");
                    else
                        differences.Add($"{currentPath}: Objects are of different types");

                    continue;
                }

                var type = firstObj.GetType();
                if (type.IsPrimitive || firstObj is string || firstObj is decimal)
                {
                    if (!firstObj.Equals(secondObj))
                        differences.Add($"{currentPath}: {firstObj} → {secondObj}");
                    continue;
                }

                if (typeof(IEnumerable).IsAssignableFrom(type) && firstObj is IEnumerable firstEnumerable && secondObj is IEnumerable secondEnumerable)
                {
                    var list1 = firstEnumerable.Cast<object>().ToList();
                    var list2 = secondEnumerable.Cast<object>().ToList();
                    for (int i = 0; i < Math.Max(list1.Count, list2.Count); i++)
                    {
                        var item1 = i < list1.Count ? list1[i] : "MISSING";
                        var item2 = i < list2.Count ? list2[i] : "MISSING";
                        stack.Push((item1, item2, $"{currentPath}[{i}]"));
                    }
                    continue;
                }

                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var value1 = prop.GetValue(firstObj);
                    var value2 = prop.GetValue(secondObj);
                    string propertyPath = string.IsNullOrEmpty(currentPath) ? prop.Name : $"{currentPath}.{prop.Name}";
                    stack.Push((value1, value2, propertyPath));
                }
            }

            return differences;
        }

        private bool CompareCollections(IEnumerable firstCollection, IEnumerable secondCollection)
        {
            var firstList = firstCollection.Cast<object>().ToList();
            var secondList = secondCollection.Cast<object>().ToList();

            if (firstList.Count != secondList.Count)
                return false;

            for (int i = 0; i < firstList.Count; i++)
            {
                if (!IsEqual(firstList[i], secondList[i]))
                    return false;
            }

            return true;
        }
    }
}
