using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericParameterPrimitiveSyntax : BaseGenericParameterSyntax
{
    public PredefinedTypeSyntax UnderlyingType { get; init; }

    internal GenericParameterPrimitiveSyntax(
        PredefinedTypeSyntax underlyingType,
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia) : base(leadingTrivia, trailingTrivia)
    {
        UnderlyingType = underlyingType;
    }
}
