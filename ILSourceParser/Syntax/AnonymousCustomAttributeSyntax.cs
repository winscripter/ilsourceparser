using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an anonymous custom attribute in IL code. This is a type of
/// custom attribute that does not contain any byte data. Example:
/// <code>
///   .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor()
///   
///   // There is no "= ( 01 00 00 00 )" after "::.ctor()", so this is an anonymous
///   // custom attribute.
/// </code>
/// There is a different syntax node to represent custom attributes that contain
/// byte data, which is <see cref="CustomAttributeSyntax"/>.
/// </summary>
public class AnonymousCustomAttributeSyntax : BaseCustomAttributeSyntax
{
    /// <summary>
    /// Represents the invocation to the constructor. For example, in this
    /// custom attribute:
    /// <code>
    ///   .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor()
    /// </code>
    /// .. this property will hold a reference to <c>[System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor()</c>,
    /// specifically the constructor method <c>.ctor()</c>, and the
    /// return type <c>instance void</c>.
    /// </summary>
    public MethodCallSyntax AttributeConstructorTarget { get; init; }

    internal AnonymousCustomAttributeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax attributeConstructorTarget) : base(leadingTrivia, trailingTrivia)
    {
        AttributeConstructorTarget = attributeConstructorTarget;
    }
}
