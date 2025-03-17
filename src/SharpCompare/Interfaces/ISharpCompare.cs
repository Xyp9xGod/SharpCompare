namespace SharpCompare.Interfaces
{
    public interface ISharpCompare
    {
        /// <summary>
        /// Compares two objects by their properties and values, ignoring memory references.
        /// </summary>
        /// <param name="firstObject">The first object to compare.</param>
        /// <param name="secondObject">The second object to compare.</param>
        /// <returns>
        /// Returns <c>true</c> if both objects have the same property values; otherwise, returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// - Supports deep comparison of nested objects.
        /// - Can compare collections like lists and dictionaries.
        /// - Allows ignoring specific properties using the <see cref="IgnoreComparisonAttribute"/>.
        /// </remarks>
        bool IsEqual(object firstObject, object secondObject);

        /// <summary>
        /// Identifies and returns a list of differences between two objects by comparing their properties and values.
        /// </summary>
        /// <param name="firstObject">The first object to compare.</param>
        /// <param name="secondObject">The second object to compare.</param>
        /// <param name="path">
        /// (Optional) The current property path used for tracking nested differences.
        /// This parameter is mainly used for recursive comparisons of nested objects.
        /// </param>
        /// <returns>
        /// A list of differences between the two objects.
        /// Each difference is formatted as "<c>PropertyName: Value1 → Value2</c>".
        /// </returns>
        /// <remarks>
        /// - Supports deep comparison of nested objects and collections.
        /// - Can detect differences in primitive types, strings, and complex objects.
        /// - Returns a human-readable list of differences.
        /// - Useful for debugging and data validation.
        /// </remarks>
        List<string> GetDifferences(object firstObject, object secondObject, string path = "");

        /// <summary>
        /// Compares two objects by serializing them into JSON and checking if the resulting JSON strings are equal.
        /// </summary>
        /// <param name="firstObject">The first object to compare.</param>
        /// <param name="secondObject">The second object to compare.</param>
        /// <returns>
        /// Returns <c>true</c> if both objects serialize to identical JSON; otherwise, returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// - Uses JSON serialization to normalize the object structure before comparison.
        /// - Ignores memory references and focuses purely on data values.
        /// - Useful for scenarios where the serialization format is critical.
        /// </remarks>
        bool IsEqualJson(object firstObject, object secondObject);

        /// <summary>
        /// Compares two objects by computing a cryptographic hash of their serialized JSON representation.
        /// </summary>
        /// <param name="firstObject">The first object to compare.</param>
        /// <param name="secondObject">The second object to compare.</param>
        /// <returns>
        /// Returns <c>true</c> if both objects produce the same hash; otherwise, returns <c>false</c>.
        /// </returns>
        /// <remarks>
        /// - Uses SHA-256 to generate a hash from the JSON-serialized object.
        /// - Efficient for checking if objects have the same content without direct value comparisons.
        /// - Particularly useful for caching, deduplication, or integrity validation scenarios.
        /// </remarks>
        bool CompareByHash(object firstObject, object secondObject);
    }
}
