namespace ILMath.Test;

[TestClass]
public class EvaluationTest
{
    [TestMethod]
    public void TestMethod1()
    {
        var evaluator = CreateEvaluator("2 + 1 * 5");
        var context = CreateEvaluationContext();
        Assert.AreEqual(7.0, evaluator(context), 0.00001);
    }
    
    [TestMethod]
    public void TestMethod2()
    {
        var evaluator = CreateEvaluator("sin(pi)");
        var context = CreateEvaluationContext();
        Assert.AreEqual(0.0, evaluator(context), 0.00001);
    }
    
    [TestMethod]
    public void TestMethod3()
    {
        var evaluator = CreateEvaluator("sin(pi / 2)");
        var context = CreateEvaluationContext();
        Assert.AreEqual(1.0, evaluator(context), 0.00001);
    }
    
    [TestMethod]
    public void TestMethod4()
    {
        var evaluator = CreateEvaluator("cos(pi / 4) * 8.0");
        var context = CreateEvaluationContext();
        Assert.AreEqual(Math.Sqrt(2.0) * 4.0, evaluator(context), 0.00001);
    }
    
    [TestMethod]
    public void TestMethod5()
    {
        var evaluator = CreateEvaluator("4.0 * sin(pi / 4) * 8.0 + 1.0");
        var context = CreateEvaluationContext();
        Assert.AreEqual(Math.Sqrt(2.0) * 16.0 + 1.0, evaluator(context), 0.00001);
    }

    private static Evaluator CreateEvaluator(string expression)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var syntaxTree = parser.Parse();
        var compiler = new Compiler(syntaxTree);
        return compiler.Compile(string.Empty);
    }

    private static IEvaluationContext CreateEvaluationContext()
    {
        return EvaluationContext.CreateDefault();
    }
}