using BenchmarkDotNet.Running;
using ILMath.Benchmark;

BenchmarkRunner.Run<CompilationMethodsCompilationBenchmark>();
BenchmarkRunner.Run<CompilationMethodsEvaluationBenchmark>();
BenchmarkRunner.Run<CompilationMethodsNativeAotBenchmark>();