namespace ILMath.Test;

[TestClass]
public class HashTest
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.AreEqual(HashExpression("2 + 1 * 5"), HashExpression("2 + 1 * 5"));
    }
    
    [TestMethod]
    public void TestMethod2()
    {
        Assert.AreEqual(HashExpression("sin(pi)"), HashExpression("sin(pi)"));
    }
    
    [TestMethod]
    public void TestMethod3()
    {
        Assert.AreNotEqual(HashExpression("sin(pi / 2)"), HashExpression("sin(pi / 4)"));
    }
    
    [TestMethod]
    public void TestMethod4()
    {
        Assert.AreNotEqual(HashExpression("cos(pi / 4) * 8.0"), HashExpression("sin(pi / 4) * 8.0"));
    }
    
    [TestMethod]
    public void TestMethod5()
    {
        Assert.AreNotEqual(HashExpression("4.0 * sin(pi / 4) * 8.0 + 1.0"), HashExpression("4.0 * sin(pi / 4) * 8.0 + 2.0"));
    }
    
    private static int HashExpression(string expression)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var syntaxTree = parser.Parse();
        return syntaxTree.GetHashCode();
    }
}