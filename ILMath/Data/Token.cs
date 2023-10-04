namespace ILMath.Data;

/// <summary>
/// Represents a token.
/// </summary>
public struct Token
{
    /// <summary>
    /// The type of this token.
    /// </summary>
    public TokenType Type { get; } = TokenType.None;

    /// <summary>
    /// The value of the current token. Might be null.
    /// </summary>
    public string? Value { get; } = null;

    public Token(TokenType type, string? value)
    {
        Type = type;
        Value = value;
    }

    public Token(TokenType type)
    {
        Type = type;
    }
    
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Value) ? $"Token({Type})" : $"Token({Type}, {Value})";
    }
}