using SharpCompare.Factory;
using SharpCompare.Interfaces;
using SharpCompare.Models;

namespace SharpCompare.Tests.Reflection
{
    public class SharpCompareObjectTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void CompareDifferentTypes_ShouldReturnFalse()
        {
            var person = new Person { Name = "John", Age = 30 };
            var address = new Address { Street = "Street A", Number = 100 };

            bool result = _comparer.IsEqual(person, address);

            Assert.False(result);
        }

        [Fact]
        public void GetDifferences_ShouldReturnDifferences_WhenGenericObjectsAreDifferent()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Bob", Age = 35 };

            var differences = _comparer.GetDifferences(obj1, obj2);

            Assert.Contains("Name: Alice → Bob", differences);
            Assert.Contains("Age: 30 → 35", differences);
        }

        [Fact]
        public void GetDifferences_ShouldReturnDifferences_WhenClassObjectsAreDifferent()
        {
            var firstPerson = new Person { Name = "Alice", Age = 30 };
            var secondPerson = new Person { Name = "Bob", Age = 35 };

            var differences = _comparer.GetDifferences(firstPerson, secondPerson);

            Assert.Contains("Name: Alice → Bob", differences);
            Assert.Contains("Age: 30 → 35", differences);
        }

        [Fact]
        public void GetDifferences_ShouldReturnDifferences_WhenObjectsAreDifferent()
        {
            var obj1 = new { Name = "Alice", Age = 30 };
            var obj2 = new { Name = "Bob", Age = 35 };

            var differences = _comparer.GetDifferences(obj1, obj2);

            Assert.Contains("Name: Alice → Bob", differences);
            Assert.Contains("Age: 30 → 35", differences);
        }

        [Fact]
        public void GetDifferences_ShouldHandleClassNullValues()
        {
            var firstPerson = new Person { Name = "Alice", Age = null };
            var secondPerson = new Person { Name = "Alice", Age = 30 };

            var differences = _comparer.GetDifferences(firstPerson, secondPerson);

            Assert.Contains("Age: null → 30", differences);
        }

        [Fact]
        public void GetDifferences_ShouldReturnTypeDifference_WhenObjectsAreOfDifferentTypes()
        {
            var obj1 = new { Name = "Alice" };
            var obj2 = new { FirstName = "Alice" };

            var differences = _comparer.GetDifferences(obj1, obj2);

            Assert.Contains("Objects are of different types", differences);
        }

        [Fact]
        public void GetDifferences_ShouldDetectNestedDifferences()
        {
            var firstPerson = new Person
            {
                Name = "Alice",
                Age = 30,
                Address = new Address { Street = "Main St", Number = 100 }
            };

            var secondPerson = new Person
            {
                Name = "Alice",
                Age = 30,
                Address = new Address { Street = "Second St", Number = 100 }
            };

            var differences = _comparer.GetDifferences(firstPerson, secondPerson);

            Assert.Contains("Address.Street: Main St → Second St", differences);
        }

        [Fact]
        public void IsEqual_DifferentTypes_ReturnsFalse()
        {
            var person = new Person { Name = "Jon", Age = 30 };
            var student = new Student { Name = "Jon", Age = 30 };

            Assert.False(_comparer.IsEqual(person, student));
        }

        [Fact]
        public void ObjectsWithIgnoredProperty_ShouldBeEqual()
        {
            var person1 = new Person { Name = "John", Age = 30, Password = "Value1" };
            var person2 = new Person { Name = "John", Age = 30, Password = "Value2" };

            bool result = _comparer.IsEqual(person1, person2);

            Assert.True(result);
        }

        [Fact]
        public void ObjectsWithNullProperties_ShouldBeEqual()
        {
            var person1 = new Person { Name = "John", Age = 30, Address = null };
            var person2 = new Person { Name = "John", Age = 30, Address = null };

            bool result = _comparer.IsEqual(person1, person2);

            Assert.True(result);
        }

        [Fact]
        public void CompareObjectWithNull_ShouldReturnFalse()
        {
            var person1 = new Person { Name = "John", Age = 30 };
            Person person2 = null;

            bool result = _comparer.IsEqual(person1, person2);

            Assert.False(result);
        }
    }
}
