namespace ILMath;

/// <summary>
/// Provides context for evaluating a compiled expression.
/// </summary>
public interface IEvaluationContext
{
    /// <summary>
    /// Gets the value of a variable.
    /// </summary>
    /// <param name="identifier">The variable's identifier.</param>
    /// <returns>The variable's value.</returns>
    double GetVariable(string identifier);
    
    /// <summary>
    /// Calls a function.
    /// </summary>
    /// <param name="identifier">The function's identifier.</param>
    /// <param name="parameters">The function's parameters.</param>
    /// <returns>The returned result of the function.</returns>
    double CallFunction(string identifier, Span<double> parameters);
}