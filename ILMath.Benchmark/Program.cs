using BenchmarkDotNet.Running;
using ILMath.Benchmark;

BenchmarkRunner.Run<DynamicVsFunctionalCompilation>();
BenchmarkRunner.Run<DynamicVsFunctionalEvaluation>();
BenchmarkRunner.Run<FunctionalNativeAot>();