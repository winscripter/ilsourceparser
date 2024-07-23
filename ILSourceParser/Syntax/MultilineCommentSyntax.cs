using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class MultilineCommentSyntax : BaseCommentSyntax
{
    public string CommentText { get; init; }

    internal MultilineCommentSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string commentText) : base(leadingTrivia, trailingTrivia)
    {
        CommentText = commentText;
    }
}
