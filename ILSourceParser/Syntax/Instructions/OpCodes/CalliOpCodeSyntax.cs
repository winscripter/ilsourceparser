using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>calli</c> instruction syntax.
/// This instruction is used to invoke the given function pointer and push its result onto the evaluation stack.
/// </summary>
public class CalliOpCodeSyntax : OpCodeSyntax
{
    public FunctionPointerInvocationSyntax FunctionPointer { get; init; }

    internal CalliOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        FunctionPointerInvocationSyntax functionPointer) : base(leadingTrivia, trailingTrivia)
    {
        FunctionPointer = functionPointer;
    }
}
