using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class ParamDirectiveSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public string ParameterIndex { get; init; }
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
