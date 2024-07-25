using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents information about a parameter modifier in IL code. For example,
/// <c>[in]</c>, <c>[out]</c>, or <c>[opt]</c>.
/// </summary>
public class ParameterModifierSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Represents the type of parameter modifier as <see cref="ParameterModifierType"/>.
    /// </summary>
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
