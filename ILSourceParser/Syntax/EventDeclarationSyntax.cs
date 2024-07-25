using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class EventDeclarationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public TypeSyntax ReturnType { get; init; }
    public string Name { get; init; }
    public AddOnAccessorSyntax? AddOnAccessor { get; init; }
    public RemoveOnAccessorSyntax? RemoveOnAccessor { get; init; }
    public IEnumerable<BaseCustomAttributeSyntax> CustomAttributes { get; init; }

    internal EventDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax returnType,
        string name,
        AddOnAccessorSyntax? addOnAccessor,
        RemoveOnAccessorSyntax? removeOnAccessor,
        IEnumerable<BaseCustomAttributeSyntax> customAttributes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        ReturnType = returnType;
        Name = name;
        AddOnAccessor = addOnAccessor;
        RemoveOnAccessor = removeOnAccessor;
        CustomAttributes = customAttributes;
    }
}
