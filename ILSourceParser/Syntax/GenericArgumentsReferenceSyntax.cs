using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericArgumentsReferenceSyntax : ArgumentCollectionBaseSyntax
{
    internal GenericArgumentsReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<BaseGenericParameterSyntax> parameters) : base(leadingTrivia, trailingTrivia, parameters)
    {
    }
}
