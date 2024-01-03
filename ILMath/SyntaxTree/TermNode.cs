namespace ILMath.SyntaxTree;

public class TermNode : INode
{
    public IReadOnlyList<INode> Children { get; }
    
    public TermNode(IEnumerable<INode> children)
    {
        Children = children.ToList();
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        return Children;
    }
    
    public override string ToString()
    {
        return $"Term({string.Join(", ", Children)})";
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var child in Children)
            hash.Add(child);
        return hash.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TermNode other)
            return false;
        if (Children.Count != other.Children.Count)
            return false;
        var equals = true;
        for (var i = 0; i < Children.Count; i++)
            equals &= Children[i].Equals(other.Children[i]);
        return equals;
    }
}