using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>initobj</c> instruction.
/// </summary>
public class InitobjOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Target { get; init; }

    internal InitobjOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
