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
    
    public override string ToString()
    {
        return $"Unary({Operator}, {Child})";
    }
}