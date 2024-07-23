namespace ILSourceParser.Trivia;

public enum SyntaxTriviaType
{
    Whitespace,
    HexadecimalPrefix,
    StringStart,
    StringEnd,
    ByteArrayKeyword,
    OpenParenthesis,
    CloseParenthesis,
    OpenBracket,
    CloseBracket,
    IntegerNegativeMark,
    FunctionName,
    HashAlgorithm,
    PermissionSet,
    BoolKeyword,
    FieldDefinition,
    EqualsCharacter,
    CustomDirectiveKeyword,
    InlineCommentStart,
    MultilineCommentStart,
    MultilineCommentEnd,
    ModoptKeyword,
    ModreqKeyword,
    MethodKeyword,
    UnmanagedKeyword,
    TypeAsterisk,
    TypeArray,
    TypeAmpersand
}
