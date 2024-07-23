using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericParameterReferenceTypeSyntax : TypeSyntax
{
    public int NumberOfExclamationMarks { get; init; }
    public string RawParameterReference { get; init; }

    public GenericParameterReferenceTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        int numberOfExclamationMarks,
        string rawParameterReference) : base(leadingTrivia, trailingTrivia)
    {
        NumberOfExclamationMarks = numberOfExclamationMarks;
        RawParameterReference = rawParameterReference;
    }

    public bool IsParameterReferenceByIndex
    {
        get => uint.TryParse(RawParameterReference, out _);
    }
}
