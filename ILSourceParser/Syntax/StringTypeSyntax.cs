using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class StringTypeSyntax : TypeSyntax
{
    internal StringTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string? value) : base(leadingTrivia, trailingTrivia)
    {
        Value = value;
    }

    public string? Value { get; set; }

    public bool IsJustStringKeyword
    {
        get => Value is null;
    }
}
