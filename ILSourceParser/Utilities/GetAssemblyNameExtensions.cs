using ILSourceParser.Syntax;

namespace ILSourceParser.Utilities;

/// <summary>
/// A bunch of extension methods that retrieve names of assemblies from
/// given syntax nodes.
/// </summary>
public static class GetAssemblyNameExtensions
{
    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="attribute">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this AnonymousCustomAttributeSyntax attribute) =>
        attribute.AttributeConstructorTarget.GetAssemblyName();

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="customAttribute">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this CustomAttributeSyntax customAttribute) =>
        customAttribute.AttributeConstructorTarget.GetAssemblyName();

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="reference">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this NonGenericTypeReferenceSyntax reference) =>
        reference.AssemblyReference?.AssemblyName;

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="reference">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this GenericTypeReferenceSyntax reference) =>
        reference.AssemblyReference?.AssemblyName;

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="typeReference">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this TypeReferenceSyntax typeReference) =>
        typeReference is NonGenericTypeReferenceSyntax nonGeneric
        ? nonGeneric.AssemblyReference?.AssemblyName
        : typeReference is GenericTypeReferenceSyntax generic
          ? generic.AssemblyReference?.AssemblyName
          : null;

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="type">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this TypeSyntax type) =>
        type is TypeReferenceSyntax typeReference ? typeReference.GetAssemblyName() : null;

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="invocation">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this MethodInvocationSyntax invocation) =>
        invocation.TypeReference.GetAssemblyName();

    /// <summary>
    /// Returns the explicit name of the assembly from this syntax node. For example,
    /// if the type reference is prefixed with <c>[System.Private.CoreLib]</c>, this
    /// method will return <c>System.Private.CoreLib</c> as a string. If assembly reference
    /// is omitted, this method returns <see langword="NULL"/>.
    /// </summary>
    /// <param name="call">The syntax node to get assembly name from.</param>
    /// <returns>
    /// If assembly name is explicitly emitted, returns the name of the assembly. Otherwise, returns <see langword="NULL"/>
    /// </returns>
    public static string? GetAssemblyName(this MethodCallSyntax call) =>
        call.MethodInvocation.GetAssemblyName();
}
