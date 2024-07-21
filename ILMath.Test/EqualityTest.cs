using ILMath.SyntaxTree;

namespace ILMath.Test;

[TestClass]
public class EqualityTest
{
    [TestMethod]
    public void TestMethod1()
    {
        var expect = new OperatorNode(
            OperatorType.Plus, 
            new NumberNode(2.0), 
            new OperatorNode(
                OperatorType.Multiplication, 
                new NumberNode(1.0), 
                new NumberNode(5.0)));
        Assert.AreEqual(expect, CreateSyntaxTree("2 + 1 * 5"));
    }
    
    [TestMethod]
    public void TestMethod2()
    {
        var expect = new FunctionNode("sin", [ new VariableNode("pi") ]);
        Assert.AreEqual(expect, CreateSyntaxTree("sin(pi)"));
    }
    
    [TestMethod]
    public void TestMethod3()
    {
        var expect = new FunctionNode(
            "sin", [
                new OperatorNode(
                    OperatorType.Division, 
                    new VariableNode("pi"), 
                    new NumberNode(2.0))]);
        Assert.AreNotEqual(expect, CreateSyntaxTree("sin(pi / 4)"));
    }
    
    [TestMethod]
    public void TestMethod4()
    {
        var expect = new OperatorNode(
            OperatorType.Multiplication, 
            new FunctionNode(
                "sin", [
                    new OperatorNode(
                        OperatorType.Division, 
                        new VariableNode("pi"), 
                        new NumberNode(4.0))]), 
            new NumberNode(8.0));
        Assert.AreEqual(expect, CreateSyntaxTree("sin(pi / 4) * 8.0"));
    }
    
    [TestMethod]
    public void TestMethod5()
    {
        var expect = new NumberNode(5.0);
        Assert.AreEqual(expect, CreateSyntaxTree("5"));
    }
    
    [TestMethod]
    public void TestMethod6()
    {
        var expect = new OperatorNode(
            OperatorType.Exponent, 
            new NumberNode(4.0), 
            new NumberNode(7.0));
        Assert.AreEqual(expect, CreateSyntaxTree("4 ^ 7"));
    }
    
    [TestMethod]
    public void TestMethod7()
    {
        var expect = new OperatorNode(
            OperatorType.Exponent, 
            new NumberNode(4.0), 
            new OperatorNode(
                OperatorType.Plus, 
                new NumberNode(7.0), 
                new NumberNode(2.0)));
        Assert.AreEqual(expect, CreateSyntaxTree("4 ^ (7 + 2)"));
    }
    
    [TestMethod]
    public void TestMethod8()
    {
        var expect = new OperatorNode(
            OperatorType.Exponent, 
            new NumberNode(4.0), 
            new OperatorNode(
                OperatorType.Exponent, 
                new OperatorNode(
                    OperatorType.Plus, 
                    new NumberNode(7.0), 
                    new NumberNode(2.0)), 
                new NumberNode(1.5)));
        Assert.AreEqual(expect, CreateSyntaxTree("4 ^ (7 + 2) ^ 1.5"));
    }
    
    private static INode CreateSyntaxTree(string expression)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var syntaxTree = parser.Parse();
        return syntaxTree;
    }
}