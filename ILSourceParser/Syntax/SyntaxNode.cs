using System.Collections.Immutable;
using ILSourceParser.Trivia;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a base class for each syntax node defined.
/// </summary>
public abstract class SyntaxNode
{
    /// <summary>
    /// Represents leading trivia - e.g. syntax trivias appearing before the
    /// syntax node.
    /// </summary>
    public abstract ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }

    /// <summary>
    /// Represents trailing trivia - e.g. syntax trivias appearing after the
    /// syntax node.
    /// </summary>
    public abstract ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
}
