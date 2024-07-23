namespace ILSourceParser.Trivia;

public class OpenParenthesisTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.OpenParenthesis;

    internal OpenParenthesisTrivia()
    {
    }

    public override string ToString()
    {
        return "(";
    }
}
