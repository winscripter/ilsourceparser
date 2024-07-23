namespace ILSourceParser.Trivia;

public class ModreqKeywordTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.ModreqKeyword;

    internal ModreqKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return "modreq";
    }
}
