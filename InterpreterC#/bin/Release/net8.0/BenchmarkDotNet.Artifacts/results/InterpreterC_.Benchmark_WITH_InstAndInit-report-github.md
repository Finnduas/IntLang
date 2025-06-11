```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.26100.4061/24H2/2024Update/HudsonValley)
AMD Ryzen 7 6800H with Radeon Graphics 3.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-OUYCQZ : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Runtime=.NET 8.0  InvocationCount=100000  IterationCount=50  
LaunchCount=1  WarmupCount=10  

```
| Method | Mean     | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------- |---------:|----------:|----------:|-------:|-------:|----------:|
| parser | 4.286 μs | 0.0407 μs | 0.0822 μs | 1.2600 | 0.0200 |  10.36 KB |
