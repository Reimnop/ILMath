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
}