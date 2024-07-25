using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a base node for every other syntax node that specifies a type. This syntax
/// node is associated with the following nodes:
/// <list type="bullet">
///   <item>
///     <see cref="GenericTypeReferenceSyntax"/>
///   </item>
///   <item>
///     <see cref="NonGenericTypeReferenceSyntax"/>
///   </item>
///   <item>
///     <see cref="FunctionPointerSyntax"/>
///   </item>
///   <item>
///     <see cref="GenericParameterReferenceTypeSyntax"/>
///   </item>
///   <item>
///     <see cref="ByteArraySyntax"/>
///   </item>
/// </list>
/// </summary>
public abstract class TypeSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    protected TypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }
}
