using System.Text;
using ILMath.Data;

namespace ILMath;

/// <summary>
/// Analyzes the input string and returns a list of tokens.
/// </summary>
public class Lexer
{
    /// <summary>
    /// The current token. Consume to get the next token.
    /// </summary>
    public Token CurrentToken { get; private set; }
    
    private readonly string input;
    private int index;
    
    public Lexer(string input)
    {
        this.input = input;
        Consume(TokenType.None);
    }

    /// <summary>
    /// Consumes the current token and sets <see cref="CurrentToken"/> to the next token.
    /// </summary>
    /// <param name="type">The type </param>
    public bool Consume(TokenType type)
    {
        if (CurrentToken.Type != type) 
            return false;
        
        CurrentToken = NextToken();
        return true;
    }

    private Token NextToken()
    {
        // Skip whitespace, if any
        while (index < input.Length && char.IsWhiteSpace(input[index]))
            index++;
        
        // Check if we are at the end of the input
        if (index >= input.Length)
            return new Token(TokenType.EndOfInput);

        var currentChar = input[index];
        var token = currentChar switch
        {
            '+' => new Token(TokenType.Plus),
            '-' => new Token(TokenType.Minus),
            '*' => new Token(TokenType.Multiplication),
            '/' => new Token(TokenType.Division),
            '%' => new Token(TokenType.Modulo),
            '^' => new Token(TokenType.Power),
            '(' => new Token(TokenType.OpenParenthesis),
            ')' => new Token(TokenType.CloseParenthesis),
            ',' => new Token(TokenType.Comma),
            _ => new Token(TokenType.None)
        };
        
        // If we found a symbol, return it
        if (token.Type != TokenType.None)
        {
            index++;
            return token;
        }
        
        // Else, we found a number or identifier
        return NextNonSymbolToken();
    }

    private Token NextNonSymbolToken()
    {
        var currentChar = input[index];
        
        // If it is a number, read the whole number
        if (char.IsDigit(currentChar))
            return NextNumber();
        
        // If it is a letter, read the whole identifier
        if (IsIdentifierChar(currentChar))
            return NextIdentifier();

        // Else, we found an unknown token
        index++;
        return new Token(TokenType.Unknown, currentChar.ToString());
    }

    private Token NextIdentifier()
    {
        var builder = new StringBuilder();
        
        // Read the whole identifier
        while (index < input.Length && IsIdentifierChar(input[index]))
        {
            builder.Append(input[index]);
            index++;
        }
        
        return new Token(TokenType.Identifier, builder.ToString());
    }

    private bool IsIdentifierChar(char c)
    {
        return c == '_' 
               || c == '@'
               || c == '$'
               || c == '#'
               || (c >= 'a' && c <= 'z') 
               || (c >= 'A' && c <= 'Z') 
               || char.IsDigit(c);
    }

    private Token NextNumber()
    {
        var builder = new StringBuilder();
        
        // Read the whole number
        while (index < input.Length && char.IsDigit(input[index]))
        {
            builder.Append(input[index]);
            index++;
        }

        // If we found a dot, read the decimal part
        if (index < input.Length && input[index] == '.')
        {
            builder.Append(input[index]);
            index++;
            
            while (index < input.Length && char.IsDigit(input[index]))
            {
                builder.Append(input[index]);
                index++;
            }
        }
        
        return new Token(TokenType.Number, builder.ToString());
    }
}