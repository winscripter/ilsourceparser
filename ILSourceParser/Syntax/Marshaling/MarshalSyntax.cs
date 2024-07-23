using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Marshaling;

/// <summary>
/// Represents IL <c>marshal()</c> function.
/// </summary>
public abstract class MarshalSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public MarshalTypeSyntax MarshalType { get; init; }

    protected internal MarshalSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MarshalTypeSyntax marshalType)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        MarshalType = marshalType;
    }
}
