using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public abstract class TypeBaseSyntax : TypeSyntax
{
    protected TypeBaseSyntax(ImmutableArray<SyntaxTrivia> leadingTrivia, ImmutableArray<SyntaxTrivia> trailingTrivia) : base(leadingTrivia, trailingTrivia)
    {
    }
}
