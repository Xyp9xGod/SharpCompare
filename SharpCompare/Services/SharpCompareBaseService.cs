using SharpCompare.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SharpCompare.Services
{
    internal abstract class SharpCompareBaseService : ISharpCompare
    {
        public abstract bool IsEqual(object firstObject, object secondObject);
        public abstract List<string> GetDifferences(object firstObject, object secondObject, string path = "");

        public bool IsEqualJson(object firstObject, object secondObject)
        {
            if (firstObject == null || secondObject == null)
                return firstObject == secondObject;

            string json1 = JsonSerializer.Serialize(firstObject);
            string json2 = JsonSerializer.Serialize(secondObject);

            return json1 == json2;
        }

        public bool CompareByHash(object firstObject, object secondObject)
        {
            return ComputeObjectHash(firstObject) == ComputeObjectHash(secondObject);
        }

        private static string ComputeObjectHash(object obj)
        {
            if (obj == null)
                return string.Empty;

            string json = JsonSerializer.Serialize(obj);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
