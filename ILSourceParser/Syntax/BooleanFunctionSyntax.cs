using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents the <c>bool()</c> IL function.
/// </summary>
/// <remarks>
///   Not to be confused with a simple <c>bool</c> type, this node specifies the
///   <c>bool()</c> function, e.g. <c>bool(true)</c>.
/// </remarks>
public class BooleanFunctionSyntax : TypeSyntax
{
    /// <summary>
    /// A boolean passed to the function.
    /// </summary>
    public BooleanLiteralSyntax Value { get; init; }

    internal BooleanFunctionSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia, 
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        BooleanLiteralSyntax value) : base(leadingTrivia, trailingTrivia)
    {
        Value = value;
    }
}
