namespace ILSourceParser.Trivia;

public class FieldDefinitionTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.FieldDefinition;

    internal FieldDefinitionTrivia()
    {
    }

    public override string ToString()
    {
        return ".field";
    }
}
