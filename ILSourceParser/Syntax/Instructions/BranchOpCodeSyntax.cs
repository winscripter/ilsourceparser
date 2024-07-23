using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public sealed class BranchOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }
    public string TargetLabel { get; init; }

    internal BranchOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name,
        string targetLabel) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
        TargetLabel = targetLabel;
    }
}
