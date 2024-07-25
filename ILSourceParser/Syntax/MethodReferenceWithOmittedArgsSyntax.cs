using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class MethodReferenceWithOmittedArgsSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    public TypeReferenceSyntax TypeReference { get; init; }
    public string MethodName { get; init; }
    public TypeReferenceSyntax MethodDeclaringTypeReference { get; init; }

    internal MethodReferenceWithOmittedArgsSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeReferenceSyntax typeReference,
        string methodName,
        TypeReferenceSyntax methodDeclaringTypeReference)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        TypeReference = typeReference;
        MethodName = methodName;
        MethodDeclaringTypeReference = methodDeclaringTypeReference;
    }
}
