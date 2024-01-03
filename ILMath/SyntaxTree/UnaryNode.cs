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
    
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Operator);
        hash.Add(Child);
        return hash.ToHashCode();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not UnaryNode other)
            return false;
        return Operator.Equals(other.Operator) && Child.Equals(other.Child);
    }
}