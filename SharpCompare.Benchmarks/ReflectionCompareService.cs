using System.Collections;
using System.Reflection;
using SharpCompare.Extensions;
using SharpCompare.Interfaces;

namespace SharpCompare.Benchmarks
{
    internal class ReflectionCompareService : ISharpCompare
    {
        public bool CompareByHash(object firstObject, object secondObject)
        {
            throw new NotImplementedException();
        }

        public List<string> GetDifferences(object firstObject, object secondObject, string path = "")
        {
            throw new NotImplementedException();
        }

        public bool IsEqual(object firstObject, object secondObject)
        {
            if (firstObject == null || secondObject == null)
                return firstObject == secondObject;

            if (firstObject.GetType() != secondObject.GetType())
                return false;

            if (firstObject is IEnumerable firstEnumerable && secondObject is IEnumerable secondEnumerable)
                return CompareCollections(firstEnumerable, secondEnumerable);

            var properties = firstObject.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreComparisonAttribute)));

            foreach (var property in properties)
            {
                var value1 = property.GetValue(firstObject);
                var value2 = property.GetValue(secondObject);

                if (!AreValuesEqual(value1, value2))
                    return false;
            }

            return true;
        }

        public bool IsEqualJson(object firstObject, object secondObject)
        {
            throw new NotImplementedException();
        }

        private bool AreValuesEqual(object value1, object value2)
        {
            if (value1 == null || value2 == null)
                return value1 == value2;

            if (value1.GetType().IsClass && !(value1 is string))
                return IsEqual(value1, value2);

            return Equals(value1, value2);
        }

        private bool CompareCollections(IEnumerable firstCollection, IEnumerable secondCollection)
        {
            var firstList = firstCollection.Cast<object>().ToList();
            var secondList = secondCollection.Cast<object>().ToList();

            if (firstList.Count != secondList.Count)
                return false;

            for (int i = 0; i < firstList.Count; i++)
            {
                if (!AreValuesEqual(firstList[i], secondList[i]))
                    return false;
            }

            return true;
        }
    }
}
