using SharpCompare.Interfaces;
using SharpCompare.Factory;
using Xunit;

namespace SharpCompare.Tests.Net8.Common
{
    public class SharpCompareObjectsTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: true);
        [Fact]
        public void IsEqualJson_ShouldReturnTrue_WhenObjectsAreIdentical()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Alice", Age = 30 };

            Assert.True(_comparer.IsEqualJson(obj1, obj2));
        }

        [Fact]
        public void IsEqualJson_ShouldReturnFalse_WhenObjectsAreDifferent()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Bob", Age = 35 };

            Assert.False(_comparer.IsEqualJson(obj1, obj2));
        }

        [Fact]
        public void CompareByHash_ShouldReturnTrue_WhenObjectsAreIdentical()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Alice", Age = 30 };

            Assert.True(_comparer.CompareByHash(obj1, obj2));
        }

        [Fact]
        public void CompareByHash_ShouldReturnFalse_WhenObjectsAreDifferent()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Bob", Age = 35 };

            Assert.False(_comparer.CompareByHash(obj1, obj2));
        }
    }
}
