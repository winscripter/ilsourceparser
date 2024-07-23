using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class PropertyDeclarationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public TypeSyntax ReturnType { get; init; }
    public bool IsReturnTypeInstance { get; init; }
    public string Name { get; init; }
    public GetAccessorSyntax? GetAccessor { get; init; }
    public SetAccessorSyntax? SetAccessor { get; init; }
    public IEnumerable<BaseCustomAttributeSyntax> CustomAttributes { get; init; }

    internal PropertyDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax returnType,
        bool isReturnTypeInstance,
        string name,
        GetAccessorSyntax? getAccessor,
        SetAccessorSyntax? setAccessor,
        IEnumerable<BaseCustomAttributeSyntax> customAttributes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        ReturnType = returnType;
        IsReturnTypeInstance = isReturnTypeInstance;
        Name = name;
        GetAccessor = getAccessor;
        SetAccessor = setAccessor;
        CustomAttributes = customAttributes;
    }
}
