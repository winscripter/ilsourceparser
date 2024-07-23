using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public sealed class ConversionOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }

    internal ConversionOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
    }
}
