using SharpCompare.Factory;
using SharpCompare.Interfaces;
using SharpCompare.Models;
using Xunit;

namespace SharpCompare.Tests.Net8.DFS
{
    public class SharpCompareDictionaryTests
    {
        private readonly ISharpCompare _comparer = SharpCompareFactory.Create(useDFS: true);

        [Fact]
        public void IsEqualDictionaries_ShouldReturnTrue()
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
        public void IsEqualDictionaries_ShouldReturnFalse()
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
        public void IsEqualDictionariesWithStringAndInt_ShouldReturnFalse_WhenDictionariesArDifferent()
        {
            var dict1 = new Dictionary<string, int>
            {
                { "A", 1 },
                { "B", 2 }
            };

            var dict2 = new Dictionary<string, int>
            {
                { "A", 3 },
                { "B", 4 }
            };

            bool result = _comparer.IsEqual(dict1, dict2);

            Assert.False(result);
        }
        
        [Fact]
        public void IsEqualDictionariesWithStringAndInt_ShouldReturnTrue_WhenDictionariesAreIdentical()
        {
            var dict1 = new Dictionary<string, int>
            {
                { "A", 1 },
                { "B", 2 }
            };

            var dict2 = new Dictionary<string, int>
            {
                { "A", 1 },
                { "B", 2 }
            };

            bool result = _comparer.IsEqual(dict1, dict2);

            Assert.True(result);
        }

        [Fact]
        public void GetDifferences_ShouldDetectDifferencesInDictionaries()
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

            var differences = _comparer.GetDifferences(dict1, dict2);

            Assert.Contains("B.Name: Carlos → Maria", differences);
            Assert.Contains("B.Age: 40 → 25", differences);
        }

        [Fact]
        public void IsEqualJson_ShouldReturnTrue_WhenDictionariesAreIdentical()
        {
            var dict1 = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };
            var dict2 = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };

            bool result = _comparer.IsEqualJson(dict1, dict2);

            Assert.True(result);
        }

        [Fact]
        public void IsEqualJson_ShouldReturnFalse_WhenDictionariesAreDifferent()
        {
            var dict1 = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };
            var dict2 = new Dictionary<string, int> { { "A", 1 }, { "B", 3 } };

            bool result = _comparer.IsEqualJson(dict1, dict2);

            Assert.False(result);
        }

        [Fact]
        public void CompareByHash_ShouldReturnTrue_WhenDictionariesAreIdentical()
        {
            var dict1 = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };
            var dict2 = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };

            bool result = _comparer.CompareByHash(dict1, dict2);

            Assert.True(result);
        }

        [Fact]
        public void CompareByHash_ShouldReturnFalse_WhenDictionariesAreDifferent()
        {
            var dict1 = new Dictionary<string, int> { { "A", 1 }, { "B", 2 } };
            var dict2 = new Dictionary<string, int> { { "A", 1 }, { "B", 3 } };

            bool result = _comparer.CompareByHash(dict1, dict2);

            Assert.False(result);
        }
    }
}