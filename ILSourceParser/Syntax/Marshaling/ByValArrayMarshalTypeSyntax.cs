using ILSourceParser.Trivia;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ILSourceParser.Syntax.Marshaling;

/// <summary>
/// Represents a <c>byval array</c> marshalling type with the size constant.
/// </summary>
public class ByValArrayMarshalTypeSyntax : MarshalTypeSyntax
{
    /// <summary>
    /// The size constant of the array.
    /// Equivalent to <see cref="MarshalAsAttribute.SizeConst"/>.
    /// </summary>
    public int SizeConst { get; init; }

    internal ByValArrayMarshalTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string value,
        int sizeConst) : base(leadingTrivia, trailingTrivia, value)
    {
        SizeConst = sizeConst;
    }
}
