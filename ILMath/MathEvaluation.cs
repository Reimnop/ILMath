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
    /// <returns>The evaluator.</returns>
    public static Evaluator CompileExpression(string functionName, string expression)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var node = parser.Parse();
        var compiler = new Compiler(node);
        return compiler.Compile(functionName);
    }
}