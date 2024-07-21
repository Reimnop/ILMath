using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ILMath.Benchmark;

[SimpleJob(RuntimeMoniker.Net80)]
public class DynamicVsFunctionalEvaluation
{
    private IEvaluationContext context = null!;
    private Evaluator dynamicEvaluator = null!;
    private Evaluator functionalEvaluator = null!;

    [GlobalSetup]
    public void Setup()
    {
        context = EvaluationContext.CreateDefault();
        dynamicEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, true);
        functionalEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, false);
    }
    
    [Benchmark]
    public double Dynamic()
    {
        return dynamicEvaluator(context);
    }
    
    [Benchmark]
    public double Functional()
    {
        return functionalEvaluator(context);
    }
}