using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ILMath.Benchmark;

[SimpleJob(RuntimeMoniker.NativeAot80)]
public class FunctionalNativeAot
{
    private IEvaluationContext context = null!;
    private Evaluator functionalEvaluator = null!;

    [GlobalSetup]
    public void Setup()
    {
        context = EvaluationContext.CreateDefault(); 
        functionalEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, false);
    }
    
    [Benchmark]
    public Evaluator Compilation()
    {
        return MathEvaluation.CompileExpression(string.Empty, Constant.Expression, false);
    }
    
    [Benchmark]
    public double Evaluation()
    {
        return functionalEvaluator(context);
    }
}