using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public class LoadIndirectOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }

    internal LoadIndirectOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
    }
}
