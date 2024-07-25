using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericTypeReferenceSyntax : TypeReferenceSyntax
{
    public AssemblyReferenceSyntax? AssemblyReference { get; init; }
    public TypePrefix? Prefix { get; init; }
    public string ClassName { get; init; }
    public KnownSpecialMethodType? SpecialMethodType { get; init; }
    public GenericArgumentsReferenceSyntax? GenericArguments { get; init; }
    
    public GenericTypeReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        AssemblyReferenceSyntax? assemblyReference,
        TypePrefix? prefix,
        string className,
        KnownSpecialMethodType? specialMethodType,
        GenericArgumentsReferenceSyntax? genericArguments) : base(leadingTrivia, trailingTrivia)
    {
        AssemblyReference = assemblyReference;
        Prefix = prefix;
        ClassName = className;
        SpecialMethodType = specialMethodType;
        GenericArguments = genericArguments;
    }
}
