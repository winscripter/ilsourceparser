using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Marshaling;

/// <summary>
/// Represents IL <c>marshal()</c> for fields.
/// </summary>
public class FieldMarshalSyntax : MarshalSyntax
{
    internal FieldMarshalSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MarshalTypeSyntax marshalType) : base(leadingTrivia, trailingTrivia, marshalType)
    {
    }
}
