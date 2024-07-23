namespace ILSourceParser.Trivia;

public class FunctionNameTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.FunctionName;

    public string FunctionName { get; init; }

    internal FunctionNameTrivia(string functionName)
    {
        FunctionName = functionName;
    }

    public override string ToString()
    {
        return FunctionName;
    }
}
