namespace ILMath.SyntaxTree;

public class VariableNode : INode
{
    public string Identifier { get; }
    
    public VariableNode(string identifier)
    {
        Identifier = identifier;
    }
    
    public override string ToString()
    {
        return $"Variable({Identifier})";
    }
}