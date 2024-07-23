using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>switch</c> instruction syntax.
/// </summary>
public class SwitchOpCodeSyntax : OpCodeSyntax
{
    public SwitchInstructionBodySyntax InstructionBody { get; init; }

    internal SwitchOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        SwitchInstructionBodySyntax instructionBody) : base(leadingTrivia, trailingTrivia)
    {
        InstructionBody = instructionBody;
    }
}
