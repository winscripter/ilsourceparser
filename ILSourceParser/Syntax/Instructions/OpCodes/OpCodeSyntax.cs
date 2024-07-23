using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

public abstract class OpCodeSyntax : InstructionSyntax
{
    protected internal OpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia) : base(leadingTrivia, trailingTrivia)
    {
    }
}
