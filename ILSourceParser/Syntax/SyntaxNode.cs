using System.Collections.Immutable;
using ILSourceParser.Trivia;

namespace ILSourceParser.Syntax;

public abstract class SyntaxNode
{
    public abstract ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    public abstract ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
}
