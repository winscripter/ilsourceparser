using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class PredefinedTypeSyntax : TypeSyntax
{
    public string TypeName { get; set; }
    public PredefinedTypeKind Kind { get; set; }
    public string? Value { get; set; }

    internal PredefinedTypeSyntax(ImmutableArray<SyntaxTrivia> leadingTrivia, ImmutableArray<SyntaxTrivia> trailingTrivia, string typeName, PredefinedTypeKind kind, string? value) : base(leadingTrivia, trailingTrivia)
    {
        TypeName = typeName;
        Kind = kind;
        Value = value;
    }
}
