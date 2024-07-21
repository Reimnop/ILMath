namespace ILMath.Exception;

/// <summary>
/// Thrown by the <see cref="FunctionCompiler"/> when an error occurs.
/// </summary>
public class CompilerException : System.Exception
{
    public CompilerException(string message) : base(message)
    {
    }
}