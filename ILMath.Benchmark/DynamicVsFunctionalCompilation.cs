using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ILMath.Benchmark;

[SimpleJob(RuntimeMoniker.Net80)]
public class DynamicVsFunctionalCompilation
{
    private IEvaluationContext context = null!;

    [GlobalSetup]
    public void Setup()
    {
        context = EvaluationContext.CreateDefault();
    }
    
    [Benchmark]
    public Evaluator Dynamic()
    {
        return MathEvaluation.CompileExpression(string.Empty, Constant.Expression, true);
    }
    
    [Benchmark]
    public Evaluator Functional()
    {
        return MathEvaluation.CompileExpression(string.Empty, Constant.Expression, false);
    }
}