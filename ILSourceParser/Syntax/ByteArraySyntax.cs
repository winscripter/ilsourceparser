using ILSourceParser.Trivia;
using ILSourceParser.Utilities;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a syntax node for the <c>bytearray()</c> IL type.
/// </summary>
/// <remarks>
///   Not to be confused with the byte array, this node refers to the <c>bytearray(...)</c>
///   IL type. Byte arrays are simply represented as <see cref="IEnumerable{ByteSyntax}"/>
///   of type <see cref="ByteSyntax"/>.
/// </remarks>
public class ByteArraySyntax : TypeSyntax
{
    /// <summary>
    /// A list of bytes passed to the <c>bytearray</c> type.
    /// </summary>
    public IEnumerable<ByteSyntax> Bytes { get; init; }

    internal ByteArraySyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<ByteSyntax> bytes) : base(leadingTrivia, trailingTrivia)
    {
        Bytes = bytes;
    }
}
