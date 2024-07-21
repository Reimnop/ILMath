using System.Diagnostics;
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
    private record struct CompilationState(LocalBuilder? Parameters, int StackLocation);

    private readonly INode root;
    private readonly int maximumParameterStackSize;
    
    public Compiler(INode root)
    {
        this.root = root;
        
        // Calculate the maximum parameter stack size
        var maximumStackSize = 0;
        CalculateMaximumParameterStackSize(root, -1, ref maximumStackSize);
        maximumParameterStackSize = maximumStackSize;
    }

    private static void CalculateMaximumParameterStackSize(INode node, int stackLocation, ref int maximumStackSize)
    {
        if (node is FunctionNode functionNode)
        {
            // For each child, increment the stack size
            foreach (var child in functionNode.Parameters)
            {
                stackLocation++;
                CalculateMaximumParameterStackSize(child, stackLocation, ref maximumStackSize);
                maximumStackSize = Math.Max(maximumStackSize, stackLocation + 1);
            }
        }
        else
            foreach (var child in node.EnumerateChildren())
                CalculateMaximumParameterStackSize(child, stackLocation, ref maximumStackSize);
    }

    /// <summary>
    /// Compiles the syntax tree into a function.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <returns>The evaluator.</returns>
    public Evaluator Compile(string name)
    {
        return CompileSyntaxTree(name, root);
    }

    private Evaluator CompileSyntaxTree(string name, INode rootNode)
    {
        // Create a new dynamic method
        var method = new DynamicMethod(name, typeof(double), new [] { typeof(IEvaluationContext) });
        var il = method.GetILGenerator();
        
        // If maximum parameter stack size is greater than zero, stackalloc the parameters array
        LocalBuilder? parameters = null;
        if (maximumParameterStackSize > 0)
        {
            // Load the parameters count onto the stack
            il.Emit(OpCodes.Ldc_I4, maximumParameterStackSize * sizeof(double));
            
            // Create the parameters array on the stack
            il.Emit(OpCodes.Conv_U);
            il.Emit(OpCodes.Localloc);
            
            // Store the parameters array in a local variable
            parameters = il.DeclareLocal(typeof(double*));
            il.Emit(OpCodes.Stloc, parameters);
        }

        // Compile the syntax tree into IL code
        CompileNode(rootNode, il, new CompilationState(parameters, 0));
        
        // Return the value on the stack
        il.Emit(OpCodes.Ret);

        // Return a delegate from the dynamic method
        return (Evaluator) method.CreateDelegate(typeof(Evaluator));
    }

    private void CompileNode(INode node, ILGenerator il, CompilationState state)
    {
        switch (node)
        {
            case OperatorNode expressionNode:
                CompileOperatorNode(expressionNode, il, state);
                break;
            case NumberNode numberNode:
                CompileNumberNode(numberNode, il);
                break;
            case UnaryNode unaryNode:
                CompileUnaryNode(unaryNode, il, state);
                break;
            case VariableNode variableNode:
                CompileVariableNode(variableNode, il);
                break;
            case FunctionNode functionNode:
                CompileFunctionNode(functionNode, il, state);
                break;
            default:
                throw new CompilerException($"Unknown node type: {node.GetType()}");
        }
    }

    private void CompileOperatorNode(OperatorNode operatorNode, ILGenerator il, CompilationState state)
    {
        var left = operatorNode.Left;
        var right = operatorNode.Right;
        var @operator = operatorNode.Operator;
        CompileNode(left, il, state);
        CompileNode(right, il, state);
        GenerateOperatorInstruction(@operator, il);
    }
    
    private static void CompileNumberNode(NumberNode numberNode, ILGenerator il)
    {
        il.Emit(OpCodes.Ldc_R8, numberNode.Value);
    }
    
    private void CompileUnaryNode(UnaryNode unaryNode, ILGenerator il, CompilationState state)
    {
        CompileNode(unaryNode.Child, il, state);
        if (unaryNode.Operator == OperatorType.Minus)
            il.Emit(OpCodes.Neg);
    }
    
    private void CompileVariableNode(VariableNode variableNode, ILGenerator il)
    {
        // Load the context onto the stack
        il.Emit(OpCodes.Ldarg_0);
        
        // Load the variable identifier onto the stack
        il.Emit(OpCodes.Ldstr, variableNode.Identifier);

        // Call the GetVariable method
        il.Emit(OpCodes.Callvirt, typeof(IEvaluationContext).GetMethod(nameof(IEvaluationContext.GetVariable))!);
    }
    
    private void CompileFunctionNode(FunctionNode functionNode, ILGenerator il, CompilationState state)
    {
        // Load the context onto the stack
        il.Emit(OpCodes.Ldarg_0);
        
        // Load the function identifier onto the stack
        il.Emit(OpCodes.Ldstr, functionNode.Identifier);
        
        var parametersCount = functionNode.Parameters.Count;
        if (parametersCount > 0)
        {
            // Make sure the parameters array is not null
            Debug.Assert(state.Parameters != null);
            
            // Populate the parameters array
            for (var i = 0; i < parametersCount; i++)
            {
                // Load the parameters array pointer onto the stack
                il.Emit(OpCodes.Ldloc, state.Parameters);

                var offset = state.StackLocation + i;
                if (offset > 0)
                {
                    // Load the current byte offset onto the stack
                    il.Emit(OpCodes.Ldc_I4, offset * sizeof(double));
                
                    // Add the byte offset to the array pointer
                    il.Emit(OpCodes.Add);
                }

                // Compile the parameter node
                CompileNode(functionNode.Parameters[i], il, state with {StackLocation = offset});

                // Store the parameter value in the array
                il.Emit(OpCodes.Stind_R8);
            }

            // Load the parameters array pointer onto the stack
            il.Emit(OpCodes.Ldloc, state.Parameters);
            
            var parametersOffset = state.StackLocation;
            if (parametersOffset > 0)
            {
                // Load the current byte offset onto the stack
                il.Emit(OpCodes.Ldc_I4, parametersOffset * sizeof(double));
                
                // Add the byte offset to the array pointer
                il.Emit(OpCodes.Add);
            }
            
            // Load the parameters count onto the stack
            il.Emit(OpCodes.Ldc_I4, parametersCount);
            
            // Create a span from the parameters array
            il.Emit(OpCodes.Newobj, typeof(Span<double>).GetConstructor(new [] { typeof(void*), typeof(int) })!);
        }
        else
        {
            // Create an empty span
            // We use Call instead of Ldsfld because the Span<double>.Empty is a property
            il.Emit(OpCodes.Call, typeof(Span<double>).GetProperty(nameof(Span<double>.Empty))!.GetMethod!);
        }

        // Call the CallFunction method
        il.Emit(OpCodes.Callvirt, typeof(IEvaluationContext).GetMethod(nameof(IEvaluationContext.CallFunction))!);
    }
    
    private static void GenerateOperatorInstruction(OperatorType operatorType, ILGenerator il)
    {
        var opCode = operatorType switch
        {
            OperatorType.Plus => OpCodes.Add,
            OperatorType.Minus => OpCodes.Sub,
            OperatorType.Multiplication => OpCodes.Mul,
            OperatorType.Division => OpCodes.Div,
            OperatorType.Modulo => OpCodes.Rem,
            OperatorType.Exponent => OpCodes.Call,
            _ => throw new CompilerException($"Unknown operator type '{operatorType}'")
        };
        
        if (operatorType != OperatorType.Exponent)
            il.Emit(opCode);
        else 
            il.Emit(opCode, typeof(Math).GetMethod(nameof(Math.Pow))!);
    }
}