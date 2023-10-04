namespace ILMath.SyntaxTree;

public class ExpressionNode : INode
{
    public IReadOnlyList<INode> Children { get; }
    
    public ExpressionNode(IEnumerable<INode> children)
    {
        Children = children.ToList();
    }

    public override string ToString()
    {
        return $"Expression({string.Join(", ", Children)})";
    }
}