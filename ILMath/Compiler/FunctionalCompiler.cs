using System.Linq.Expressions;
using ILMath.Exception;
using ILMath.SyntaxTree;

namespace ILMath.Compiler;

/// <summary>
/// Compiles the expression using functional methods.
/// </summary>
public class FunctionalCompiler : ICompiler
{
    /// <summary>
    /// Compiles the syntax tree into a function.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="tree">The syntax tree to compile.</param>
    /// <returns>The evaluator.</returns>
    public Evaluator Compile(string name, INode tree)
    {
        return CompileSyntaxTree(tree);
    }

    private Evaluator CompileSyntaxTree(INode rootNode)
    {
        var compiledRoot = CompileNode(rootNode);
        return context => compiledRoot(context);
    }

    private Evaluator CompileNode(INode node)
    {
        return node switch
        {
            OperatorNode expressionNode => CompileOperatorNode(expressionNode),
            NumberNode numberNode => CompileNumberNode(numberNode),
            UnaryNode unaryNode => CompileUnaryNode(unaryNode),
            VariableNode variableNode => CompileVariableNode(variableNode),
            FunctionNode functionNode => CompileFunctionNode(functionNode),
            _ => throw new CompilerException($"Unknown node type: {node.GetType()}")
        };
    }

    private Evaluator CompileOperatorNode(OperatorNode operatorNode)
    {
        var left = operatorNode.Left;
        var right = operatorNode.Right;
        var @operator = operatorNode.Operator;
        var compiledLeft = CompileNode(left);
        var compiledRight = CompileNode(right);
        return @operator switch
        {
            OperatorType.Plus => context => compiledLeft(context) + compiledRight(context),
            OperatorType.Minus => context => compiledLeft(context) - compiledRight(context),
            OperatorType.Multiplication => context => compiledLeft(context) * compiledRight(context),
            OperatorType.Division => context => compiledLeft(context) / compiledRight(context),
            OperatorType.Modulo => context => compiledLeft(context) % compiledRight(context),
            OperatorType.Exponent => context => Math.Pow(compiledLeft(context), compiledRight(context)),
            _ => throw new CompilerException($"Unknown operator: {@operator}")
        };
    }
    
    private static Evaluator CompileNumberNode(NumberNode numberNode)
    {
        var value = numberNode.Value;
        return _ => value;
    }
    
    private Evaluator CompileUnaryNode(UnaryNode unaryNode)
    {
        var compiledChild = CompileNode(unaryNode.Child);
        return unaryNode.Operator switch
        {
            OperatorType.Plus => context => compiledChild(context),
            OperatorType.Minus => context => -compiledChild(context),
            _ => throw new CompilerException($"Unknown unary operator: {unaryNode.Operator}")
        };
    }
    
    private Evaluator CompileVariableNode(VariableNode variableNode)
    {
        var identifier = variableNode.Identifier;
        return context => context.GetVariable(identifier);
    }

    private Evaluator CompileFunctionNode(FunctionNode functionNode)
    {
        var parameters = functionNode.Parameters;
        var compiledParameters = parameters.Select(CompileNode).ToArray();
        return context =>
        {
            Span<double> values = stackalloc double[compiledParameters.Length];
            for (var i = 0; i < compiledParameters.Length; i++)
                values[i] = compiledParameters[i](context);
            return context.CallFunction(functionNode.Identifier, values);
        };
    }
}