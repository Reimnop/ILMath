namespace ILMath.Test;

[TestClass]
public class EvaluationTest
{
    [TestMethod]
    public void TestMethod1()
    {
        RunTest("2 + 1 * 5", 7.0);
    }
    
    [TestMethod]
    public void TestMethod2()
    {
        RunTest("sin(pi)", 0.0);
    }
    
    [TestMethod]
    public void TestMethod3()
    {
        RunTest("sin(pi / 2)", 1.0);
    }
    
    [TestMethod]
    public void TestMethod4()
    {
        RunTest("cos(pi / 4) * 8.0", Math.Sqrt(2.0) * 4.0);
    }
    
    [TestMethod]
    public void TestMethod5()
    {
        RunTest("4.0 * sin(pi / 4) * 8.0 + 1.0", Math.Sqrt(2.0) * 16.0 + 1.0);
    }
    
    [TestMethod]
    public void TestMethod6()
    {
        RunTest("4 ^ 7", Math.Pow(4.0, 7.0));
    }
    
    [TestMethod]
    public void TestMethod7()
    {
        RunTest("4 ^ (7 + 2)", Math.Pow(4.0, 7.0 + 2.0));
    }
    
    [TestMethod]
    public void TestMethod8()
    {
        RunTest("4 ^ (7 + 2) ^ 1.5", Math.Pow(4.0, Math.Pow(7.0 + 2.0, 1.5)));
    }

    private static void RunTest(string expression, double expected)
    {
        foreach (var method in Enum.GetValues<CompilationMethod>())
        {
            var compiled = MathEvaluation.CompileExpression(string.Empty, expression, method);
            var context = CreateEvaluationContext();
            Assert.AreEqual(expected, compiled(context), 0.00001, $"Test failed for compilation method: {method}");
        }
    }

    private static IEvaluationContext CreateEvaluationContext()
    {
        return EvaluationContext.CreateDefault();
    }
}