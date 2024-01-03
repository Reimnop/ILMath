namespace ILMath.SyntaxTree;

public class OperatorNode : INode
{
    public OperatorType Operator { get; }
    
    public OperatorNode(OperatorType @operator)
    {
        Operator = @operator;
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        yield break;
    }
    
    public override string ToString()
    {
        return $"Operator({Operator})";
    }
    
    public override int GetHashCode()
    {
        return Operator.GetHashCode();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not OperatorNode other)
            return false;
        return Operator.Equals(other.Operator);
    }
}