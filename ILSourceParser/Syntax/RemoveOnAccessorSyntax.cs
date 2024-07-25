using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents the IL <c>.removeon</c> accessor, similar to C#'s event <c>remove</c> accessor.
/// </summary>
public class RemoveOnAccessorSyntax : AccessorSyntax
{
    /// <summary>
    /// Accessors in IL code typically reference to another method that contains actual
    /// implementation of the accessor. This property references a method that contains
    /// the implementation of the accessor.
    /// </summary>
    public MethodCallSyntax Target { get; init; }

    internal RemoveOnAccessorSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
