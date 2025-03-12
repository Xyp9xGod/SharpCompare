# SharpCompare 🔍

[![NuGet](https://img.shields.io/nuget/v/SharpCompare.svg)](https://www.nuget.org/packages/SharpCompare/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/Xyp9xGod/SharpCompare/blob/main/License.txt)

A lightweight and efficient library for deep object comparison in C#, designed to simplify equality checks without relying on memory references. Perfect for testing, validation, and data synchronization scenarios.

---

## Table of Contents 📑
- [Features](#-features)
- [Installation](#-installation)
- [Usage](#-usage)
  - [Basic Comparison](#basic-object-comparison)
  - [Ignoring Properties](#ignoring-specific-properties)
  - [Collections Comparison](#comparing-collections)
- [Benchmarking](#-benchmarking)
- [License](#-license)
- [Contributing](#-contributing)
- [Contact](#-contact)

---

## 🚀 Features
- **Deep Object Comparison**: Recursively compare nested properties and fields.
- **Collection Support**: Works with `IEnumerable`, arrays, dictionaries, and more.
- **Custom Ignore Rules**: Use `[IgnoreComparison]` to exclude properties.
- **Optimized Performance**: DFS-based traversal with early termination on mismatches.
- **Benchmark-Ready**: Integrates with `BenchmarkDotNet` for performance analysis.

---

## 📦 Installation

Install via **NuGet Package Manager**:
```sh
PM> NuGet\Install-Package SharpCompare -Version 1.0.2
```

Or using the **.NET CLI**:
```sh
dotnet add package SharpCompare --version 1.0.2
```

---

## 🔥 Usage

### Basic Object Comparison
Compare two objects by value:
```csharp
using SharpCompare;

var comparer = SharpCompareFactory.Create();

var obj1 = new { Name = "Alice", Age = 30 };
var obj2 = new { Name = "Alice", Age = 30 };

bool isEqual = comparer.IsEqual(obj1, obj2); // Returns true
```

### Ignoring Specific Properties
Mark properties to ignore with `[IgnoreComparison]`:
```csharp
using SharpCompare;

public class User
{
    public string Name { get; set; }
    
    [IgnoreComparison]
    public Guid SessionId { get; set; } // Ignored during comparison
}

var user1 = new User { Name = "Alice", SessionId = Guid.NewGuid() };
var user2 = new User { Name = "Alice", SessionId = Guid.NewGuid() };

var comparer = SharpCompareFactory.Create();
bool result = comparer.IsEqual(user1, user2); // true (SessionId is ignored)
```

### Comparing Collections
Compare lists, dictionaries, and other collections:
```csharp
var comparer = SharpCompareFactory.Create();

var dict1 = new Dictionary<int, string> { { 1, "A" }, { 2, "B" } };
var dict2 = new Dictionary<int, string> { { 1, "A" }, { 2, "B" } };

bool areDictionariesEqual = comparer.IsEqual(dict1, dict2); // true
```

---

## 🏎️ Benchmarking
Measure performance using `BenchmarkDotNet`:

1. Add the benchmarking package:
```sh
dotnet add package BenchmarkDotNet
```

2. Create a benchmark class:
```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SharpCompare;

public class ComparisonBenchmark
{
    private readonly ISharpCompare _dfsComparer = SharpCompareFactory.Create(useDFS: true);
    private readonly ISharpCompare _reflectionComparer = SharpCompareFactory.Create(useDFS: false);

    private object _objA = new { Data = new List<int> { 1, 2, 3 } };
    private object _objB = new { Data = new List<int> { 1, 2, 3 } };

    [Benchmark]
    public bool CompareUsingDFS() => _dfsComparer.IsEqual(_objA, _objB);

    [Benchmark]
    public bool CompareUsingReflection() => _reflectionComparer.IsEqual(_objA, _objB);
}

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<ComparisonBenchmark>();
    }
}
```

3. Run the benchmark:
```sh
dotnet run --project SharpCompare.Benchmarks -c Release
```

---

## 📜 License
This project is licensed under the **MIT License** - see the [LICENSE](https://github.com/Xyp9xGod/SharpCompare/blob/main/License.txt) file for details.

---

## 🤝 Contributing
Contributions are welcome! Please:
1. Fork the repository.
2. Create a feature branch.
3. Submit a pull request.

GitHub Repository: [https://github.com/Xyp9xGod/SharpCompare](https://github.com/Xyp9xGod/SharpCompare)

---

## 📫 Contact
- **Issues & Suggestions**: [GitHub Issues](https://github.com/Xyp9xGod/SharpCompare/issues)
- **LinkedIn**: [Arilson Silva](https://www.linkedin.com/in/arilsonsilva/)
