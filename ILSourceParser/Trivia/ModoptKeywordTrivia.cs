namespace ILSourceParser.Trivia;

public class ModoptKeywordTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.ModoptKeyword;

    internal ModoptKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return "modopt";
    }
}
