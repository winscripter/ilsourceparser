using ILSourceParser.Trivia;
using System.Collections.Immutable;
using System.Globalization;

namespace ILSourceParser.Syntax;

public class MetadataTokenSyntax : TypeSyntax
{
    public MetadataTokenSyntax(ImmutableArray<SyntaxTrivia> leadingTrivia, ImmutableArray<SyntaxTrivia> trailingTrivia, string value) : base(leadingTrivia, trailingTrivia)
    {
        Value = value;
    }

    public string Value { get; init; }

    public uint ValueUInt32
    {
        get => uint.Parse(Value, CultureInfo.InvariantCulture);
    }
}
