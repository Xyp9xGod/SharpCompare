using SharpCompare.Extensions;

namespace SharpCompare.Models
{
    public class Person
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public Address? Address { get; set; }
        [IgnoreComparison]
        public string? Password { get; set; }
    }
}
