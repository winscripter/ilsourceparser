using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class InlineCommentSyntax : BaseCommentSyntax
{
    public string CommentText { get; init; }

    internal InlineCommentSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string commentText) : base(leadingTrivia, trailingTrivia)
    {
        CommentText = commentText;
    }
}
