using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>.vtentry</c> directive.
/// </summary>
public sealed class VTableEntrySyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// The entry number. In this example: <c>.vtentry 1 : 2</c>, <c>1</c> is the entry number.
    /// </summary>
    public string EntryNumber { get; init; }

    /// <summary>
    /// The slot number. In this example: <c>.vtentry 1 : 2</c>, <c>2</c> is the slot number.
    /// </summary>
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
