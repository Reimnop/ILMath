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
}