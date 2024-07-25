using ILSourceParser.Common;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a syntax node for predefined types. Those include:
/// <list type="bullet">
///   <item>int8</item>
///   <item>int16</item>
///   <item>int32</item>
///   <item>int64</item>
///   <item>uint8</item>
///   <item>uint16</item>
///   <item>uint32</item>
///   <item>uint64</item>
///   <item>float32</item>
///   <item>float64</item>
/// </list>
/// This syntax node also specifies values passed to these types as functions. For
/// example, <c>int32(48)</c> is perfectly valid in IL code and such will result in
/// <see cref="PredefinedTypeSyntax"/>, which will also specify <c>48</c> passed to the
/// <c>int32</c> function.
/// </summary>
public class PredefinedTypeSyntax : TypeSyntax
{
    /// <summary>
    /// Represents the name of the type as a string. In this example: <c>int32(48)</c>,
    /// the value of this property will be 48 as a string.
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// Represents the predefined type as <see cref="PredefinedTypeKind"/>.
    /// </summary>
    public PredefinedTypeKind Kind { get; set; }

    /// <summary>
    /// Represents the value passed to the function of the predefined type. For example,
    /// <c>int32(48)</c> is valid in IL code, and parsing that will result in this property
    /// having a value of 48. However, if it is just the <c>int32</c> keyword and not a function,
    /// this property will have a value of <see langword="null"/>.
    /// </summary>
    public string? Value { get; set; }

    internal PredefinedTypeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string typeName,
        PredefinedTypeKind kind,
        string? value) : base(leadingTrivia, trailingTrivia)
    {
        TypeName = typeName;
        Kind = kind;
        Value = value;
    }
}
