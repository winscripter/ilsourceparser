using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public class PushNumberToStackOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }
    public PredefinedTypeSyntax? Parameter { get; init; }

    protected internal PushNumberToStackOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name,
        PredefinedTypeSyntax? parameter) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
        Parameter = parameter;
    }
}
