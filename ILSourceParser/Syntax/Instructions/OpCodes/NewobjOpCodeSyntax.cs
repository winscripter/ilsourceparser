using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>newobj</c> instruction syntax.
/// This instruction creates a new instance of the given object and pushes it onto the
/// evaluation stack.
/// </summary>
public class NewobjOpCodeSyntax : OpCodeSyntax
{
    public MethodCallSyntax Constructor { get; init; }

    internal NewobjOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax constructor) : base(leadingTrivia, trailingTrivia)
    {
        Constructor = constructor;
    }
}
