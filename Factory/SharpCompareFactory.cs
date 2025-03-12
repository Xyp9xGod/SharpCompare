using SharpCompare.Interfaces;
using SharpCompare.Services;

namespace SharpCompare.Factory
{
    /// <summary>
    /// Factory for creating instances of SharpCompare.
    /// </summary>
    public static class SharpCompareFactory
    {
        /// <summary>
        /// Creates a default implementation of <see cref="ISharpCompare"/>.
        /// </summary>
        /// <param name="useDFS">Defines whether to use DFS-based comparison (default: true).</param>
        /// <returns>An instance of <see cref="ISharpCompare"/>.</returns>
        public static ISharpCompare Create(bool useDFS = true)
        {
            return useDFS ? new SharpCompareDFSService() : new SharpCompareReflectionService();
        }
    }
}
