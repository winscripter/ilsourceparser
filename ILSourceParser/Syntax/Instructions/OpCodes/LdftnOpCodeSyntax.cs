using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>ldftn</c> instruction syntax.
/// This instruction is used to create and push a function pointer for the given method onto the
/// evaluation stack. The <see cref="Target"/> property specifies the method to create the
/// function pointer for.
/// </summary>
public class LdftnOpCodeSyntax : OpCodeSyntax
{
    public MethodInvocationSyntax Target { get; init; }

    internal LdftnOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodInvocationSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
