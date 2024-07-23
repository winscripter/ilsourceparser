using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL boolean literal - e.g. <c>true</c> or <c>false</c> keyword.
/// </summary>
public class BooleanLiteralSyntax : LiteralSyntax
{
    internal const string TrueEqualityString = "true";

    /// <summary>
    /// Represents the value of the literal. This string can only hold a value
    /// of <c>true</c> or <c>false</c>.
    /// </summary>
    public string Value { get; init; }

    internal BooleanLiteralSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string value) : base(leadingTrivia, trailingTrivia)
    {
        Value = value;
    }
}
