using ILSourceParser.Common;

namespace ILSourceParser.Trivia;

public sealed class TypeArrayTrivia : SyntaxTrivia, ITypeTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.TypeArray;

    internal TypeArrayTrivia()
    {
    }

    public override string ToString()
    {
        return "[]";
    }
}
