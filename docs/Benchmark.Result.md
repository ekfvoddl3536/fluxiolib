# Benchmark Results
## Runtime Environment Info
```
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
12th Gen Intel Core i9-12900K, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.204
  [Host]   : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  
```

## Table
- `Direct`: Access private fields through public properties. This is a scenario where performance loss is completely non-existent.  
- `Reflection`: Access private fields using `System.Reflection`.  
  - `w/o cache`: Repeatedly queries the field each time it needs to be accessed.
  - `w/ cache`: Locates the field before starting the benchmark.
  - `w/ cache +noBoxUnbox`: Locates the field before starting the benchmark and suppresses *boxing* and *unboxing* in `GetValue` and `SetValue`.
- `FluxIO`: Access private fields using `fluxiolib`.  
  - `w/o cache`: Repeatedly queries the field each time it needs to be accessed.
  - `w/o name cmp`: Repeatedly queries the field each time it needs to be accessed, but does not search by the field's name.
  - `w/ reflection`: Repeatedly queries the field each time it needs to be accessed, using `System.Reflection`.
  - `w/ cache`: Locates the field before starting the benchmark.
  
| Method                                     | N    | Mean           | Error         | StdDev      |
|------------------------------------------- |----- |---------------:|--------------:|------------:|
| **Direct**                                     | **1**    |       **2.067 ns** |     **0.0081 ns** |   **0.0076 ns** |
| Reflection(w/o cache)                    | 1    |     240.134 ns |     1.4886 ns |   1.3925 ns |
| FluxIO(w/o cache)                        | 1    |      93.158 ns |     0.5302 ns |   0.4960 ns |
| Reflection(w/ cache)                     | 1    |     203.622 ns |     1.1923 ns |   1.1153 ns |
| Reflection(w/ cache +noBoxUnbox)         | 1    |     181.778 ns |     0.9892 ns |   0.9253 ns |
| FluxIO(w/o name cmp)                     | 1    |      34.047 ns |     0.1532 ns |   0.1433 ns |
| FluxIO(w/ reflection)                    | 1    |      29.747 ns |     0.1248 ns |   0.1107 ns |
| FluxIO(w/ cache)                         | 1    |       2.092 ns |     0.0137 ns |   0.0128 ns |
| **Direct**                                     | **10**   |      **31.265 ns** |     **0.1310 ns** |   **0.1225 ns** |
| Reflection(w/o cache)                    | 10   |   2,344.889 ns |    14.2207 ns |  12.6063 ns |
| FluxIO(w/o cache)                        | 10   |     888.928 ns |     3.3551 ns |   3.1384 ns |
| Reflection(w/ cache)                     | 10   |   2,028.743 ns |     8.6767 ns |   7.6917 ns |
| Reflection(w/ cache +noBoxUnbox)         | 10   |   1,965.770 ns |    12.2738 ns |  11.4810 ns |
| FluxIO(w/o name cmp)                     | 10   |     288.996 ns |     1.2380 ns |   1.0975 ns |
| FluxIO(w/ reflection)                    | 10   |     280.582 ns |     1.5721 ns |   1.4705 ns |
| FluxIO(w/ cache)                         | 10   |      31.368 ns |     0.1340 ns |   0.1254 ns |
| **Direct**                                     | **100**  |     **325.373 ns** |     **1.4319 ns** |   **1.2693 ns** |
| Reflection(w/o cache)                    | 100  |  23,883.049 ns |   110.5459 ns | 103.4047 ns |
| FluxIO(w/o cache)                        | 100  |   9,263.932 ns |    39.8737 ns |  37.2978 ns |
| Reflection(w/ cache)                     | 100  |  20,037.725 ns |   110.3330 ns |  97.8074 ns |
| Reflection(w/ cache +noBoxUnbox)         | 100  |  19,270.552 ns |    60.8898 ns |  56.9564 ns |
| FluxIO(w/o name cmp)                     | 100  |   3,352.054 ns |     9.7042 ns |   9.0773 ns |
| FluxIO(w/ reflection)                    | 100  |   2,757.813 ns |    12.8037 ns |  11.3501 ns |
| FluxIO(w/ cache)                         | 100  |     325.419 ns |     1.1952 ns |   1.0595 ns |
| **Direct**                                     | **1000** |   **3,269.418 ns** |    **16.7019 ns** |  **15.6230 ns** |
| Reflection(w/o cache)                    | 1000 | 234,333.610 ns |   888.5923 ns | 831.1898 ns |
| FluxIO(w/o cache)                        | 1000 |  93,232.839 ns |   381.3523 ns | 356.7171 ns |
| Reflection(w/ cache)                     | 1000 | 202,702.440 ns | 1,002.8055 ns | 938.0249 ns |
| Reflection(w/ cache +noBoxUnbox)         | 1000 | 190,516.761 ns |   859.9003 ns | 718.0557 ns |
| FluxIO(w/o name cmp)                     | 1000 |  37,549.836 ns |   109.0050 ns |  91.0241 ns |
| FluxIO(w/ reflection)                    | 1000 |  28,454.370 ns |   109.9704 ns |  97.4859 ns |
| FluxIO(w/ cache)                         | 1000 |   3,267.093 ns |    10.0056 ns |   9.3593 ns |
