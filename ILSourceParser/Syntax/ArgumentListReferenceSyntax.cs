using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a syntax node for argument list references. This is used in
/// <see cref="MethodInvocationSyntax"/> to specify parameters passed to the
/// method that's being invoked.
/// </summary>
public sealed class ArgumentListReferenceSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// An enumerable of parameters passed to the method being invoked.
    /// </summary>
    public IEnumerable<TypeSyntax> Arguments { get; init; }

    internal ArgumentListReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<TypeSyntax> arguments)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        Arguments = arguments;
    }
}
