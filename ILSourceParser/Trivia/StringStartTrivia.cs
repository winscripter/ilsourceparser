namespace ILSourceParser.Trivia;

public class StringStartTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.StringStart;

    public bool IsDoubleQuote { get; init; }

    internal StringStartTrivia(bool isDoubleQuote = true)
    {
        IsDoubleQuote = isDoubleQuote;
    }

    public override string ToString()
    {
        return IsDoubleQuote ? "\"" : "'";
    }
}
