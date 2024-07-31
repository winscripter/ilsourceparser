using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following nodes:
/// <list type="bullet">
///   <item>
///     <see cref="ManagedFunctionPointerInvocationSyntax"/>
///   </item>
///   <item>
///     <see cref="UnmanagedFunctionPointerInvocationSyntax"/>
///   </item>
/// </list>
/// </summary>
public class FunctionPointerInvocationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    protected internal FunctionPointerInvocationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia) =>
        (LeadingTrivia, TrailingTrivia) = (leadingTrivia, trailingTrivia);
}
