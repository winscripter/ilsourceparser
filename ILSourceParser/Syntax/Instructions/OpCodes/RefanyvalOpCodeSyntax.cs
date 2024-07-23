using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>refanyval</c> instruction syntax.
/// </summary>
public class RefanyvalOpCodeSyntax : OpCodeSyntax
{
    internal RefanyvalOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax type) : base(leadingTrivia, trailingTrivia)
    {
        Type = type;
    }

    public TypeSyntax Type { get; init; }
}
