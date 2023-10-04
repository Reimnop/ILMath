namespace ILMath.SyntaxTree;

public interface INode
{
    IEnumerable<INode> EnumerateChildren();
}