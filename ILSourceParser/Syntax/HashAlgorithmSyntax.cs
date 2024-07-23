using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class HashAlgorithmSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    
    public string Value { get; init; }

    public HashAlgorithmSyntax(ImmutableArray<SyntaxTrivia> leadingTrivia, ImmutableArray<SyntaxTrivia> trailingTrivia, string value)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        Value = value;
    }

    public HashMode GetHashMode()
    {
        try
        {
            return (HashMode)Convert.ToUInt32(Value, 16);
        }
        catch
        {
            try
            {
                return (HashMode)Convert.ToUInt32(Value);
            }
            catch
            {
                return HashMode.Unknown;
            }
        }
    }
}
