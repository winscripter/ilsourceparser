namespace ILSourceParser.Trivia;

public class HashAlgorithmTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.HashAlgorithm;

    internal HashAlgorithmTrivia()
    {
    }

    public override string ToString()
    {
        return ".hash algorithm";
    }
}
