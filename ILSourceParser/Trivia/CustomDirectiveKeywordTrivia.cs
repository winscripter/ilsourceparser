namespace ILSourceParser.Trivia;

public class CustomDirectiveKeywordTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.CustomDirectiveKeyword;

    internal CustomDirectiveKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return ".custom";
    }
}
