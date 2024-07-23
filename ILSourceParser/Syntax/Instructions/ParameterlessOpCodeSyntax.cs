using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public sealed class ParameterlessOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }

    internal ParameterlessOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
    }
}
