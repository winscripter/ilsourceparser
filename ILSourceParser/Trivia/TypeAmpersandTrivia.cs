using ILSourceParser.Common;

namespace ILSourceParser.Trivia;

public sealed class TypeAmpersandTrivia : SyntaxTrivia, ITypeTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.TypeAmpersand;

    internal TypeAmpersandTrivia()
    {
    }

    public override string ToString()
    {
        return "&";
    }
}
