using SharpCompare.Factory;
using SharpCompare.Interfaces;

namespace SharpCompare.Tests.Reflection
{
    public class SharpComparePrimitiveTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void ComparePrimitiveTypes_ShouldReturnTrue()
        {
            Assert.True(_comparer.IsEqual(0.124, 0.124));
            Assert.True(_comparer.IsEqual(10, 10));
            Assert.True(_comparer.IsEqual(3.14, 3.14));
            Assert.True(_comparer.IsEqual('A', 'A'));
            Assert.True(_comparer.IsEqual(true, true));
            Assert.True(_comparer.IsEqual(false, false));
        }

        [Fact]
        public void ComparePrimitiveTypes_ShouldReturnFalse()
        {
            Assert.False(_comparer.IsEqual(0.123, 0.124));
            Assert.False(_comparer.IsEqual(10, 20));
            Assert.False(_comparer.IsEqual(3.14, 2.71));
            Assert.False(_comparer.IsEqual('A', 'B'));
            Assert.False(_comparer.IsEqual(true, false));
            Assert.False(_comparer.IsEqual(false, true));
        }
    }
}
