using SharpCompare.Extensions;
using System.Collections;
using System.Reflection;

namespace SharpCompare.Services
{
    internal class SharpCompareReflectionService : SharpCompareBaseService
    {
        public override bool IsEqual(object firstObject, object secondObject)
        {
            if (firstObject == null || secondObject == null)
                return firstObject == secondObject;

            if (firstObject.GetType() != secondObject.GetType())
                return false;

            if (firstObject is string || firstObject is decimal || firstObject.GetType().IsPrimitive || firstObject.GetType().IsValueType)
                return firstObject.Equals(secondObject);

            if (firstObject is IDictionary firstDict && secondObject is IDictionary secondDict)
            {
                if (firstDict.Count != secondDict.Count)
                    return false;

                foreach (var key in firstDict.Keys)
                {
                    if (!secondDict.Contains(key))
                        return false;

                    if (!AreValuesEqual(firstDict[key], secondDict[key]))
                        return false;
                }
                return true;
            }

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

        public override List<string> GetDifferences(object? firstObject, object? secondObject, string path = "")
        {
            var differences = new List<string>();

            if (firstObject == null || secondObject == null)
            {
                if (firstObject != secondObject)
                    differences.Add($"{path}: {firstObject?.ToString() ?? "null"} → {secondObject?.ToString() ?? "null"}");
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

            if (firstObject is IDictionary firstDict && secondObject is IDictionary secondDict)
            {
                foreach (var key in firstDict.Keys.Cast<object>().Union(secondDict.Keys.Cast<object>()))
                {
                    string? keyPath = string.IsNullOrEmpty(path) ? key.ToString() : $"{path}.{key}";

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

                    differences.AddRange(GetDifferences(firstDict[key], secondDict[key], keyPath));
                }
                return differences;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                var firstSet = ((IEnumerable)firstObject).Cast<object>().ToHashSet();
                var secondSet = ((IEnumerable)secondObject).Cast<object>().ToHashSet();

                foreach (var item in firstSet.Except(secondSet))
                {
                    differences.Add($"{item} → Missing on second object");
                }

                foreach (var item in secondSet.Except(firstSet))
                {
                    differences.Add($"{item} → Missing on first object");
                }

                return differences;
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value1 = prop.GetValue(firstObject);
                var value2 = prop.GetValue(secondObject);
                string propertyPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";

                if (value1 == null || value2 == null)
                {
                    if (value1 != value2)
                        differences.Add($"{propertyPath}: {value1?.ToString() ?? "null"} → {value2?.ToString() ?? "null"}");
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
