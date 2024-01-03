using ILMath.SyntaxTree;

namespace ILMath.Test;

[TestClass]
public class EqualityTest
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.AreEqual(CreateSyntaxTree("2 + 1 * 5"), CreateSyntaxTree("2 + 1 * 5"));
    }
    
    [TestMethod]
    public void TestMethod2()
    {
        Assert.AreEqual(CreateSyntaxTree("sin(pi)"), CreateSyntaxTree("sin(pi)"));
    }
    
    [TestMethod]
    public void TestMethod3()
    {
        Assert.AreNotEqual(CreateSyntaxTree("sin(pi / 2)"), CreateSyntaxTree("sin(pi / 4)"));
    }
    
    [TestMethod]
    public void TestMethod4()
    {
        Assert.AreNotEqual(CreateSyntaxTree("cos(pi / 4) * 8.0"), CreateSyntaxTree("sin(pi / 4) * 8.0"));
    }
    
    [TestMethod]
    public void TestMethod5()
    {
        Assert.AreNotEqual(CreateSyntaxTree("4.0 * sin(pi / 4) * 8.0 + 1.0"), CreateSyntaxTree("4.0 * sin(pi / 4) * 8.0 + 2.0"));
    }
    
    private static INode CreateSyntaxTree(string expression)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var syntaxTree = parser.Parse();
        return syntaxTree;
    }
}