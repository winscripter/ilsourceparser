using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>ldftn</c> instruction syntax.
/// </summary>
public class LdvirtftnOpCodeSyntax : OpCodeSyntax
{
    public MethodCallSyntax Target { get; init; }

    internal LdvirtftnOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
