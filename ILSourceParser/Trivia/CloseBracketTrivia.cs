namespace ILSourceParser.Trivia;

public class CloseBracketTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.CloseBracket;

    internal CloseBracketTrivia()
    {
    }

    public override string ToString()
    {
        return "]";
    }
}
