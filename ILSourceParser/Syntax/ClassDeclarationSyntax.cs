using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class ClassDeclarationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public IEnumerable<string> Flags { get; init; }
    public string Name { get; init; }
    public TypeSyntax? ClassExtends { get; init; }
    public IEnumerable<TypeSyntax>? ClassImplements { get; init; }
    public IEnumerable<SyntaxNode> DescendantNodes { get; init; }

    internal ClassDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<string> flags,
        string name,
        TypeSyntax? classExtends,
        IEnumerable<TypeSyntax>? classImplements,
        IEnumerable<SyntaxNode> descendantNodes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        Flags = flags;
        Name = name;
        ClassExtends = classExtends;
        ClassImplements = classImplements;
        DescendantNodes = descendantNodes;
    }
}
