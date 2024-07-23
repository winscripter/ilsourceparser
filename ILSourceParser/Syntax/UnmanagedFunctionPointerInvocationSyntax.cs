using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public sealed class UnmanagedFunctionPointerInvocationSyntax : FunctionPointerInvocationSyntax
{
    internal UnmanagedFunctionPointerInvocationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax returnType,
        IEnumerable<TypeSyntax> parameters,
        CallKind callingConv) : base(leadingTrivia, trailingTrivia)
    {
        ReturnType = returnType;
        Parameters = parameters;
        CallingConvention = callingConv;
    }

    public TypeSyntax ReturnType { get; init; }
    public IEnumerable<TypeSyntax> Parameters { get; init; }
    public CallKind CallingConvention { get; init; }
}
