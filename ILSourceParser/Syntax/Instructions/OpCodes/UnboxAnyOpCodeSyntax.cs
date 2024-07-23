using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>unbox</c> instruction syntax.
/// This instruction is used to unbox the last item on the evaluation stack - that is,
/// converting type <see langword="object"/> to the given <see cref="Type"/>.
/// </summary>
public class UnboxAnyOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Type { get; init; }

    internal UnboxAnyOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax type) : base(leadingTrivia, trailingTrivia)
    {
        Type = type;
    }
}
