using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericArgumentsDefinitionSyntax : ArgumentCollectionBaseSyntax
{
    internal GenericArgumentsDefinitionSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<BaseGenericParameterSyntax> parameters) : base(leadingTrivia, trailingTrivia, parameters)
    {
    }
}
