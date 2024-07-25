using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>.param</c> directive. This directive is used to apply custom
/// attributes to method parameters.
/// </summary>
public class ParamDirectiveSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// The one-based index of the parameter where attributes should be applied. If the
    /// value of this property is <c>0</c>, custom attributes will be applied on the method
    /// return type instead.
    /// </summary>
    public string ParameterIndex { get; init; }

    /// <summary>
    /// Descendant custom attributes of this .param directive.
    /// </summary>
    public IEnumerable<BaseCustomAttributeSyntax> CustomAttributes { get; init; }

    internal ParamDirectiveSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string parameterIndex,
        IEnumerable<BaseCustomAttributeSyntax> customAttributes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        ParameterIndex = parameterIndex;
        CustomAttributes = customAttributes;
    }
}
