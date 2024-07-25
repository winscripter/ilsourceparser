using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents either the <c>string</c> keyword or the string literal.
/// </summary>
public class StringTypeSyntax : TypeSyntax
{
    internal StringTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string? value) : base(leadingTrivia, trailingTrivia)
    {
        Value = value;
    }

    /// <summary>
    /// Represents the value of the string literal, if specified.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Is the string type just the <c>string</c> keyword and not the string literal?
    /// </summary>
    public bool IsStringKeyword
    {
        get => Value is null;
    }
}
