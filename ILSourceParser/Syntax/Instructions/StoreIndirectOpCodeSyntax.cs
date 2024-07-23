using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public class StoreIndirectOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }

    internal StoreIndirectOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
    }
}
