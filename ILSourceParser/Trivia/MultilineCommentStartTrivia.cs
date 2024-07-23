namespace ILSourceParser.Trivia;

public class MultilineCommentStartTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.MultilineCommentStart;

    internal MultilineCommentStartTrivia()
    {
    }

    public override string ToString()
    {
        return "/*";
    }
}
