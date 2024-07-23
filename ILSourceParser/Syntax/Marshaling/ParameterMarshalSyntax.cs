using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Marshaling;

/// <summary>
/// Represents IL <c>marshal()</c> for method parameters.
/// </summary>
public class ParameterMarshalSyntax : MarshalSyntax
{
    internal ParameterMarshalSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MarshalTypeSyntax marshalType) : base(leadingTrivia, trailingTrivia, marshalType)
    {
    }
}
