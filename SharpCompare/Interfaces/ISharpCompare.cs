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
    }
}
