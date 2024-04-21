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
  
## Performance
### Benchmark Result Summary
- `Direct`: Access private fields through public properties. This is a scenario where performance loss is completely non-existent.   
- `Reflection`: Access private fields using `System.Reflection`.  
- `FluxIO`: Access private fields using `fluxiolib`.  
  
| Method                                     | N    | Mean           | Error         | StdDev      |
|------------------------------------------- |----- |---------------:|--------------:|------------:|
| **Direct**                                     | **1**    |       **2.067 ns** |     **0.0081 ns** |   **0.0076 ns** |
| Reflection                               | 1    |     181.778 ns |     0.9892 ns |   0.9253 ns |
| FluxIO                                   | 1    |       2.092 ns |     0.0137 ns |   0.0128 ns |
| **Direct**                                     | **10**   |      **31.265 ns** |     **0.1310 ns** |   **0.1225 ns** |
| Reflection                               | 10   |   1,965.770 ns |    12.2738 ns |  11.4810 ns |
| FluxIO                                   | 10   |      31.368 ns |     0.1340 ns |   0.1254 ns |
| **Direct**                                     | **100**  |     **325.373 ns** |     **1.4319 ns** |   **1.2693 ns** |
| Reflection                               | 100  |  19,270.552 ns |    60.8898 ns |  56.9564 ns |
| FluxIO                                   | 100  |     325.419 ns |     1.1952 ns |   1.0595 ns |
| **Direct**                                     | **1000** |   **3,269.418 ns** |    **16.7019 ns** |  **15.6230 ns** |
| Reflection                               | 1000 | 190,516.761 ns |   859.9003 ns | 718.0557 ns |
| FluxIO                                   | 1000 |   3,267.093 ns |    10.0056 ns |   9.3593 ns |
  
Benchmark source code: [Zip Archive](./.benchmark/FluxIOLib.Benchmark.zip)  
  
Full benchmark results can be viewed [here](./docs/Benchmark.Result.md).  
  
  
## Obtaining Build Artifacts
- [한국어](./docs/ko/GetBuildArtifacts.md)  
- [English](./docs/en/GetBuildArtifacts.md)  
  
  
## How to Use
This is the recommended way to use it for a single field:  
```csharp
// FILE: Foo.cs
class Foo
{
    public int age;
    protected float score;
    
    // Property to retrieve the score
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
Although there is a built-in field search feature, the performance variability is much greater than searching fields using Reflection.  
   
Therefore, while the basic guide will include instructions on using the built-in field search feature, using it as shown in the example above is best for a single field.  
   
More information on how to use it can be found [here](#usage-guide).  
  
## Usage Guide
- [한국어](./docs/ko/HowToUse_Basic.md)
- [English](./docs/en/HowToUse_Basic.md)

## API Document
- [한국어](./docs/ko/API/fluxiolib.md)
- English (**Not ready yet**)
