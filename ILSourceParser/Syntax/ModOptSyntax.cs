using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class ModOptSyntax : ModifierNotationSyntax
{
    public TypeReferenceSyntax TypeReference { get; init; }

    internal ModOptSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeReferenceSyntax typeReference) : base(leadingTrivia, trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        TypeReference = typeReference;
    }
}
