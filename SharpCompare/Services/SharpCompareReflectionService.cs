using SharpCompare.Extensions;
using SharpCompare.Interfaces;
using System.Collections;
using System.Reflection;

namespace SharpCompare.Services
{
    internal class SharpCompareReflectionService : ISharpCompare
    {
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

        public List<string> GetDifferences(object firstObject, object secondObject, string path = "")
        {
            var differences = new List<string>();

            if (firstObject == null || secondObject == null)
            {
                if (firstObject != secondObject)
                    differences.Add($"{path}: {firstObject} → {secondObject}");
                return differences;
            }

            if (firstObject.GetType() != secondObject.GetType())
            {
                if (string.IsNullOrEmpty(path))
                    differences.Add("Objects are of different types");
                else
                    differences.Add($"{path}: Objects are of different types");

                return differences;
            }

            var type = firstObject.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value1 = prop.GetValue(firstObject);
                var value2 = prop.GetValue(secondObject);
                string propertyPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";

                if (value1 == null || value2 == null)
                {
                    if (value1 != value2)
                        differences.Add($"{propertyPath}: {value1} → {value2}");
                    continue;
                }

                if (prop.PropertyType.IsPrimitive || prop.PropertyType.IsValueType || value1 is string || value1 is decimal)
                {
                    if (!value1.Equals(value2))
                        differences.Add($"{propertyPath}: {value1} → {value2}");
                }
                else
                {
                    differences.AddRange(GetDifferences(value1, value2, propertyPath));
                }
            }

            return differences;
        }

        private bool AreValuesEqual(object? value1, object? value2)
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
