using ILSourceParser.Trivia;
using System.Collections.Immutable;
using System.Reflection;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>.ver</c> directive. This directive specifies the version of
/// the assembly, similar to <see cref="AssemblyVersionAttribute"/>.
/// </summary>
public class VerDirectiveSyntax : SyntaxNode
{
    /// <summary>
    /// Specifies the major version. In this example: <c>.ver 1 : 2 : 3 : 4</c>, <c>1</c> is the major version.
    /// </summary>
    public char Major { get; init; }

    /// <summary>
    /// Specifies the minor version. In this example: <c>.ver 1 : 2 : 3 : 4</c>, <c>2</c> is the minor version.
    /// </summary>
    public char Minor { get; init; }

    /// <summary>
    /// Specifies the build version. In this example: <c>.ver 1 : 2 : 3 : 4</c>, <c>3</c> is the build version.
    /// </summary>
    public char Build { get; init; }

    /// <summary>
    /// Specifies the revision. In this example: <c>.ver 1 : 2 : 3 : 4</c>, <c>4</c> is the revision.
    /// </summary>
    public char Revision { get; init; }

    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    internal VerDirectiveSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        char major,
        char minor,
        char build,
        char revision)
    {
        Major = major;
        Minor = minor;
        Build = build;
        Revision = revision;
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
