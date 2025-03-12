using BenchmarkDotNet.Attributes;
using SharpCompare.Factory;
using SharpCompare.Interfaces;

namespace SharpCompare.Benchmarks
{
    public class ComparisonBenchmark
    {
        private readonly ISharpCompare _dfsComparer = SharpCompareFactory.Create(useDFS: true);
        private readonly ISharpCompare _reflectionComparer = SharpCompareFactory.Create(useDFS: false);

        private Person _person1;
        private Person _person2;

        [GlobalSetup]
        public void Setup()
        {
            _person1 = new Person
            {
                Name = "John",
                Age = 30,
                Address = new Address { Street = "Main St", Number = 100 }
            };

            _person2 = new Person
            {
                Name = "John",
                Age = 30,
                Address = new Address { Street = "Main St", Number = 100 }
            };
        }

        [Benchmark]
        public bool CompareUsingReflection()
        {
            return _reflectionComparer.IsEqual(_person1, _person2);
        }

        [Benchmark]
        public bool CompareUsingDFS()
        {
            return _dfsComparer.IsEqual(_person1, _person2);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public int Number { get; set; }
    }

}
