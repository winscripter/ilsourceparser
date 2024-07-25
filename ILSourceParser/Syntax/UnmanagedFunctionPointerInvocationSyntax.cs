using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents invocation of an IL unmanaged function pointer.
/// </summary>
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

    /// <summary>
    /// The return type of the unmanaged function pointer.
    /// </summary>
    public TypeSyntax ReturnType { get; init; }

    /// <summary>
    /// Parameters passed to the unmanaged function pointer.
    /// </summary>
    public IEnumerable<TypeSyntax> Parameters { get; init; }

    /// <summary>
    /// The calling convention of the unmanaged function pointer being invoked.
    /// </summary>
    public CallKind CallingConvention { get; init; }
}
