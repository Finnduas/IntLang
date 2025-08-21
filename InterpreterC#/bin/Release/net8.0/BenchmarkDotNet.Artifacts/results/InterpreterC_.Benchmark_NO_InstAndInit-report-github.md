```

BenchmarkDotNet v0.15.1, Windows 11 (10.0.26100.4652/24H2/2024Update/HudsonValley)
AMD Ryzen 7 6800H with Radeon Graphics 3.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 [AttachedDebugger]
  Job-CWMKVH : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Runtime=.NET 8.0  InvocationCount=100000000  IterationCount=50  
LaunchCount=1  WarmupCount=15  

```
| Method      | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------------ |---------:|----------:|----------:|-------:|----------:|
| parser_pure | 4.249 ns | 0.1220 ns | 0.2465 ns | 0.0038 |      32 B |
