using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following types:
/// <list type="bullet">
///   <item>
///     <see cref="ModOptSyntax"/>
///   </item>
///   <item>
///     <see cref="ModReqSyntax"/>
///   </item>
/// </list>
/// </summary>
public class ModifierNotationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    protected internal ModifierNotationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
