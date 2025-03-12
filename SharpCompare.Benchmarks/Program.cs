using BenchmarkDotNet.Running;
using SharpCompare.Benchmarks;

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<ComparisonBenchmark>();
    }
}