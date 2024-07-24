using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>finally</c> block.
/// </summary>
public sealed class FinallyBlockSyntax : SyntaxNode
{
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Descendant nodes of the block.
    /// </summary>
    public IEnumerable<SyntaxNode> DescendantNodes { get; init; }

    internal FinallyBlockSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<SyntaxNode> descendantNodes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        DescendantNodes = descendantNodes;
    }
}
