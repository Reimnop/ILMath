using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ILMath.Benchmark;

[SimpleJob(RuntimeMoniker.NativeAot80)]
public class CompilationMethodsNativeAotBenchmark
{
    private IEvaluationContext context = null!;
    private Evaluator expressionTreeEvaluator = null!;
    private Evaluator functionalEvaluator = null!;

    [GlobalSetup]
    public void Setup()
    {
        context = EvaluationContext.CreateDefault(); 
        expressionTreeEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.ExpressionTree);
        functionalEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.Functional);
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
    
    [Benchmark]
    public double ExpressionTreeEvaluation()
    {
        return expressionTreeEvaluator(context);
    }
    
    [Benchmark]
    public double FunctionalEvaluation()
    {
        return functionalEvaluator(context);
    }
}