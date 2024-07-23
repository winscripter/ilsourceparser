using ILSourceParser.Trivia;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ILSourceParser.Syntax.Marshaling;

/// <summary>
/// Represents a <c>custom</c> marshalling type.
/// </summary>
public sealed class CustomMarshalTypeSyntax : MarshalTypeSyntax
{
    /// <summary>
    /// Equivalent to <see cref="MarshalAsAttribute.MarshalTypeRef"/>
    /// </summary>
    public string AssemblyString { get; init; }

    /// <summary>
    /// Equivalent to <see cref="MarshalAsAttribute.MarshalCookie"/>
    /// </summary>
    public string Cookie { get; init; }

    internal CustomMarshalTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string value,
        string assemblyString,
        string cookie) : base(leadingTrivia, trailingTrivia, value)
    {
        AssemblyString = assemblyString;
        Cookie = cookie;
    }
}
