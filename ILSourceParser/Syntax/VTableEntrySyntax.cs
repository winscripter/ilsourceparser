using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public sealed class VTableEntrySyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public string EntryNumber { get; init; }
    public string SlotNumber { get; init; }

    internal VTableEntrySyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string entryNumber,
        string slotNumber)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        EntryNumber = entryNumber;
        SlotNumber = slotNumber;
    }
}
