using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public class StoreLocalOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }
    public uint? Index { get; init; }

    internal StoreLocalOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name,
        uint? index) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
        Index = index;
    }
}
