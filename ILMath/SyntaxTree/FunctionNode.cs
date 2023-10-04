namespace ILMath.SyntaxTree;

public class FunctionNode : INode
{
    public string Identifier { get; }
    public IReadOnlyList<INode> Parameters { get; }
    
    public FunctionNode(string identifier, IEnumerable<INode> parameters)
    {
        Identifier = identifier;
        Parameters = parameters.ToList();
    }
    
    public override string ToString()
    {
        return $"Function({Identifier}, {string.Join(", ", Parameters)})";
    }
}