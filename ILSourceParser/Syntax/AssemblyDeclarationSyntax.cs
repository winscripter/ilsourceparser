using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class AssemblyDeclarationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public string AssemblyName { get; init; }
    public bool IsExtern { get; init; }
    public IEnumerable<SyntaxNode> DescendantNodes { get; init; }

    internal AssemblyDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string assemblyName,
        bool isExtern,
        IEnumerable<SyntaxNode> descendantNodes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        AssemblyName = assemblyName;
        IsExtern = isExtern;
        DescendantNodes = descendantNodes;
    }
}
