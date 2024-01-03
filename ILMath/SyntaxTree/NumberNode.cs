using System.Globalization;

namespace ILMath.SyntaxTree;

public class NumberNode : INode
{
    public double Value { get; }
    
    public NumberNode(double value)
    {
        Value = value;
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        yield break;
    }
    
    public override string ToString()
    {
        return $"Number({Value.ToString(CultureInfo.InvariantCulture)})";
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not NumberNode other)
            return false;
        return Value.Equals(other.Value);
    }
}