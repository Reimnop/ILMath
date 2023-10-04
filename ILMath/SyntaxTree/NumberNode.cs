using System.Globalization;

namespace ILMath.SyntaxTree;

public class NumberNode : INode
{
    public double Value { get; }
    
    public NumberNode(double value)
    {
        Value = value;
    }
    
    public override string ToString()
    {
        return $"Number({Value.ToString(CultureInfo.InvariantCulture)})";
    }
}