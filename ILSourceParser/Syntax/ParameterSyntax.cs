using ILSourceParser.Syntax.Marshaling;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents declaration of a method parameter in IL code.
/// </summary>
public class ParameterSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Specifies parameter modifiers of this parameter. For example, <c>[out]</c>, <c>[in]</c>,
    /// and <c>[opt]</c>.
    /// </summary>
    public IEnumerable<ParameterModifierSyntax> Modifiers { get; init; }

    /// <summary>
    /// Specifies the marshalling of this parameter, if specified (e.g. the IL <c>marshal()</c>
    /// function).
    /// </summary>
    public ParameterMarshalSyntax? Marshalling { get; init; }

    /// <summary>
    /// Specifies the type of this parameter, such as string or int32.
    /// </summary>
    public TypeSyntax ParameterType { get; init; }

    /// <summary>
    /// Specifies the name of this parameter. In IL code, parameter names are optional, so
    /// this could have a value of null.
    /// </summary>
    public string? Name { get; init; }

    internal ParameterSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<ParameterModifierSyntax> modifiers,
        ParameterMarshalSyntax? marshalling,
        TypeSyntax parameterType,
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
