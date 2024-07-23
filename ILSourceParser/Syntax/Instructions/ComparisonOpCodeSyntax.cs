using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public sealed class ComparisonOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }

    internal ComparisonOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
    }
}
