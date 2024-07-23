namespace ILSourceParser.Common;

/// <summary>
/// Represents the well-known special method name in IL code, typically
/// reserved and/or illegal in C# code but not in IL. These include:
/// <list type="bullet">
///   <item>
///     <c>Finalize</c> - the C# finalizer, like: <c>~MyMethod() { }</c>
///   </item>
///   <item>
///     <c>Operator</c> - not a method, but methods prefixed with <c>op_</c>. Those
///     are methods that specify C# user-defined operators.
///   </item>
///   <item>
///     <c>.ctor</c> - equivalent to a constructor in C#
///   </item>
///   <item>
///     <c>.cctor</c> - equivalent to a static constructor in C#
///   </item>
/// </list>
/// </summary>
public enum KnownSpecialMethodType
{
    /// <summary>
    /// The <c>Finalize()</c> method, typically a finalizer in C# code.
    /// </summary>
    Finalize,

    /// <summary>
    /// Methods starting with <c>op_</c>, typically user-defined operators in
    /// C# code.
    /// </summary>
    Operator,

    /// <summary>
    /// The <c>.ctor</c> method, typically a constructor in C# code.
    /// </summary>
    Ctor,

    /// <summary>
    /// The <c>.cctor</c> method, typically a static constructor in C# code.
    /// </summary>
    Cctor
}
