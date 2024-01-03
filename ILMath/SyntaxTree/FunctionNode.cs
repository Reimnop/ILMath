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
    
    public IEnumerable<INode> EnumerateChildren()
    {
        return Parameters;
    }
    
    public override string ToString()
    {
        return $"Function({Identifier}, {string.Join(", ", Parameters)})";
    }
    
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Identifier);
        foreach (var parameter in Parameters)
            hash.Add(parameter);
        return hash.ToHashCode();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not FunctionNode other)
            return false;
        if (Identifier != other.Identifier)
            return false;
        if (Parameters.Count != other.Parameters.Count)
            return false;
        var equals = true;
        for (var i = 0; i < Parameters.Count; i++)
            equals &= Parameters[i].Equals(other.Parameters[i]);
        return equals;
    }
}