using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following types:
/// <list type="bullet">
///   <item>
///     <see cref="InlineCommentSyntax"/>
///   </item>
///   <item>
///     <see cref="MultilineCommentSyntax"/>
///   </item>
/// </list>
/// </summary>
public abstract class BaseCommentSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    protected internal BaseCommentSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
