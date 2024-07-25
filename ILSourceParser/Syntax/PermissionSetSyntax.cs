using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents the IL <c>.permissionset</c> directive syntax.
/// </summary>
public class PermissionSetSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Represents the security action for this permission set. For example, in the
    /// following example:
    /// <code>
    ///   .permissionset reqmin = (
    ///       // ...
    ///   )
    /// </code>
    /// .. this property will have a value of <see cref="SecurityAction.ReqMin"/>.
    /// </summary>
    public SecurityAction SecurityAction { get; init; }

    /// <summary>
    /// The byte array that contains the bytes of the permission set.
    /// </summary>
    public IEnumerable<ByteSyntax> ByteArray { get; init; }

    internal PermissionSetSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        SecurityAction securityAction,
        IEnumerable<ByteSyntax> byteArray)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        SecurityAction = securityAction;
        ByteArray = byteArray;
    }
}
