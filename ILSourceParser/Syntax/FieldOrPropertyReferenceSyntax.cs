using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class FieldOrPropertyReferenceSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    public TypeReferenceSyntax FieldType { get; init; }
    public string FieldOrPropertyName { get; init; }
    public TypeReferenceSyntax DeclaringTypeReference { get; init; }

    internal FieldOrPropertyReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeReferenceSyntax fieldType,
        string objectName,
        TypeReferenceSyntax declaringTypeReference)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        FieldType = fieldType;
        FieldOrPropertyName = objectName;
        DeclaringTypeReference = declaringTypeReference;
    }
}
