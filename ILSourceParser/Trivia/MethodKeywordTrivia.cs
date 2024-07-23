namespace ILSourceParser.Trivia;

public class MethodKeywordTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.MethodKeyword;

    internal MethodKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return "method";
    }
}
