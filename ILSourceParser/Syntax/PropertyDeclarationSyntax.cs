using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents a declaration of a property, e.g. the IL <c>.property</c> directive.
/// </summary>
public class PropertyDeclarationSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// The type of the property, for example, a string or int32.
    /// </summary>
    public TypeSyntax ReturnType { get; init; }

    /// <summary>
    /// Is this property static? For example, has the <c>instance</c> keyword:
    /// <code>
    ///   //        vvvvvvvv &lt;--------- HERE
    ///   .property instance string MyProperty()
    ///   {
    ///   }
    /// </code>
    /// </summary>
    public bool IsReturnTypeInstance { get; init; }

    /// <summary>
    /// Represents the name of the property.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Specifies the information about the <c>.get</c> accessor defined in this property. If
    /// this accessor is omitted, the value of this <see cref="GetAccessor"/> property is
    /// <see langword="null"/>.
    /// </summary>
    public GetAccessorSyntax? GetAccessor { get; init; }

    /// <summary>
    /// Specifies the information about the <c>.set</c> accessor defined in this property. If
    /// this accessor is omitted, the value of this <see cref="SetAccessor"/> property is
    /// <see langword="null"/>.
    /// </summary>
    public SetAccessorSyntax? SetAccessor { get; init; }

    /// <summary>
    /// Represents custom attributes defined in this property. In this example:
    /// <example>
    ///   <code>
    ///     .property instance string MyProperty()
    ///     {
    ///         .custom instance void [MyAssembly]MyNamespace.MyAttribute::.ctor() = (
    ///            01 00 00 00
    ///         )
    ///         .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor() = (
    ///            01 00 00 00
    ///         )
    ///         .get instance string get_MyProperty()
    ///         .set instance void set_MyProperty(string)
    ///     }
    ///   </code>
    /// </example>
    /// .. the first two <c>.custom</c> directives inside the property will be defined
    /// in this <see cref="CustomAttributes"/> property.
    /// </summary>
    public IEnumerable<BaseCustomAttributeSyntax> CustomAttributes { get; init; }

    internal PropertyDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax returnType,
        bool isReturnTypeInstance,
        string name,
        GetAccessorSyntax? getAccessor,
        SetAccessorSyntax? setAccessor,
        IEnumerable<BaseCustomAttributeSyntax> customAttributes)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        ReturnType = returnType;
        IsReturnTypeInstance = isReturnTypeInstance;
        Name = name;
        GetAccessor = getAccessor;
        SetAccessor = setAccessor;
        CustomAttributes = customAttributes;
    }
}
