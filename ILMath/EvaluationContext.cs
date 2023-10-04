using ILMath.Exception;

namespace ILMath;

public delegate double MathFunction(Span<double> parameters);

/// <summary>
/// Default implementation of <see cref="IEvaluationContext"/>.
/// </summary>
public class EvaluationContext : IEvaluationContext
{
    private readonly Dictionary<string, double> variables = new();
    private readonly Dictionary<string, MathFunction> functions = new();

    /// <summary>
    /// Registers default variables and functions to this <see cref="EvaluationContext"/>.
    /// </summary>
    public void RegisterBuiltIns()
    {
        // Register variables
        RegisterVariable("pi", Math.PI);
        RegisterVariable("e", Math.E);
        RegisterVariable("tau", Math.PI * 2.0);
        RegisterVariable("phi", (1.0 + Math.Sqrt(5.0)) / 2.0);
        RegisterVariable("inf", double.PositiveInfinity);
        RegisterVariable("nan", double.NaN);
        RegisterVariable("degToRad", Math.PI / 180.0);
        RegisterVariable("radToDeg", 180.0 / Math.PI);

        // Register functions
        RegisterFunction("sin", parameters => Math.Sin(parameters[0]));
        RegisterFunction("cos", parameters => Math.Cos(parameters[0]));
        RegisterFunction("tan", parameters => Math.Tan(parameters[0]));
        RegisterFunction("asin", parameters => Math.Asin(parameters[0]));
        RegisterFunction("acos", parameters => Math.Acos(parameters[0]));
        RegisterFunction("atan", parameters => Math.Atan(parameters[0]));
        RegisterFunction("atan2", parameters => Math.Atan2(parameters[0], parameters[1]));
        RegisterFunction("sinh", parameters => Math.Sinh(parameters[0]));
        RegisterFunction("cosh", parameters => Math.Cosh(parameters[0]));
        RegisterFunction("tanh", parameters => Math.Tanh(parameters[0]));
        RegisterFunction("sqrt", parameters => Math.Sqrt(parameters[0]));
        RegisterFunction("cbrt", parameters => Math.Cbrt(parameters[0]));
        RegisterFunction("root", parameters => Math.Pow(parameters[0], 1.0 / parameters[1]));
        RegisterFunction("exp", parameters => Math.Exp(parameters[0]));
        RegisterFunction("abs", parameters => Math.Abs(parameters[0]));
        RegisterFunction("log", parameters => Math.Log(parameters[0]));
        RegisterFunction("log10", parameters => Math.Log10(parameters[0]));
        RegisterFunction("log2", parameters => Math.Log2(parameters[0]));
        RegisterFunction("logn", parameters => Math.Log(parameters[0], parameters[1]));
        RegisterFunction("pow", parameters => Math.Pow(parameters[0], parameters[1]));
        RegisterFunction("mod", parameters => parameters[0] % parameters[1]);
        RegisterFunction("min", parameters => Math.Min(parameters[0], parameters[1]));
        RegisterFunction("max", parameters => Math.Max(parameters[0], parameters[1]));
        RegisterFunction("floor", parameters => Math.Floor(parameters[0]));
        RegisterFunction("ceil", parameters => Math.Ceiling(parameters[0]));
        RegisterFunction("round", parameters => Math.Round(parameters[0]));
        RegisterFunction("sign", parameters => Math.Sign(parameters[0]));
        RegisterFunction("clamp", parameters => Math.Clamp(parameters[0], parameters[1], parameters[2]));
        RegisterFunction("lerp", parameters => (parameters[2] - parameters[1]) * parameters[0] + parameters[1]);
        RegisterFunction("inverseLerp", parameters => (parameters[0] - parameters[1]) / (parameters[2] - parameters[1]));
    }

    /// <summary>
    /// Registers a variable to this <see cref="EvaluationContext"/>.
    /// </summary>
    /// <param name="identifier">The variable's identifier.</param>
    /// <param name="value">The variable's value.</param>
    public void RegisterVariable(string identifier, double value)
    {
        variables.Add(identifier, value);
    }

    /// <summary>
    /// Registers a function to this <see cref="EvaluationContext"/>.
    /// </summary>
    /// <param name="identifier">The function's identifier.</param>
    /// <param name="function">The function.</param>
    public void RegisterFunction(string identifier, MathFunction function)
    {
        functions.Add(identifier, function);
    }

    public double GetVariable(string identifier)
    {
        if (variables.TryGetValue(identifier, out var value))
            return value;
        throw new EvaluationException($"Unknown variable: {identifier}");
    }

    public double CallFunction(string identifier, Span<double> parameters)
    {
        if (functions.TryGetValue(identifier, out var function))
            return function(parameters);
        throw new EvaluationException($"Unknown function: {identifier}");
    }
    
    /// <summary>
    /// Creates a default implementation of <see cref="IEvaluationContext"/>.
    /// </summary>
    /// <returns>The default <see cref="IEvaluationContext"/></returns>
    public static EvaluationContext CreateDefault()
    {
        var context = new EvaluationContext();
        context.RegisterBuiltIns();
        return context;
    }
}