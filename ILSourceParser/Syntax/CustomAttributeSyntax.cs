using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an anonymous custom attribute in IL code, similar to C#'s attributes. Example:
/// <code>
///   .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor() = (
///       01 00 00 00
///   )
/// </code>
/// There is a different syntax node to represent custom attributes that omit the
/// byte data <c>= ( 01 00 00 00 )</c>, which is <see cref="AnonymousCustomAttributeSyntax"/>.
/// </summary>
/// <remarks>
///   It is important to acknowledge that parsing attribute byte data in IL is
///   harder than in C# and generally requires a deeper understanding on how
///   attributes in IL bytecode work.
/// </remarks>
public class CustomAttributeSyntax : BaseCustomAttributeSyntax
{
    /// <summary>
    /// Represents the invocation to the constructor. For example, in this
    /// custom attribute:
    /// <code>
    ///   .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor() = (
    ///       01 00 00 00
    ///   )
    /// </code>
    /// .. this property will hold a reference to <c>[System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor()</c>,
    /// specifically the constructor method <c>.ctor()</c>, and the
    /// return type <c>instance void</c>.
    /// </summary>
    public MethodCallSyntax AttributeConstructorTarget { get; init; }

    /// <summary>
    /// Represents the attribute data of this custom attribute. For example,
    /// in this custom attribute:
    /// <code>
    ///   .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor() = (
    ///       01 00 00 00
    ///   )
    /// </code>
    /// .. this property will hold byte data of the custom attribute,
    /// being <c>01 00 00 00</c> combined.
    /// </summary>
    public IEnumerable<ByteSyntax> AttributeData { get; init; }

    internal CustomAttributeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        IEnumerable<ByteSyntax> attributeData,
        MethodCallSyntax attributeConstructorTarget) : base(leadingTrivia, trailingTrivia)
    {
        AttributeData = attributeData;
        AttributeConstructorTarget = attributeConstructorTarget;
    }
}
