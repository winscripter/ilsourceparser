using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents the IL <c>.addon</c> accessor, similar to C#'s event <c>add</c> accessor.
/// </summary>
public class AddOnAccessorSyntax : AccessorSyntax
{
    /// <summary>
    /// Accessors in IL code typically reference to another method that contains actual
    /// implementation of the accessor. This property references a method that contains
    /// the implementation of the accessor.
    /// </summary>
    public MethodCallSyntax Target { get; init; }

    internal AddOnAccessorSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
