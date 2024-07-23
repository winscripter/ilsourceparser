namespace ILSourceParser.Trivia;

public class IntegerNegativeMarkTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.IntegerNegativeMark;

    internal IntegerNegativeMarkTrivia()
    {
    }

    public override string ToString()
    {
        return "-";
    }
}
