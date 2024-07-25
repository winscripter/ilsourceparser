using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class MethodDeclarationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    
    public IEnumerable<string> MethodFlags { get; init; }
    public IEnumerable<string> MethodImplementationFlags { get; init; }
    public IEnumerable<MethodFlagSyntax> AdditionalMethodImplementationFlags { get; init; }

    public TypeSyntax ReturnType { get; init; }
    public bool IsInstanceReturnType { get; init; }
    public string MethodName { get; init; }

    public PInvokeImplSyntax? PInvokeInfo { get; init; }

    public IEnumerable<ParameterSyntax> Parameters { get; init; }

    public IEnumerable<SyntaxNode> DescendantNodes { get; init; }

    public MethodDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<string> methodFlags,
        IEnumerable<string> methodImplementationFlags,
        IEnumerable<MethodFlagSyntax> additionalMethodImplementationFlags,
        TypeSyntax returnType,
        string methodName,
        PInvokeImplSyntax? pInvokeInfo,
        IEnumerable<ParameterSyntax> parameters,
        IEnumerable<SyntaxNode> descendantNodes,
        bool isInstanceReturnType)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        MethodFlags = methodFlags;
        MethodImplementationFlags = methodImplementationFlags;
        AdditionalMethodImplementationFlags = additionalMethodImplementationFlags;
        ReturnType = returnType;
        MethodName = methodName;
        PInvokeInfo = pInvokeInfo;
        Parameters = parameters;
        DescendantNodes = descendantNodes;
        IsInstanceReturnType = isInstanceReturnType;
    }
}
