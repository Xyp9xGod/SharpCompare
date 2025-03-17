using SharpCompare.Factory;
using SharpCompare.Interfaces;

namespace SharpCompare.Tests.Reflection
{
    public class SharpCompareAnonymousTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void CompareAnonymousTypes_ShouldReturnTrue_WhenIdentical()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Alice", Age = 30 };

            Assert.True(_comparer.IsEqual(obj1, obj2));
        }

        [Fact]
        public void CompareAnonymousTypes_ShouldReturnFalse_WhenDifferent()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Bob", Age = 25 };

            Assert.False(_comparer.IsEqual(obj1, obj2));
        }
    }
}
