using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class ParameterModifierSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public ParameterModifierType ModifierType { get; init; }

    internal ParameterModifierSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        ParameterModifierType modifierType)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        ModifierType = modifierType;
    }
}
