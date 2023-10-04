namespace ILMath.SyntaxTree;

public class TermNode : INode
{
    public IReadOnlyList<INode> Children { get; }
    
    public TermNode(IEnumerable<INode> children)
    {
        Children = children.ToList();
    }
    
    public override string ToString()
    {
        return $"Term({string.Join(", ", Children)})";
    }
}