using System.Collections.Immutable;
using ILSourceParser.Trivia;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a syntax node for a byte - e.g. <c>0x05</c> or <c>1c</c>. This node is
/// typically returned by syntax nodes that heavily rely on bytes, f.e. <see cref="CustomAttributeSyntax"/>
/// or <see cref="PermissionSetSyntax"/>.
/// </summary>
public class ByteSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Represents the raw value of the byte. For instance, if <c>1c</c> was specified,
    /// this string will hold the value of <c>1c</c>. Value may be prefixed with the
    /// hexadecimal integer prefix (0x). The length of this string is always 2 characters.
    /// </summary>
    public string Value { get; init; }

    internal ByteSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string value)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        Value = value;
    }
}
