using SharpCompare.Factory;
using SharpCompare.Interfaces;

namespace SharpCompare.Tests.Reflection
{
    public class SharpCompareHashTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void IsEqualHashSet_ShouldReturnTrue()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };

            var hashSet2 = new HashSet<int>() { 1, 2, 3, 4, 5 };

            bool result = _comparer.IsEqual(hashSet1, hashSet2);

            Assert.True(result);
        }

        [Fact]
        public void IsEqualHashSet_ShouldReturnFalse()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };

            var hashSet2 = new HashSet<int>() { 1, 2, 7, 4, 5 };

            bool result = _comparer.IsEqual(hashSet1, hashSet2);

            Assert.False(result);
        }

        [Fact]
        public void GetDifferences_ShouldDetectDifferencesInHashSets()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };
            var hashSet2 = new HashSet<int>() { 1, 2, 7, 4, 9 };

            var differences = _comparer.GetDifferences(hashSet1, hashSet2);

            Assert.Contains("3 → Missing on second object", differences);
            Assert.Contains("5 → Missing on second object", differences);
            Assert.Contains("7 → Missing on first object", differences);
            Assert.Contains("9 → Missing on first object", differences);
        }

        [Fact]
        public void IsEqualJson_ShouldReturnTrue_WhenHashSetsAreIdentical()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };
            var hashSet2 = new HashSet<int>() { 1, 2, 3, 4, 5 };

            bool result = _comparer.IsEqualJson(hashSet1, hashSet2);

            Assert.True(result);
        }

        [Fact]
        public void IsEqualJson_ShouldReturnFalse_WhenHashSetsAreDifferent()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };
            var hashSet2 = new HashSet<int>() { 1, 2, 7, 4, 9 };

            bool result = _comparer.IsEqualJson(hashSet1, hashSet2);

            Assert.False(result);
        }

        [Fact]
        public void CompareByHash_ShouldReturnTrue_WhenHashSetsAreIdentical()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };
            var hashSet2 = new HashSet<int>() { 1, 2, 3, 4, 5 };

            bool result = _comparer.CompareByHash(hashSet1, hashSet2);

            Assert.True(result);
        }

        [Fact]
        public void CompareByHash_ShouldReturnFalse_WhenDictionariesAreDifferent()
        {
            var hashSet1 = new HashSet<int>() { 1, 2, 3, 4, 5 };
            var hashSet2 = new HashSet<int>() { 1, 2, 7, 4, 9 };

            bool result = _comparer.CompareByHash(hashSet1, hashSet2);

            Assert.False(result);
        }
    }
}
