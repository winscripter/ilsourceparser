using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents the IL <c>.locals</c> directive.
/// </summary>
public class LocalsDirectiveSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Specifies whether the <c>init</c> flag is passed or not (e.g. is this <c>.locals init</c>
    /// or just <c>.locals</c>?).
    /// </summary>
    public bool IsLocalsInit { get; init; }

    /// <summary>
    /// Represents variables of this directive.
    /// </summary>
    public IEnumerable<LocalVariableSyntax> Variables { get; init; }

    internal LocalsDirectiveSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        bool isLocalsInit,
        IEnumerable<LocalVariableSyntax> variables)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        IsLocalsInit = isLocalsInit;
        Variables = variables;
    }
}
