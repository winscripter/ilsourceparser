namespace ILSourceParser.Trivia;

public class UnmanagedKeywordTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.UnmanagedKeyword;

    internal UnmanagedKeywordTrivia()
    {
    }

    public override string ToString()
    {
        return "unmanaged";
    }
}
