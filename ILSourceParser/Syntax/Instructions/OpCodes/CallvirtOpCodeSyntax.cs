using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>callvirt</c> instruction syntax.
/// </summary>
public class CallvirtOpCodeSyntax : OpCodeSyntax
{
    public MethodInvocationSyntax Target { get; init; }

    internal CallvirtOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodInvocationSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
