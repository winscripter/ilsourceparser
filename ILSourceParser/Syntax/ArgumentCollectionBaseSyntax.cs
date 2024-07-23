using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following nodes:
/// <list type="bullet">
///   <item><see cref="GenericArgumentsDefinitionSyntax"/></item>
///   <item><see cref="GenericArgumentsReferenceSyntax"/></item>
/// </list>
/// </summary>
public class ArgumentCollectionBaseSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// A list of parameters passed to the argument collection.
    /// </summary>
    public IEnumerable<BaseGenericParameterSyntax> Parameters { get; init; }

    internal ArgumentCollectionBaseSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<BaseGenericParameterSyntax> parameters)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        Parameters = parameters;
    }
}
