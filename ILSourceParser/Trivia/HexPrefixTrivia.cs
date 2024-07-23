namespace ILSourceParser.Trivia;

public class HexPrefixTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.HexadecimalPrefix;

    internal HexPrefixTrivia()
    {
    }

    public override string ToString()
    {
        return "0x";
    }
}
