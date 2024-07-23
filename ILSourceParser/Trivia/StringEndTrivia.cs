namespace ILSourceParser.Trivia;

public class StringEndTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.StringEnd;

    public bool IsDoubleQuote { get; init; }

    internal StringEndTrivia(bool isDoubleQuote = true)
    {
        IsDoubleQuote = isDoubleQuote;
    }

    public override string ToString()
    {
        return IsDoubleQuote ? "\"" : "'";
    }
}
