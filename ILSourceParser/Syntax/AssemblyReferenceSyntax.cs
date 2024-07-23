using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a syntax node for assembly references. For example, in this type reference:
/// <example>
///   <code>
///     [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor()
///   </code>
/// </example>
/// .. "[System.Runtime]" will be part of this syntax node.
/// </summary>
public class AssemblyReferenceSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Represents the name of the assembly in the assembly reference. For example, in this type reference:
    /// <example>
    ///   <code>
    ///     [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor()
    ///   </code>
    /// </example>
    /// .. this property will hold a value of "System.Runtime". Brackets at start
    /// and end are removed automatically.
    /// </summary>
    public string AssemblyName { get; set; }

    internal AssemblyReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string assemblyName)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        AssemblyName = assemblyName;
    }
}
