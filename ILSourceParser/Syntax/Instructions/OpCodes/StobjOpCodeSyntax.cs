using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>stobj</c> instruction syntax.
/// </summary>
public class StobjOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Type { get; init; }

    internal StobjOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax type) : base(leadingTrivia, trailingTrivia)
    {
        Type = type;
    }
}
