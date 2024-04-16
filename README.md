# FluxIO
## What is this?
This library was created to support access to instance fields without the use of reflection.  
  
It includes the following features:  
- Fast search for instance fields
- Direct access to instance fields without boxing/unboxing
- Cost-free high-speed access to instance fields
   
**And, this project is being developed targeting 64-bit processors.**  
  
This is a project still under development.  
Please refer to [this table](./docs/Compatibility.md) to check compatibility with specific systems.  
  
  
## Obtaining Build Artifacts
- [한국어](./docs/ko/GetBuildArtifacts.md)  
- [English](./docs/en/GetBuildArtifacts.md)  
  
  
## How to Use
When searching for fields, the scope to be searched is evaluated as the highest priority, followed by the field's access modifier level and Type. The field name to be searched last is evaluated.  
  
The most basic way to use it. This method is similar to the existing Reflection, but it is not the recommended method.  
  
```csharp
// FILE: Foo.cs
class Foo
{
    public int age;
    protected float score;

    public float MyScore
    {
        get
        {
            return this.score;
        }
    }
}
```
```csharp
// FILE: Program.cs
using fluxiolib;

FluxRuntimeFieldDesc fd_score = FluxTool.GetInstanceField(typeof(Foo), "score"u8, ProtFlags.All);

if (fd_score.IsNull)  {
    // The field does not exist or the search failed.
    Console.WriteLine("Error!");
    return;
}

Foo foo = new Foo();

Console.Write("Before: ");
Console.WriteLine(foo.MyScore);

fd_score.FieldAccessor.Value<float>(foo) = 250;

Console.Write("After: ");
Console.WriteLine(foo.MyScore);

Console.Write("FieldOffset: ");
Console.WriteLine(fd_score.FieldOffset);

// Print:
//  Before: 0
//  After: 250
//  FieldOffset: 4
```
  
Alternatively, you can also create a field accessor from an already found `FieldInfo`.  
```csharp
// FILE: Program.cs
using fluxiolib;
using System.Reflection; // for 'FieldInfo' and 'BindingFlags'

FieldInfo? fieldInfo = typeof(Foo).GetField("score", BindingFlags.NonPublic | BindingFlags.Instance);
if (fieldInfo == null) {
    Console.WriteLine("Error!");
    return;
}

FluxRuntimeFieldDesc fd_score = FluxTool.ToRuntimeFieldDesc(fieldInfo);

Foo foo = new Foo();

Console.Write("Before: ");
Console.WriteLine(foo.MyScore);

fd_score.FieldAccessor.Value<float>(foo) = 250;

Console.Write("After: ");
Console.WriteLine(foo.MyScore);

Console.Write("FieldOffset: ");
Console.WriteLine(fd_score.FieldOffset);

// Print:
//  Before: 0
//  After: 250
//  FieldOffset: 4
```

More information on how to use it can be found [here](#usage-guide).  

## Performance
```
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900K, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.204
  [Host]   : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  
```
| Method                 | N    | Mean           | Error         | StdDev        |
|----------------------- |----- |---------------:|--------------:|--------------:|
| **ReflectionWithoutCache** | **1**    |     **250.908 ns** |     **1.5342 ns** |     **1.4351 ns** |
| FluxioWithoutCache     | 1    |      36.466 ns |     0.1675 ns |     0.1484 ns |
| Reflection             | 1    |     209.844 ns |     1.0083 ns |     0.9431 ns |
| Fluxio                 | 1    |       3.723 ns |     0.0256 ns |     0.0240 ns |
| **ReflectionWithoutCache** | **10**   |   **2,391.964 ns** |    **14.3021 ns** |    **13.3782 ns** |
| FluxioWithoutCache     | 10   |     341.973 ns |     2.2206 ns |     2.0772 ns |
| Reflection             | 10   |   2,023.494 ns |     7.1326 ns |     6.6718 ns |
| Fluxio                 | 10   |      31.637 ns |     0.1070 ns |     0.1001 ns |
| **ReflectionWithoutCache** | **100**  |  **23,915.672 ns** |    **67.7863 ns** |    **63.4074 ns** |
| FluxioWithoutCache     | 100  |   3,869.912 ns |    23.6135 ns |    22.0881 ns |
| Reflection             | 100  |  22,071.924 ns |   138.0956 ns |   129.1747 ns |
| Fluxio                 | 100  |     326.988 ns |     1.2908 ns |     1.0078 ns |
| **ReflectionWithoutCache** | **1000** | **242,442.337 ns** | **1,734.1919 ns** | **1,622.1642 ns** |
| FluxioWithoutCache     | 1000 |  43,822.103 ns |   712.8511 ns |   666.8014 ns |
| Reflection             | 1000 | 217,849.370 ns | 1,115.2665 ns |   988.6547 ns |
| Fluxio                 | 1000 |   3,340.424 ns |    14.2207 ns |    13.3021 ns |
  
Benchmark source code: [Zip Archive](./.benchmark/FluxIOLib.Benchmark.zip)  
   
Boxing and unboxing in reflection were considered to significantly affect performance, so the source code was modified to prevent boxing and unboxing during reflection and benchmarks were rerun, but no significant difference was found. 
   
 Method                    | N    | Mean         | Error       | StdDev      |
-------------------------- |----- |-------------:|------------:|------------:|
 **Reflection**                | **1**    |     **218.3 ns** |     **1.52 ns** |     **1.42 ns** |
 ReflectionWithoutBoxUnbox | 1    |     185.7 ns |     1.74 ns |     1.62 ns |
 **Reflection**                | **10**   |   **2,057.8 ns** |    **11.68 ns** |    **10.35 ns** |
 ReflectionWithoutBoxUnbox | 10   |   1,991.5 ns |    10.17 ns |     9.51 ns |
 **Reflection**                | **100**  |  **20,677.7 ns** |   **102.90 ns** |    **96.25 ns** |
 ReflectionWithoutBoxUnbox | 100  |  19,819.2 ns |    76.31 ns |    67.64 ns |
 **Reflection**                | **1000** | **207,219.7 ns** |   **827.38 ns** |   **733.45 ns** |
 ReflectionWithoutBoxUnbox | 1000 | 193,150.1 ns | 1,354.83 ns | 1,267.31 ns |


## Usage Guide
- [한국어](./docs/ko/HowToUse_Basic.md)
- [English](./docs/en/HowToUse_Basic.md)

## API Document
- [한국어](./docs/ko/API/fluxiolib.md)
- English (**Not ready yet**)
