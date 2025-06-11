```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.26100.4061/24H2/2024Update/HudsonValley)
AMD Ryzen 7 6800H with Radeon Graphics 3.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method       | Mean     | Error    | StdDev   | Allocated |
|------------- |---------:|---------:|---------:|----------:|
| LinearSearch | 30.07 ns | 0.298 ns | 0.264 ns |         - |
| BinarySearch | 15.20 ns | 0.184 ns | 0.172 ns |         - |
