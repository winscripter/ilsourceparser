using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public sealed class MethodInvocationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    public TypeReferenceSyntax TypeReference { get; init; }
    public string MethodName { get; init; }
    public ArgumentListReferenceSyntax Arguments { get; init; }

    internal MethodInvocationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeReferenceSyntax typeReference,
        string methodName,
        ArgumentListReferenceSyntax arguments)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        TypeReference = typeReference;
        MethodName = methodName;
        Arguments = arguments;
    }
}
