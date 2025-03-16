using SharpCompare.Interfaces;
using SharpCompare.Factory;
using SharpCompare.Models;

namespace SharpCompare.Tests.DFS
{
    public class SharpCompareCollectionTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: true);

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
        public void CompareList_ShouldReturnFalse()
        {
            var list1 = new List<int>
            {
                1, 2, 4, 5, 6
            };

            var list2 = new List<int>
            {
                7, 2, 4, 5, 8
            };

            bool result = _comparer.IsEqual(list1, list2);

            Assert.False(result);
        }

        [Fact]
        public void CompareList_ShouldReturnTrue()
        {
            var list1 = new List<int>
            {
                1, 2, 3, 4, 5
            };

            var list2 = new List<int>
            {
                1, 2, 3, 4, 5
            };

            bool result = _comparer.IsEqual(list1, list2);

            Assert.True(result);
        }
    }
}
