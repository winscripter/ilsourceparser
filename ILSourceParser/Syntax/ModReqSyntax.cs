using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class ModReqSyntax : ModifierNotationSyntax
{
    public TypeReferenceSyntax TypeReference { get; init; }

    internal ModReqSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeReferenceSyntax typeReference) : base(leadingTrivia, trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        TypeReference = typeReference;
    }
}
