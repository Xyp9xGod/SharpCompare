using SharpCompare.Factory;
using SharpCompare.Interfaces;
using SharpCompare.Models;

namespace SharpCompare.Tests.Reflection
{
    public class SharpCompareNestedObjectTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void ObjectsAreEqual_ShouldReturnTrue()
        {
            var person1 = new Person { Name = "John", Age = 30, Address = new Address { Street = "Street A", Number = 100 } };
            var person2 = new Person { Name = "John", Age = 30, Address = new Address { Street = "Street A", Number = 100 } };

            bool result = _comparer.IsEqual(person1, person2);

            Assert.True(result);
        }

        [Fact]
        public void ObjectsAreDifferent_ShouldReturnFalse()
        {
            var person1 = new Person { Name = "John", Age = 30, Address = new Address { Street = "Street A", Number = 100 } };
            var person2 = new Person { Name = "Carlos", Age = 25, Address = new Address { Street = "Street B", Number = 200 } };

            bool result = _comparer.IsEqual(person1, person2);

            Assert.False(result);
        }
    }
}
