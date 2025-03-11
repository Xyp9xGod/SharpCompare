using SharpCompare.Extensions;
using SharpCompare.Interfaces;
using System.Collections;
using System.Reflection;

namespace SharpCompare.Services
{
    public class SharpCompareService : ISharpCompare
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
