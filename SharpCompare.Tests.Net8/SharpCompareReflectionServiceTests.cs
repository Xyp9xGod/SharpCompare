using SharpCompare.Factory;
using SharpCompare.Interfaces;
using SharpCompare.Models;
using Xunit;

namespace SharpCompare.Tests.Net8
{
    public class SharpCompareReflectionServiceTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: false);

        [Fact]
        public void IsEqual_SameValues_ReturnsTrue()
        {
            var firstPerson = new Person { Name = "Jon", Age = 30 };
            var secondPerson = new Person { Name = "Jon", Age = 30 };

            Assert.True(_comparer.IsEqual(firstPerson, secondPerson));
        }

        [Fact]
        public void IsEqual_DifferentValues_ReturnsFalse()
        {
            var firstPerson = new Person { Name = "Jon", Age = 30 };
            var secondPerson = new Person { Name = "Mary", Age = 25 };

            Assert.False(_comparer.IsEqual(firstPerson, secondPerson));
        }

        [Fact]
        public void IsEqual_NullObject_ReturnsFalse()
        {
            var person = new Person { Name = "Jon", Age = 30 };

            Assert.False(_comparer.IsEqual(person, null));
        }

        [Fact]
        public void IsEqual_DifferentTypes_ReturnsFalse()
        {
            var person = new Person { Name = "Jon", Age = 30 };
            var student = new Student { Name = "Jon", Age = 30 };

            Assert.False(_comparer.IsEqual(person, student));
        }

        [Fact]
        public void IsEqual_IgnoreProperty_ReturnsTrue()
        {
            var firstPerson = new Person { Name = "Jon", Age = 30, Password = "1234" };
            var secondPerson = new Person { Name = "Jon", Age = 30, Password = "5678" };

            Assert.True(_comparer.IsEqual(firstPerson, secondPerson));
        }

        [Fact]
        public void IsEqual_CompareLists_ReturnsTrue()
        {
            var firstFamily = new Family
            {
                LastName = "Meyers",
                Members = new List<Person>
                {
                    new Person { Name = "Jon", Age = 30 },
                    new Person { Name = "Mary", Age = 25 }
                }
            };

            var secondFamily = new Family
            {
                LastName = "Meyers",
                Members = new List<Person>
                {
                    new Person { Name = "Jon", Age = 30 },
                    new Person { Name = "Mary", Age = 25 }
                }
            };

            Assert.True(_comparer.IsEqual(firstFamily, secondFamily));
        }

        [Fact]
        public void IsEqual_CompareLists_ReturnsFalse()
        {
            var firstFamily = new Family
            {
                LastName = "Meyers",
                Members = new List<Person>
                {
                    new Person { Name = "Jon", Age = 30 },
                    new Person { Name = "Mary", Age = 25 }
                }
            };

            var secondFamily = new Family
            {
                LastName = "Meyers",
                Members = new List<Person>
                {
                    new Person { Name = "Jon", Age = 30 },
                    new Person { Name = "Carlos", Age = 40 }
                }
            };

            Assert.False(_comparer.IsEqual(firstFamily, secondFamily));
        }

        [Fact]
        public void GetDifferences_ShouldReturnEmptyList_WhenObjectsAreEqual()
        {
            var firstPerson = new { Name = "Alice", Age = 30 };
            var secondPerson = new { Name = "Alice", Age = 30 };

            var differences = _comparer.GetDifferences(firstPerson, secondPerson);

            Assert.Empty(differences);
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
        public void GetDifferences_ShouldHandleNullValues()
        {
            var firstPerson = new Person { Name = "Alice", Age = (int?)null };
            var secondPerson = new Person { Name = "Alice", Age = 30 };

            var differences = _comparer.GetDifferences(firstPerson, secondPerson);

            Assert.Contains("Age:  → 30", differences);
        }

        [Fact]
        public void GetDifferences_ShouldReturnTypeDifference_WhenObjectsAreOfDifferentTypes()
        {
            var person = new Person { Name = "Alice" };
            var student = new Student { Name = "Alice" };

            var differences = _comparer.GetDifferences(person, student);

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
    }
}
