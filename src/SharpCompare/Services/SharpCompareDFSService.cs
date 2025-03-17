using SharpCompare.Extensions;
using System.Collections;
using System.Reflection;

namespace SharpCompare.Services
{
    internal class SharpCompareDFSService : SharpCompareBaseService
    {
        private static readonly Dictionary<Type, PropertyInfo[]> _propertyCache = new();

        public override bool IsEqual(object firstObject, object secondObject)
        {
            if (firstObject == null || secondObject == null)
                return firstObject == secondObject;

            if (firstObject.GetType() != secondObject.GetType())
                return false;

            var stack = new Stack<ValueTuple<object, object>>();
            var visited = new HashSet<ValueTuple<object, object>>();

            stack.Push((firstObject, secondObject));

            while (stack.Count > 0)
            {
                var (obj1, obj2) = stack.Pop();

                if (obj1 == null || obj2 == null)
                {
                    if (obj1 != obj2) return false;
                    continue;
                }

                if (ReferenceEquals(obj1, obj2))
                    continue;

                if (obj1.GetType().IsPrimitive || obj1 is string)
                {
                    if (!obj1.Equals(obj2)) return false;
                    continue;
                }

                // Avoid redundant comparisons
                var pair = (obj1, obj2);
                if (visited.Contains(pair))
                    continue;
                visited.Add(pair);

                if (obj1 is IEnumerable firstEnumerable && obj2 is IEnumerable secondEnumerable)
                {
                    var enumerator1 = firstEnumerable.GetEnumerator();
                    var enumerator2 = secondEnumerable.GetEnumerator();

                    try
                    {
                        while (enumerator1.MoveNext() && enumerator2.MoveNext())
                        {
                            stack.Push((enumerator1.Current, enumerator2.Current));
                        }

                        if (enumerator1.MoveNext() || enumerator2.MoveNext())
                            return false;
                    }
                    finally
                    {
                        (enumerator1 as IDisposable)?.Dispose();
                        (enumerator2 as IDisposable)?.Dispose();
                    }

                    continue;
                }

                var type = obj1.GetType();
                if (!_propertyCache.TryGetValue(type, out var properties))
                {
                    properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => !Attribute.IsDefined(p, typeof(IgnoreComparisonAttribute)))
                        .ToArray();
                    _propertyCache[type] = properties;
                }

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

        public override List<string> GetDifferences(object firstObject, object secondObject, string path = "")
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

                if (firstObj is IDictionary firstDict && secondObj is IDictionary secondDict)
                {
                    foreach (var key in firstDict.Keys.Cast<object>().Union(secondDict.Keys.Cast<object>()))
                    {
                        string? keyPath = string.IsNullOrEmpty(currentPath) ? key.ToString() : $"{currentPath}.{key}";

                        if (!firstDict.Contains(key))
                        {
                            differences.Add($"{keyPath}: Missing in first dictionary");
                            continue;
                        }

                        if (!secondDict.Contains(key))
                        {
                            differences.Add($"{keyPath}: Missing in second dictionary");
                            continue;
                        }

                        stack.Push((firstDict[key], secondDict[key], keyPath));
                    }
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
