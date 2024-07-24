using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public sealed class TryFinallyBlockSyntax : SyntaxNode
{
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public TryBlockSyntax TryBlock { get; init; }
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
