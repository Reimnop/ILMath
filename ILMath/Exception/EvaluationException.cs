namespace ILMath.Exception;

/// <summary>
/// Thrown when errors occur during evaluation.
/// </summary>
public class EvaluationException : System.Exception
{
    public EvaluationException(string message) : base(message)
    {
    }
}