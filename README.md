# SharpCompare

![NuGet](https://www.nuget.org/packages/SharpCompare/)
![Build](https://img.shields.io/github/actions/workflow/status/seu-repo/sharpcompare/build.yml?style=for-the-badge)
![License](https://github.com/Xyp9xGod/SharpCompare/blob/CompareObjects/License.txt)

SharpCompare is a powerful C# library for comparing objects based on their properties and values, ignoring memory references. It supports deep comparisons, collections, and custom rules for ignoring specific properties.

## 🚀 Features

- **Deep comparison** of objects, including nested properties.
- **Supports collections** like lists and dictionaries.
- **Efficient comparison using DFS**, stopping early when a difference is found.
- **Custom property ignoring** via `[IgnoreComparison]` attribute.
- **Benchmarking support** to compare different comparison strategies.

---

## 📦 Installation

Install via NuGet Package Manager:

```sh
PM> Install-Package SharpCompare
```

Or using .NET CLI:

```sh
dotnet add package SharpCompare
```

---

## 🔥 Usage

### Basic Object Comparison

```csharp
using SharpCompare;

var comparer = new SharpCompareService();

var obj1 = new { Name = "Alice", Age = 30 };
var obj2 = new { Name = "Alice", Age = 30 };

bool isEqual = comparer.IsEqual(obj1, obj2); // Returns TRUE
```

### Ignoring Specific Properties

Use the `[IgnoreComparison]` attribute to exclude properties from comparison.

```csharp
using SharpCompare;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    [IgnoreComparison] // This field will be ignored
    public string TemporaryId { get; set; }
}

var person1 = new Person { Name = "John", Age = 30, TemporaryId = "ABC123" };
var person2 = new Person { Name = "John", Age = 30, TemporaryId = "XYZ789" };

var comparer = new SharpCompareService();
bool result = comparer.IsEqual(person1, person2); // Returns TRUE
```

### Comparing Collections

```csharp
using SharpCompare;

var list1 = new List<int> { 1, 2, 3 };
var list2 = new List<int> { 1, 2, 3 };

var comparer = new SharpCompareService();
bool isEqual = comparer.IsEqual(list1, list2); // Returns TRUE
```

---

## 🏎️ Benchmarking

To compare performance between **Reflection-based** and **DFS-based** comparison, use `BenchmarkDotNet`.

```sh
dotnet add package BenchmarkDotNet
```

Create a benchmark class:

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SharpCompare;

public class CompareBenchmark
{
    private readonly SharpCompareService comparer = new();
    private readonly object obj1 = new { Name = "Alice", Age = 30 };
    private readonly object obj2 = new { Name = "Alice", Age = 30 };

    [Benchmark]
    public void CompareUsingDFS()
    {
        comparer.IsEqual(obj1, obj2);
    }
}

BenchmarkRunner.Run<CompareBenchmark>();
```

Run the benchmark with:

```sh
dotnet run -c Release
```

---

## 📜 License

SharpCompare is licensed under the **MIT License**.

---

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a pull request on GitHub.

---

## 📫 Contact

For questions or suggestions, open an issue or reach out on LinkedIn.

