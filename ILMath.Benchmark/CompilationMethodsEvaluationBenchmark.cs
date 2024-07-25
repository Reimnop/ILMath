using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ILMath.Benchmark;

[SimpleJob(RuntimeMoniker.Net80)]
public class CompilationMethodsEvaluationBenchmark
{
    private IEvaluationContext context = null!;
    private Evaluator ilEvaluator = null!;
    private Evaluator expressionTreeEvaluator = null!;
    private Evaluator functionalEvaluator = null!;

    [GlobalSetup]
    public void Setup()
    {
        context = EvaluationContext.CreateDefault();
        ilEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.IntermediateLanguage);
        expressionTreeEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.ExpressionTree);
        functionalEvaluator = MathEvaluation.CompileExpression(string.Empty, Constant.Expression, CompilationMethod.Functional);
    }
    
    [Benchmark]
    public double IntermediateLanguageEvaluation()
    {
        return ilEvaluator(context);
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