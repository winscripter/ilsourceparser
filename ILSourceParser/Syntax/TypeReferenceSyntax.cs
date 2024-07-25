using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL directive that represents the reference to an existing type,
/// such as <c>[System.Runtime]System.Console</c>. This node is associated with the
/// following types:
/// <list type="bullet">
///   <item>
///     <see cref="NonGenericTypeReferenceSyntax"/>
///   </item>
///   <item>
///     <see cref="GenericTypeReferenceSyntax"/>
///   </item>
/// </list>
/// </summary>
public abstract class TypeReferenceSyntax : TypeBaseSyntax
{
    protected TypeReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia) : base(leadingTrivia, trailingTrivia)
    {
    }
}
