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
    }
}
