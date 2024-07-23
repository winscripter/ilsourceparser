using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Marshaling;

/// <summary>
/// Represents IL <c>marshal()</c> for return types.
/// </summary>
public class ReturnMarshalSyntax : MarshalSyntax
{
    internal ReturnMarshalSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MarshalTypeSyntax marshalType) : base(leadingTrivia, trailingTrivia, marshalType)
    {
    }
}
