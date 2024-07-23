namespace ILSourceParser.Trivia;

public class CloseParenthesisTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.CloseParenthesis;

    internal CloseParenthesisTrivia()
    {
    }

    public override string ToString()
    {
        return ")";
    }
}
