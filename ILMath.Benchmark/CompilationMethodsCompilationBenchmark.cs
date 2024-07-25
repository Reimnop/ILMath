using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ILMath.Benchmark;

[SimpleJob(RuntimeMoniker.Net80)]
public class CompilationMethodsCompilationBenchmark
{
    private IEvaluationContext context = null!;

    [GlobalSetup]
    public void Setup()
    {
        context = EvaluationContext.CreateDefault();
    }
    
    [Benchmark]
    public Evaluator IntermediateLanguageCompilation()
    {
        return MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.IntermediateLanguage);
    }

    [Benchmark]
    public Evaluator ExpressionTreeCompilation()
    {
        return MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.ExpressionTree);
    }

    [Benchmark]
    public Evaluator FunctionalCompilation()
    {
        return MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.Functional);
    }
}