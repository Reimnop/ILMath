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

    public override string ToString()
    {
        return $"Exponent({Base}, {Exponent})";
    }
}