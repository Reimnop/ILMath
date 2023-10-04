namespace ILMath;

/// <summary>
/// Helper class for math evaluation.
/// </summary>
public static class MathEvaluation
{
    /// <summary>
    /// Compiles an expression to a function.
    /// </summary>
    /// <param name="expression">The math expression.</param>
    /// <returns>The evaluator.</returns>
    public static Evaluator CompileExpression(string expression)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var node = parser.Parse();
        var compiler = new Compiler("Test");
        return compiler.Compile(node);
    }
}