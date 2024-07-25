using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public sealed class ManagedFunctionPointerInvocationSyntax : FunctionPointerInvocationSyntax
{
    internal ManagedFunctionPointerInvocationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax returnType,
        IEnumerable<TypeSyntax> parameters) : base(leadingTrivia, trailingTrivia)
    {
        ReturnType = returnType;
        Parameters = parameters;
    }

    public TypeSyntax ReturnType { get; init; }
    public IEnumerable<TypeSyntax> Parameters { get; init; }
}
