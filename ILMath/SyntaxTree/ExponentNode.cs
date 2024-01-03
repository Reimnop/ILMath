namespace ILMath.SyntaxTree;

public class ExponentNode : INode
{
    public INode Base { get; }
    public INode Exponent { get; }
    
    public ExponentNode(INode @base, INode exponent)
    {
        Base = @base;
        Exponent = exponent;
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        yield return Base;
        yield return Exponent;
    }

    public override string ToString()
    {
        return $"Exponent({Base}, {Exponent})";
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Base);
        hash.Add(Exponent);
        return hash.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ExponentNode other)
            return false;
        return Base.Equals(other.Base) && Exponent.Equals(other.Exponent);
    }
}