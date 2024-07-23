using ILSourceParser.Common;

namespace ILSourceParser.Trivia;

public class TypeAsteriskTrivia : SyntaxTrivia, ITypeTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.TypeAsterisk;

    internal TypeAsteriskTrivia()
    {
    }

    public override string ToString()
    {
        return "*";
    }
}
