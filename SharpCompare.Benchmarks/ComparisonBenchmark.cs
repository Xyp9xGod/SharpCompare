using BenchmarkDotNet.Attributes;
using SharpCompare.Factory;
using SharpCompare.Interfaces;

namespace SharpCompare.Benchmarks
{
    [MemoryDiagnoser]
    public class ComparisonBenchmark
    {
        private readonly ISharpCompare _dfsComparer = SharpCompareFactory.Create(useDFS: true);
        private readonly ISharpCompare _reflectionComparer = SharpCompareFactory.Create(useDFS: false);

        private ComplexPerson _person1;
        private ComplexPerson _person2;

        [GlobalSetup]
        public void Setup()
        {
            _person1 = new ComplexPerson
            {
                Name = "Alice",
                Age = 35,
                Address = new Address
                {
                    Street = "5th Avenue",
                    Number = 200
                },
                PhoneNumbers = new List<string> { "123456789", "987654321" },
                Metadata = new Dictionary<string, string>
                {
                    { "ID", "001" },
                    { "Status", "Active" }
                },
                Scores = new List<int> { 90, 85, 88 },
                Preferences = new HashSet<string> { "Dark Mode", "Notifications" },
                NestedFamily = new Family
                {
                    Relatives = new List<Person>
                    {
                        new Person { Name = "Bob", Age = 40, Address = new Address { Street = "Elm St", Number = 300 } },
                        new Person { Name = "Charlie", Age = 25, Address = new Address { Street = "Oak St", Number = 400 } }
                    }
                }
            };

            _person2 = new ComplexPerson
            {
                Name = "Alice",
                Age = 35,
                Address = new Address
                {
                    Street = "5th Avenue",
                    Number = 200
                },
                PhoneNumbers = new List<string> { "123456789", "987654321" },
                Metadata = new Dictionary<string, string>
                {
                    { "ID", "001" },
                    { "Status", "Active" }
                },
                Scores = new List<int> { 90, 85, 88 },
                Preferences = new HashSet<string> { "Dark Mode", "Notifications" },
                NestedFamily = new Family
                {
                    Relatives = new List<Person>
                    {
                        new Person { Name = "Bob", Age = 40, Address = new Address { Street = "Elm St", Number = 300 } },
                        new Person { Name = "Charlie", Age = 25, Address = new Address { Street = "Oak St", Number = 400 } }
                    }
                }
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

    public class ComplexPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public List<int> Scores { get; set; }
        public HashSet<string> Preferences { get; set; }
        public Family NestedFamily { get; set; }
    }

    public class Family
    {
        public List<Person> Relatives { get; set; }
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
