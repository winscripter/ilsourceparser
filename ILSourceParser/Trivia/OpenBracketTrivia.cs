namespace ILSourceParser.Trivia;

public class OpenBracketTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.OpenBracket;

    internal OpenBracketTrivia()
    {
    }

    public override string ToString()
    {
        return "[";
    }
}
