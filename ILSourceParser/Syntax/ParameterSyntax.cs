using ILSourceParser.Syntax.Marshaling;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class ParameterSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public IEnumerable<ParameterModifierSyntax> Modifiers { get; init; }
    public ParameterMarshalSyntax? Marshalling { get; init; }
    public TypeSyntax? ParameterType { get; init; }
    public string? Name { get; init; }

    internal ParameterSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<ParameterModifierSyntax> modifiers,
        ParameterMarshalSyntax? marshalling,
        TypeSyntax? parameterType,
        string? name)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        Modifiers = modifiers;
        Marshalling = marshalling;
        ParameterType = parameterType;
        Name = name;
    }
}
