using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class NonGenericTypeReferenceSyntax : TypeReferenceSyntax
{
    public AssemblyReferenceSyntax? AssemblyReference { get; init; }
    public TypePrefix? Prefix { get; init; }
    public string ClassName { get; init; }
    public KnownSpecialMethodType? SpecialMethodType { get; init; }

    public NonGenericTypeReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        AssemblyReferenceSyntax? assemblyReference,
        TypePrefix? prefix,
        string className,
        KnownSpecialMethodType? specialMethodType) : base(leadingTrivia, trailingTrivia)
    {
        AssemblyReference = assemblyReference;
        Prefix = prefix;
        ClassName = className;
        SpecialMethodType = specialMethodType;
    }
}
