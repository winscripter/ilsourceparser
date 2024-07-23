using ILSourceParser.Common;
using ILSourceParser.Syntax;
using ILSourceParser.Syntax.Instructions;
using Sprache;
using System.Runtime.InteropServices;

namespace ILSourceParser;

internal static class ParserResources
{
    private static readonly Lazy<IEnumerable<char>> s_hexChars = new(
        () => ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F']);

    private static readonly Lazy<IEnumerable<char>> s_validStrEscapeSequences =
        new(() =>
        {
            return ['a', 'b', 'f', 'n', 'r', 't', 'v', '0', 'u', '\\', '\'', '"'];
            // IL doesn't support \x or \U to my knowledge.
        });

    internal static IEnumerable<char> HexChars
    {
        get
        {
            return s_hexChars.Value;
        }
    }

    internal static IEnumerable<char> ValidStringEscapeSequences
    {
        get
        {
            return s_validStrEscapeSequences.Value;
        }
    }

    public const string AssemblyNameParser = "Assembly Name Body Parser";
    public const string SymbolNameParser = "Common Symbol and Type Name Parser";

    public static bool TypeNameParseActionWithSingleQuote(char input)
    {
        return input != '\'';
    }

    public static bool TypeNameParseAction(char input)
    {
        return char.IsLetterOrDigit(input) && input is '_' or '.' or '`' or '/' or '+' or ':';
    }

    private static readonly Lazy<IEnumerable<char>> s_specialSymCharacters = new(
        () => ['_', '/', '+', '<', '>', '$']);
    public static IEnumerable<char> SpecialSymbolCharacters
    {
        get => s_specialSymCharacters.Value;
    }

    // The dummy type in the IL Source Parser must be a real type, something like
    // an empty string is unacceptable. I will use "System.__Canon". This is a real type,
    // it does have an 'internal' modifier though. Despite all this, this class is just
    // empty, however, it is still used in the .NET runtime even today. In fact, some
    // developers think this is for .NET Framework only - no no no, __Canon is still a
    // thing even in .NET 9 and is still used, but it cannot be used by developers since
    // this type, yet again, has the 'internal' modifier.
    // 
    // From within C# or IL code, this type should not be used because, well, it has an
    // internal type. If you ever encounter this type while parsing IL and you know the
    // original source code doesn't use System.__Canon, it most likely means there is a
    // parsing error - watch out for parsing diagnostics first.
    const string dummyTypeName = "System.__Canon";

    public static TypeReferenceSyntax DummyTypeReference()
    {
        return new NonGenericTypeReferenceSyntax(
            leadingTrivia: [],
            trailingTrivia: [],
            null,
            null,
            dummyTypeName,
            null);
    }

    public static bool GenericArgumentParseAction(char c) =>
        char.IsLetterOrDigit(c) || c == '_' || c == '!';
    // Examples: '!!T', '!!T_something', '!T', 'T_something"

    private static readonly Lazy<MethodCallSyntax> s_dummyMethodCall =
        new(() =>
        {
            return new MethodCallSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        isReturnInstance: false,
                        returnType: new PredefinedTypeSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            "int32",
                            PredefinedTypeKind.Int32,
                            null
                        ),
                        methodInvocation: DummyMethodInvocation,
                        modifiers: []
                    );
        });

    public static MethodCallSyntax DummyMethodCall() => s_dummyMethodCall.Value;

    private static readonly Lazy<CustomAttributeSyntax> s_dummyAttribute =
        new(() =>
        {
            return new CustomAttributeSyntax(
                leadingTrivia: [],
                trailingTrivia: [],
                attributeData: [],
                attributeConstructorTarget: DummyMethodCall());
        });

    public static CustomAttributeSyntax DummyAttribute() => s_dummyAttribute.Value;

    public static bool CommentBodyParseAction(char c) => c != '\n';

    public static Parser<ParameterlessOpCodeSyntax> DummyInstructionNoOpCode => Parse.Return(
        new ParameterlessOpCodeSyntax(
            leadingTrivia: [],
            trailingTrivia: [],
            name: "/*Unknown opcode*/"));

    public static Parser<BranchOpCodeSyntax> DummyBranchInstruction => Parse.Return(
        new BranchOpCodeSyntax(
            leadingTrivia: [],
            trailingTrivia: [],
            name: "/*Unknown opcode*/",
            targetLabel: "/*Unknown target*/"));

    public static FunctionPointerInvocationSyntax DummyFuncPtrInvocation =>
        new ManagedFunctionPointerInvocationSyntax(
            leadingTrivia: [],
            trailingTrivia: [],
            returnType: DummyTypeReference(),
            parameters: []);

    public static MethodInvocationSyntax DummyMethodInvocation =>
        new(
            leadingTrivia: [],
            trailingTrivia: [],
            typeReference: DummyTypeReference(),
            methodName: "<unknown>",
            arguments: new ArgumentListReferenceSyntax(
                leadingTrivia: [],
                trailingTrivia: [],
                arguments: []
            )
        );

    public static FieldOrPropertyReferenceSyntax DummyFieldOrPropertyReference()
    {
        return new FieldOrPropertyReferenceSyntax(
            leadingTrivia: [],
            trailingTrivia: [],
            declaringTypeReference: DummyTypeReference(),
            objectName: "<unknown>",
            fieldType: DummyTypeReference()
        );
    }

    public static StringLiteralSyntax DummyStringLiteral()
    {
        return new StringLiteralSyntax(
            leadingTrivia: [],
            trailingTrivia: [],
            string.Empty
        );
    }

    public const string VTableUnknownEntryError = "/*Unknown entry number, defaulting to 1*/ 1";
    public const string VTableUnknownSlotError = "/*Unknown slot number, defaulting to 1*/ 1";

    public const string FileAlignmentDirectiveError = "/*Unknown file alignment*/";

    public const CallingConvention DefaultCallingConvention = CallingConvention.Winapi;

    public static bool LabelParseAction(char c) => char.IsLetterOrDigit(c) || c == '_';
}
