namespace ILSourceParser.Trivia;

public class MultilineCommentEndTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.MultilineCommentEnd;

    internal MultilineCommentEndTrivia()
    {
    }

    public override string ToString()
    {
        return "*/";
    }
}
