using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Marshaling;

public class SimpleMarshalTypeSyntax : MarshalTypeSyntax
{
    protected internal SimpleMarshalTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string value) : base(leadingTrivia, trailingTrivia, value)
    {
    }
}
