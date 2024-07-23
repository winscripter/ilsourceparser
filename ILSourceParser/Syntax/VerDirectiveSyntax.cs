using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class VerDirectiveSyntax : SyntaxNode
{
    public char Major { get; init; }
    public char Minor { get; init; }
    public char Build { get; init; }
    public char Revision { get; init; }

    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    public VerDirectiveSyntax(
        char major, char minor, char build, char revision, 
        ImmutableArray<SyntaxTrivia> leadingTrivia, ImmutableArray<SyntaxTrivia> trailingTrivia)
    {
        Major = major;
        Minor = minor;
        Build = build;
        Revision = revision;
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
    }

    public Version AsVersion()
    {
        return new(Major - '0', Minor - '0', Build - '0', Revision - '0');
    }
}
