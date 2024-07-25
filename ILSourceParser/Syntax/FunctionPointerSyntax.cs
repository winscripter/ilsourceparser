using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class FunctionPointerSyntax : TypeSyntax
{
    public TypeSyntax ReturnType { get; init; }
    public ImmutableArray<TypeSyntax> Arguments { get; init; }
    public CallKind CallingConvention { get; init; }
    public IEnumerable<ModifierNotationSyntax> Modifiers { get; init; }
    public Management Management { get; init; }

    internal FunctionPointerSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax returnType,
        ImmutableArray<TypeSyntax> arguments,
        Management management,
        CallKind callingConvention,
        IEnumerable<ModifierNotationSyntax> modifiers) : base(leadingTrivia, trailingTrivia)
    {
        ReturnType = returnType;
        Arguments = arguments;
        Management = management;
        CallingConvention = callingConvention;
        Modifiers = modifiers;
    }
}
