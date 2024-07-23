using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>sizeof</c> instruction syntax.
/// This instruction is used to push 'uint32' onto the stack that represents the size of
/// the given type in bytes. For instance, <c>sizeof(int)</c> in C# will be translated to <c>sizeof int32</c> or <c>sizeof [System.Private.CoreLib]System.Int32</c> in IL.
/// </summary>
public class SizeofOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Type { get; init; }

    internal SizeofOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax type) : base(leadingTrivia, trailingTrivia)
    {
        Type = type;
    }
}
