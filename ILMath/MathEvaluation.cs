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
    /// <param name="dynamic">Whether to use dynamic compilation.</param>
    /// <returns>The evaluator.</returns>
    public static Evaluator CompileExpression(string functionName, string expression, bool dynamic = true)
    {
        var lexer = new Lexer(expression);
        var parser = new Parser(lexer);
        var node = parser.Parse();
        if (dynamic)
        {
            var compiler = new IlCompiler(node);
            return compiler.Compile(functionName);
        }
        else
        {
            var compiler = new FunctionCompiler(node);
            return compiler.Compile(functionName);
        }
    }
}