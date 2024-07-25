using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following kinds:
/// <list type="bullet">
///   <item>
///     <see cref="GetAccessorSyntax"/>  
///   </item>
///   <item>
///     <see cref="SetAccessorSyntax"/>
///   </item>
///   <item>
///     <see cref="AddOnAccessorSyntax"/>
///   </item>
///   <item>
///     <see cref="RemoveOnAccessorSyntax"/>
///   </item>
/// </list>
/// </summary>
public class AccessorSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    internal AccessorSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
