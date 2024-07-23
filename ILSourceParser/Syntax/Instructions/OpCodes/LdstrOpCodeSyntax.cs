using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>ldstr</c> instruction.
/// This instruction is used to push a string onto the evaluation stack.
/// </summary>
public class LdstrOpCodeSyntax : OpCodeSyntax
{
    public StringLiteralSyntax String { get; init; }

    internal LdstrOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        StringLiteralSyntax @string) : base(leadingTrivia, trailingTrivia)
    {
        String = @string;
    }
}
