using System.Linq.Expressions;
using System.Reflection;
using ILMath.Exception;
using ILMath.SyntaxTree;

namespace ILMath.Compiler;

/// <summary>
/// Compiles the expression using expression trees.
/// </summary>
public class ExpressionTreeCompiler : ICompiler
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
        var parameter = Expression.Parameter(typeof(IEvaluationContext));
        var compiledExpressionTree = CompileNode(rootNode, parameter);
        return Expression.Lambda<Evaluator>(compiledExpressionTree, parameter).Compile();
    }

    private Expression CompileNode(INode node, ParameterExpression parameter)
    {
        return node switch
        {
            OperatorNode expressionNode => CompileOperatorNode(expressionNode, parameter),
            NumberNode numberNode => CompileNumberNode(numberNode, parameter),
            UnaryNode unaryNode => CompileUnaryNode(unaryNode, parameter),
            VariableNode variableNode => CompileVariableNode(variableNode, parameter),
            FunctionNode functionNode => CompileFunctionNode(functionNode, parameter),
            _ => throw new CompilerException($"Unknown node type: {node.GetType()}")
        };
    }

    private Expression CompileOperatorNode(OperatorNode operatorNode, ParameterExpression parameter)
    {
        var left = operatorNode.Left;
        var right = operatorNode.Right;
        var @operator = operatorNode.Operator;
        var compiledLeft = CompileNode(left, parameter);
        var compiledRight = CompileNode(right, parameter);
        return @operator switch
        {
            OperatorType.Plus => Expression.Add(compiledLeft, compiledRight),
            OperatorType.Minus => Expression.Subtract(compiledLeft, compiledRight),
            OperatorType.Multiplication => Expression.Multiply(compiledLeft, compiledRight),
            OperatorType.Division => Expression.Divide(compiledLeft, compiledRight),
            OperatorType.Modulo => Expression.Modulo(compiledLeft, compiledRight),
            OperatorType.Exponent => Expression.Power(compiledLeft, compiledRight),
            _ => throw new CompilerException($"Unknown operator: {@operator}")
        };
    }
    
    private static Expression CompileNumberNode(NumberNode numberNode, ParameterExpression _)
    {
        return Expression.Constant(numberNode.Value);
    }
    
    private Expression CompileUnaryNode(UnaryNode unaryNode, ParameterExpression parameter)
    {
        var compiledChild = CompileNode(unaryNode.Child, parameter);
        return unaryNode.Operator switch
        {
            OperatorType.Plus => compiledChild,
            OperatorType.Minus => Expression.Negate(compiledChild),
            _ => throw new CompilerException($"Unknown unary operator: {unaryNode.Operator}")
        };
    }
    
    private Expression CompileVariableNode(VariableNode variableNode, ParameterExpression parameter)
    {
        var identifier = variableNode.Identifier;
        return Expression.Call(
            parameter,
            typeof(IEvaluationContext).GetMethod(nameof(IEvaluationContext.GetVariable))!,
            [Expression.Constant(identifier)]);
    }

    private Expression CompileFunctionNode(FunctionNode functionNode, ParameterExpression parameter)
    {
        var parameters = functionNode.Parameters;
        var compiledParameters = parameters.Select(x => CompileNode(x, parameter));
        return Expression.Call(
            typeof(ExpressionTreeCompiler).GetMethod(nameof(CallMethod), BindingFlags.Static | BindingFlags.NonPublic)!,
            [parameter, Expression.Constant(functionNode.Identifier), Expression.NewArrayInit(typeof(double), compiledParameters)]);
    }
    
    private static double CallMethod(IEvaluationContext context, string identifier, double[] parameters)
    {
        return context.CallFunction(identifier, parameters);
    }
}