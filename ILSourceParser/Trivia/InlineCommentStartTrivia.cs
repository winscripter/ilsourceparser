namespace ILSourceParser.Trivia;

public class InlineCommentStartTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.InlineCommentStart;

    internal InlineCommentStartTrivia()
    {
    }

    public override string ToString()
    {
        return "//";
    }
}
