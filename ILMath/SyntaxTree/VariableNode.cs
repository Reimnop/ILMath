namespace ILMath.SyntaxTree;

public class VariableNode : INode
{
    public string Identifier { get; }
    
    public VariableNode(string identifier)
    {
        Identifier = identifier;
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        yield break;
    }
    
    public override string ToString()
    {
        return $"Variable({Identifier})";
    }
    
    public override int GetHashCode()
    {
        return Identifier.GetHashCode();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not VariableNode other)
            return false;
        return Identifier.Equals(other.Identifier);
    }
}