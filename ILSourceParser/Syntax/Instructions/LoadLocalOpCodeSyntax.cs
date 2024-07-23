using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public class LoadLocalOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }
    public uint? Index { get; init; }

    internal LoadLocalOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name,
        uint? index) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
        Index = index;
    }
}
