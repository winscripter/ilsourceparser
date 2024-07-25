using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class GenericParameterReferenceSyntax : BaseGenericParameterSyntax
{
    public int NumberOfExclamationMarks { get; init; }
    public string RawParameterReference { get; init; }

    public GenericParameterReferenceSyntax(
        int numberOfExclamationMarks,
        string rawParameterReference,
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia) : base(leadingTrivia, trailingTrivia)
    {
        NumberOfExclamationMarks = numberOfExclamationMarks;
        RawParameterReference = rawParameterReference;
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }

    public bool IsParameterReferenceByIndex
    {
        get => uint.TryParse(RawParameterReference, out _);
    }
}
