namespace ILMath.SyntaxTree;

public class UnaryNode : INode
{
    public OperatorType Operator { get; }
    public INode Child { get; }
    
    public UnaryNode(OperatorType @operator, INode child)
    {
        Operator = @operator;
        Child = child;
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        yield return Child;
    }

    public override string ToString()
    {
        return $"Unary({Operator}, {Child})";
    }
}