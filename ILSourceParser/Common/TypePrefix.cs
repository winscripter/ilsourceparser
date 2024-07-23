namespace ILSourceParser.Common;

/// <summary>
/// Represents the type prefix. For example, in this IL code:
/// <code>
///   [MyAssembly]MyNamespace.MyGenericClass&lt;valuetype [System.Runtime]System.Decimal&gt;
/// </code>
/// The first generic parameter of <c>MyNamespace.MyGenericClass</c> is a non-generic
/// type reference, prefixed with <c>valuetype</c>. Since <c>[System.Runtime]System.Decimal</c>
/// is prefixed with <c>valuetype</c>, this will use <see cref="ValueType"/>.
/// </summary>
public enum TypePrefix
{
    /// <summary>
    /// Type prefix <c>class</c>
    /// </summary>
    Class,

    /// <summary>
    /// Type prefix <c>valuetype</c>
    /// </summary>
    ValueType
}
