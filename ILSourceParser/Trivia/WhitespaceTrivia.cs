namespace ILSourceParser.Trivia;

public class WhitespaceTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.Whitespace;
    public int WhiteSpaces { get; init; }

    internal WhitespaceTrivia(int whiteSpaces)
    {
        WhiteSpaces = whiteSpaces;
    }

    public override string ToString()
    {
        return new(' ', WhiteSpaces);
    }
}
