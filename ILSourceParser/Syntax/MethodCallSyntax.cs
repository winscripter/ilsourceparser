using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class MethodCallSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public bool IsReturnInstance { get; init; }
    public TypeSyntax ReturnType { get; init; }
    public IEnumerable<ModifierNotationSyntax> Modifiers { get; init; }
    public MethodInvocationSyntax MethodInvocation { get; init; }

    internal MethodCallSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        bool isReturnInstance,
        TypeSyntax returnType,
        MethodInvocationSyntax methodInvocation,
        IEnumerable<ModifierNotationSyntax> modifiers)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        IsReturnInstance = isReturnInstance;
        ReturnType = returnType;
        MethodInvocation = methodInvocation;
        Modifiers = modifiers;
    }
}
