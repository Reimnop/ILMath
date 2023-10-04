using System.Reflection.Emit;
using ILMath.Exception;
using ILMath.SyntaxTree;

namespace ILMath;

/// <summary>
/// Represents a function that is compiled to evaluate an expression.
/// </summary>
public delegate double Evaluator(IEvaluationContext context);

/// <summary>
/// Compiles a syntax tree into Microsoft Intermediate Language (MSIL) code.
/// </summary>
public class Compiler
{
    private readonly string name;
    
    public Compiler(string name)
    {
        this.name = name;
    }
    
    /// <summary>
    /// Compiles the syntax tree into a function.
    /// </summary>
    /// <returns>The evaluator.</returns>
    public Evaluator Compile(INode node)
    {
        return CompileNode(name, node);
    }

    private static Evaluator CompileNode(string name, INode node)
    {
        // Create a new dynamic method with a double return type and no parameters
        var method = new DynamicMethod(name, typeof(double), new [] { typeof(IEvaluationContext) });
        var il = method.GetILGenerator();

        // Compile the syntax tree into IL code
        CompileNode(node, il);
        
        // Return the value on the stack
        il.Emit(OpCodes.Ret);

        // Return a delegate from the dynamic method
        return (Evaluator) method.CreateDelegate(typeof(Evaluator));
    }

    private static void CompileNode(INode node, ILGenerator il)
    {
        switch (node)
        {
            case ExpressionNode expressionNode:
                CompileExpressionNode(expressionNode, il);
                break;
            case TermNode termNode:
                CompileTermNode(termNode, il);
                break;
            case ExponentNode exponentNode:
                CompileExponentNode(exponentNode, il);
                break;
            case NumberNode numberNode:
                CompileNumberNode(numberNode, il);
                break;
            case UnaryNode unaryNode:
                CompileUnaryNode(unaryNode, il);
                break;
            case OperatorNode operatorNode:
                CompileOperatorNode(operatorNode, il);
                break;
            case VariableNode variableNode:
                CompileVariableNode(variableNode, il);
                break;
            case FunctionNode functionNode:
                CompileFunctionNode(functionNode, il);
                break;
            default:
                throw new CompilerException($"Unknown node type: {node.GetType()}");
        }
    }

    private static void CompileExpressionNode(ExpressionNode expressionNode, ILGenerator il)
    {
        var children = expressionNode.Children;
        CompileNode(children[0], il);
        
        // Get the next nodes
        for (var i = 1; i < children.Count; i += 2)
        {
            var @operator = (OperatorNode) children[i];
            var child = children[i + 1];
            
            CompileNode(child, il);
            CompileNode(@operator, il);
        }
    }
    
    private static void CompileTermNode(TermNode termNode, ILGenerator il)
    {
        var children = termNode.Children;
        CompileNode(children[0], il);
        
        // Get the next nodes
        for (var i = 1; i < children.Count; i += 2)
        {
            var @operator = (OperatorNode) children[i];
            var child = children[i + 1];
            
            CompileNode(child, il);
            CompileNode(@operator, il);
        }
    }
    
    private static void CompileExponentNode(ExponentNode exponentNode, ILGenerator il)
    {
        // Compile the base and exponent
        CompileNode(exponentNode.Base, il);
        CompileNode(exponentNode.Exponent, il);
        
        // Call the Math.Pow method
        il.Emit(OpCodes.Call, typeof(Math).GetMethod(nameof(Math.Pow))!);
    }
    
    private static void CompileNumberNode(NumberNode numberNode, ILGenerator il)
    {
        il.Emit(OpCodes.Ldc_R8, numberNode.Value);
    }
    
    private static void CompileUnaryNode(UnaryNode unaryNode, ILGenerator il)
    {
        CompileNode(unaryNode.Child, il);
        if (unaryNode.Operator == OperatorType.Minus)
            il.Emit(OpCodes.Neg);
    }
    
    private static void CompileOperatorNode(OperatorNode operatorNode, ILGenerator il)
    {
        var opCode = operatorNode.Operator switch
        {
            OperatorType.Plus => OpCodes.Add,
            OperatorType.Minus => OpCodes.Sub,
            OperatorType.Multiplication => OpCodes.Mul,
            OperatorType.Division => OpCodes.Div,
            OperatorType.Modulo => OpCodes.Rem,
            
            // This should never happen, unless the user builds the syntax tree manually
            _ => throw new CompilerException($"Unknown operator type: {operatorNode.Operator}")
        };
        
        il.Emit(opCode);
    }
    
    private static void CompileVariableNode(VariableNode variableNode, ILGenerator il)
    {
        // Load the context onto the stack
        il.Emit(OpCodes.Ldarg_0);
        
        // Load the variable identifier onto the stack
        il.Emit(OpCodes.Ldstr, variableNode.Identifier);

        // Call the GetVariable method
        il.Emit(OpCodes.Callvirt, typeof(IEvaluationContext).GetMethod(nameof(IEvaluationContext.GetVariable))!);
    }
    
    private static void CompileFunctionNode(FunctionNode functionNode, ILGenerator il)
    {
        // TODO: Make it use stackalloc instead
        
        // Load the context onto the stack
        il.Emit(OpCodes.Ldarg_0);
        
        // Load the function identifier onto the stack
        il.Emit(OpCodes.Ldstr, functionNode.Identifier);
        
        var parametersCount = functionNode.Parameters.Count;
        if (parametersCount > 0)
        {
            // Create the parameters array
            var parameters = il.DeclareLocal(typeof(double[]));
            il.Emit(OpCodes.Ldc_I4, parametersCount);
            il.Emit(OpCodes.Newarr, typeof(double));
            il.Emit(OpCodes.Stloc, parameters);
        
            // Populate the parameters array
            for (var i = 0; i < parametersCount; i++)
            {
                // Load the parameters array onto the stack
                il.Emit(OpCodes.Ldloc, parameters);

                // Load the current index onto the stack
                il.Emit(OpCodes.Ldc_I4, i);

                // Compile the parameter node
                CompileNode(functionNode.Parameters[i], il);

                // Store the parameter value in the array
                il.Emit(OpCodes.Stelem_R8);
            }

            // Load the parameters array onto the stack
            il.Emit(OpCodes.Ldloc, parameters);
        }
        else
        {
            // Load null onto the stack to create an empty span
            il.Emit(OpCodes.Ldnull);
        }
        
        // Create the span
        il.Emit(OpCodes.Newobj, typeof(Span<double>).GetConstructor(new [] { typeof(double[]) })!);
        
        // Call the CallFunction method
        il.Emit(OpCodes.Callvirt, typeof(IEvaluationContext).GetMethod(nameof(IEvaluationContext.CallFunction))!);
    }
}