using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>stsfld</c> instruction.
/// This instruction is used to store data to the given static field.
/// </summary>
public class StsfldOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax FieldType { get; init; }
    public FieldOrPropertyReferenceSyntax Target { get; init; }

    internal StsfldOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax fieldType,
        FieldOrPropertyReferenceSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        FieldType = fieldType;
        Target = target;
    }
}
