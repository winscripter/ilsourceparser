using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>call</c> instruction syntax.
/// This instruction is used to invoke the given method and push its result onto the evaluation stack.
/// </summary>
public class CallOpCodeSyntax : OpCodeSyntax
{
    public MethodCallSyntax Method { get; init; }

    internal CallOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax method) : base(leadingTrivia, trailingTrivia)
    {
        Method = method;
    }
}
