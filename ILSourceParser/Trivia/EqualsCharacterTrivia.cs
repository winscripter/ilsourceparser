namespace ILSourceParser.Trivia;

public class EqualsCharacterTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.EqualsCharacter;

    internal EqualsCharacterTrivia()
    {
    }

    public override string ToString()
    {
        return "=";
    }
}
