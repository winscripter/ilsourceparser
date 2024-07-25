using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>.stackreserve</c> directive.
/// </summary>
public class StackReserveDirectiveSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// The number that specifies the stack reserve.
    /// </summary>
    public string StackReserve { get; init; }

    internal StackReserveDirectiveSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string stackReserve)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        StackReserve = stackReserve;
    }
}
