namespace ILMath.SyntaxTree;

public class OperatorNode : INode
{
    public OperatorType Operator { get; }
    public INode Left { get; }
    public INode Right { get; }
    
    public OperatorNode(OperatorType @operator, INode left, INode right) {
        Operator = @operator;
        Left = left;
        Right = right;
    }
    
    public IEnumerable<INode> EnumerateChildren()
    {
        yield return Left;
        yield return Right;
    }

    public override string ToString()
    {
        return $"Operator({Operator}, {Left}, {Right})";
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Operator);
        hash.Add(Left);
        hash.Add(Right);
        return hash.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not OperatorNode other)
            return false;
        return Operator == other.Operator && Left.Equals(other.Left) && Right.Equals(other.Right);
    }
}