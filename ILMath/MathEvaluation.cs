using ILMath.Compiler;

namespace ILMath;

/// <summary>
/// Helper class for math evaluation.
/// </summary>
public static class MathEvaluation
{
    /// <summary>
    /// Compiles an expression to a function.
    /// </summary>
    /// <param name="functionName">The function name.</param>
    /// <param name="expression">The math expression.</param>
    /// <param name="method">The compilation method.</param>
    /// <returns>The evaluator.</returns>
    public static Evaluator CompileExpression(string functionName, string expression, CompilationMethod method = CompilationMethod.IntermediateLanguage)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var node = parser.Parse();
        var compiler = CreateCompiler(method);
        return compiler.Compile(functionName, node);
    }

    /// <summary>
    /// Creates a compiler based on the compilation method.
    /// </summary>
    /// <param name="method">The compilation method.</param>
    /// <returns>The created compiler.</returns>
    public static ICompiler CreateCompiler(CompilationMethod method)
    {
        return method switch
        {
            CompilationMethod.IntermediateLanguage => new IlCompiler(),
            CompilationMethod.Functional => new FunctionalCompiler(),
            CompilationMethod.ExpressionTree => new ExpressionTreeCompiler(),
            _ => throw new ArgumentException($"Unknown compilation method: {method}")
        };
    }
}