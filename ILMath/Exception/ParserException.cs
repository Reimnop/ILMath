namespace ILMath.Exception;

/// <summary>
/// Thrown by the <see cref="Parser"/> when an error occurs.
/// </summary>
public class ParserException : System.Exception
{
    public ParserException(string message) : base(message)
    {
    }
}