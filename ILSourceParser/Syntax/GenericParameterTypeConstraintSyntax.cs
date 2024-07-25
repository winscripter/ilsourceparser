using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericParameterTypeConstraintSyntax : BaseGenericParameterSyntax
{
    public TypeReferenceSyntax TypeReference { get; init; }
    public string ParameterName { get; init; }

    internal GenericParameterTypeConstraintSyntax(
        TypeReferenceSyntax typeReference,
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string parameterName) : base(leadingTrivia, trailingTrivia)
    {
        TypeReference = typeReference;
        ParameterName = parameterName;
    }
}
