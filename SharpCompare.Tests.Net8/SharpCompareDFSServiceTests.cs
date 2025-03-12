using SharpCompare.Extensions;
using SharpCompare.Factory;
using SharpCompare.Interfaces;
using Xunit;

namespace SharpCompare.Tests.Net8
{
    public class SharpCompareDFSServiceTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: true);

        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }

            [IgnoreComparison]
            public string Ignored { get; set; }
        }

        private class Address
        {
            public string Street { get; set; }
            public int Number { get; set; }
        }

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

        [Fact]
        public void ObjectsWithIgnoredProperty_ShouldBeEqual()
        {
            var person1 = new Person { Name = "John", Age = 30, Ignored = "Value1" };
            var person2 = new Person { Name = "John", Age = 30, Ignored = "Value2" };

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

        [Fact]
        public void CompareEqualCollections_ShouldReturnTrue()
        {
            var list1 = new List<Person>
            {
                new Person { Name = "John", Age = 30 },
                new Person { Name = "Maria", Age = 25 }
            };

            var list2 = new List<Person>
            {
                new Person { Name = "John", Age = 30 },
                new Person { Name = "Maria", Age = 25 }
            };

            bool result = _comparer.IsEqual(list1, list2);

            Assert.True(result);
        }

        [Fact]
        public void CompareDifferentCollections_ShouldReturnFalse()
        {
            var list1 = new List<Person>
            {
                new Person { Name = "John", Age = 30 },
                new Person { Name = "Carlos", Age = 40 }
            };

            var list2 = new List<Person>
            {
                new Person { Name = "John", Age = 30 },
                new Person { Name = "Maria", Age = 25 }
            };

            bool result = _comparer.IsEqual(list1, list2);

            Assert.False(result);
        }

        [Fact]
        public void CompareEqualDictionaries_ShouldReturnTrue()
        {
            var dict1 = new Dictionary<string, Person>
            {
                { "A", new Person { Name = "John", Age = 30 } },
                { "B", new Person { Name = "Maria", Age = 25 } }
            };

            var dict2 = new Dictionary<string, Person>
            {
                { "A", new Person { Name = "John", Age = 30 } },
                { "B", new Person { Name = "Maria", Age = 25 } }
            };

            bool result = _comparer.IsEqual(dict1, dict2);

            Assert.True(result);
        }

        [Fact]
        public void CompareDifferentDictionaries_ShouldReturnFalse()
        {
            var dict1 = new Dictionary<string, Person>
            {
                { "A", new Person { Name = "John", Age = 30 } },
                { "B", new Person { Name = "Carlos", Age = 40 } }
            };

            var dict2 = new Dictionary<string, Person>
            {
                { "A", new Person { Name = "John", Age = 30 } },
                { "B", new Person { Name = "Maria", Age = 25 } }
            };

            bool result = _comparer.IsEqual(dict1, dict2);

            Assert.False(result);
        }

        [Fact]
        public void CompareDifferentTypes_ShouldReturnFalse()
        {
            var person = new Person { Name = "John", Age = 30 };
            var address = new Address { Street = "Street A", Number = 100 };

            bool result = _comparer.IsEqual(person, address);

            Assert.False(result);
        }
    }
}
