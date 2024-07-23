namespace ILSourceParser.Trivia;

public class BoolKeywordTrivia : SyntaxTrivia
{
    private const string BOOL = "bool";

    public override SyntaxTriviaType Type => SyntaxTriviaType.BoolKeyword;

    internal BoolKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return BOOL;
    }
}
