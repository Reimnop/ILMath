using System.Diagnostics;
using ILMath.Data;
using ILMath.Exception;
using ILMath.SyntaxTree;

namespace ILMath;

/// <summary>
/// Parses the tokens and returns an expression tree.
/// </summary>
public class Parser
{
    // The lexer is injected into the parser through the constructor
    private readonly Lexer lexer;

    public Parser(Lexer lexer)
    {
        this.lexer = lexer;
    }
    
    /// <summary>
    /// Parses the tokens and returns an expression tree.
    /// </summary>
    /// <returns>The expression tree.</returns>
    public INode Parse()
    {
        return Root();
    }

    private INode Root()
    {
        var node = Expression();
        Consume(TokenType.EndOfInput);
        return node;
    }

    private INode Expression()
    {
        var left = Term();
        if (lexer.CurrentToken.Type is TokenType.Plus or TokenType.Minus)
        {
            var @operator = lexer.CurrentToken.Type == TokenType.Plus ? OperatorType.Plus : OperatorType.Minus;
            Consume(lexer.CurrentToken.Type);
            var right = Expression();
            return new OperatorNode(@operator, left, right);
        }

        return left;
    }

    private INode Term()
    {
        var left = Exponent();
        if (lexer.CurrentToken.Type is TokenType.Multiplication or TokenType.Division or TokenType.Modulo)
        {
            var @operator = lexer.CurrentToken.Type switch
            {
                TokenType.Multiplication => OperatorType.Multiplication,
                TokenType.Division => OperatorType.Division,
                TokenType.Modulo => OperatorType.Modulo,
                _ => throw new ParserException("Unexpected token")
            };
            Consume(lexer.CurrentToken.Type);
            var right = Term();
            return new OperatorNode(@operator, left, right);
        }
        return left;
    }
    
    private INode Exponent()
    {
        var @base = Factor();
        
        // Check if we have an exponent
        if (lexer.CurrentToken.Type == TokenType.Power)
        {
            Consume(TokenType.Power);
            var exponent = Exponent();
            return new OperatorNode(OperatorType.Exponent, @base, exponent);
        }
        
        return @base;
    }

    private INode Factor()
    {
        if (lexer.CurrentToken.Type == TokenType.Identifier)
        {
            var identifier = lexer.CurrentToken.Value;
            Debug.Assert(identifier != null);
            Consume(TokenType.Identifier);
            if (lexer.CurrentToken.Type == TokenType.OpenParenthesis)
            {
                Consume(TokenType.OpenParenthesis);
                var node = new FunctionNode(identifier, Parameters());
                Consume(TokenType.CloseParenthesis);
                return node;
            }
            return new VariableNode(identifier);
        }
        
        if (lexer.CurrentToken.Type == TokenType.OpenParenthesis)
        {
            Consume(TokenType.OpenParenthesis);
            var node = Expression();
            Consume(TokenType.CloseParenthesis);
            return node;
        }
        
        if (lexer.CurrentToken.Type is TokenType.Plus or TokenType.Minus)
        {
            var @operator = lexer.CurrentToken.Type == TokenType.Plus ? OperatorType.Plus : OperatorType.Minus;
            Consume(lexer.CurrentToken.Type);
            return new UnaryNode(@operator, Factor());
        }
        
        // Probably a number
        var token = lexer.CurrentToken;
        Consume(TokenType.Number);
        Debug.Assert(token.Value != null);
        var value = double.Parse(token.Value);
        return new NumberNode(value);
    }

    private IEnumerable<INode> Parameters()
    {
        if (lexer.CurrentToken.Type == TokenType.CloseParenthesis)
            yield break;
        
        yield return Expression();
        while (lexer.CurrentToken.Type == TokenType.Comma)
        {
            Consume(TokenType.Comma);
            yield return Expression();
        }
    }

    private void Consume(TokenType type)
    {
        if (!lexer.Consume(type))
            throw new ParserException($"Unexpected token: {lexer.CurrentToken} (expected: {type})");
    }
}