using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>mkrefany</c> instruction syntax.
/// </summary>
public class MkrefanyOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax BoxedType { get; init; }

    internal MkrefanyOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax boxedType) : base(leadingTrivia, trailingTrivia)
    {
        BoxedType = boxedType;
    }
}
