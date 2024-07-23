namespace ILSourceParser.Trivia;

public class ByteArrayKeywordTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.ByteArrayKeyword;

    internal ByteArrayKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return "bytearray";
    }
}
