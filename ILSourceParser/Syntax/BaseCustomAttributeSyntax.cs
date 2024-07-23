using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following kinds:
/// <list type="bullet">
///   <item>
///     <see cref="CustomAttributeSyntax"/>
///   </item>
///   <item>
///     <see cref="AnonymousCustomAttributeSyntax"/>
///   </item>
/// </list>
/// </summary>
public abstract class BaseCustomAttributeSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    internal BaseCustomAttributeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
