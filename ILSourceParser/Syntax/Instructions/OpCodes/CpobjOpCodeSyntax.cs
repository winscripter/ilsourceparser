using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>cpobj</c> instruction.
/// </summary>
public class CpobjOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Target { get; init; }

    internal CpobjOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
