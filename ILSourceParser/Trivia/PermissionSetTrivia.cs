namespace ILSourceParser.Trivia;

public class PermissionSetTrivia : SyntaxTrivia
{
    public override SyntaxTriviaType Type => SyntaxTriviaType.PermissionSet;

    internal PermissionSetTrivia()
    {
    }

    public override string ToString()
    {
        return $".permissionset";
    }
}
