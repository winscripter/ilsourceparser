using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Base class for generic parameters. This node is associated with the following nodes:
/// <list type="bullet">
///   <item>
///     <see cref="GenericParameterPrimitiveSyntax"/>
///   </item>
///   <item>
///     <see cref="GenericParameterReferenceSyntax"/>
///   </item>
///   <item>
///     <see cref="GenericParameterTypeConstraintSyntax"/>
///   </item>
/// </list>
/// </summary>
public class BaseGenericParameterSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    internal BaseGenericParameterSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
