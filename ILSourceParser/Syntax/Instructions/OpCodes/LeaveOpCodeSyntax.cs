using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>leave</c> instruction.
/// </summary>
public class LeaveOpCodeSyntax : OpCodeSyntax
{
    public string Target { get; init; }

    internal LeaveOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>leave.s</c> instruction.
/// </summary>
public class LeaveSOpCodeSyntax : OpCodeSyntax
{
    public string Target { get; init; }

    internal LeaveSOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
