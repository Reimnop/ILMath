namespace ILMath.SyntaxTree;

public class OperatorNode : INode
{
    public OperatorType Operator { get; }
    
    public OperatorNode(OperatorType @operator)
    {
        Operator = @operator;
    }
    
    public override string ToString()
    {
        return $"Operator({Operator})";
    }
}