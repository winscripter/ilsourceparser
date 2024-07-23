using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public class StoreElementOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }
    public TypeSyntax? ElementType { get; init; }

    internal StoreElementOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name,
        TypeSyntax? elementType) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
        ElementType = elementType;
    }
}
