using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>ldsfld</c> instruction.
/// This instruction is used to load the value of the given static field onto the evaluation stack.
/// </summary>
public class LdsfldOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax FieldType { get; init; }
    public FieldOrPropertyReferenceSyntax Target { get; init; }

    internal LdsfldOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax fieldType,
        FieldOrPropertyReferenceSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        FieldType = fieldType;
        Target = target;
    }
}
