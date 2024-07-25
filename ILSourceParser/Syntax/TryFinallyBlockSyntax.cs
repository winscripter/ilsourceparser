using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>.try</c> followed by <c>finally</c> block.
/// </summary>
public sealed class TryFinallyBlockSyntax : SyntaxNode
{
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// The information about the <c>.try</c> block.
    /// </summary>
    public TryBlockSyntax TryBlock { get; init; }

    /// <summary>
    /// The information about the <c>finally</c> block.
    /// </summary>
    public FinallyBlockSyntax FinallyBlock { get; init; }

    internal TryFinallyBlockSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TryBlockSyntax tryBlock,
        FinallyBlockSyntax finallyBlock)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        TryBlock = tryBlock;
        FinallyBlock = finallyBlock;
    }
}
