using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>box</c> instruction syntax.
/// This instruction pops the last value from the evaluation stack, performs type boxing
/// on it, and pushes the result back onto the stack. The parameter specifies the type of
/// the object being boxed.
/// </summary>
/// <remarks>
/// In simple terms, this type casts last item from stack to <see cref="object"/> type.
/// </remarks>
public class BoxOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax BoxedType { get; init; }

    internal BoxOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax boxedType) : base(leadingTrivia, trailingTrivia)
    {
        BoxedType = boxedType;
    }
}
