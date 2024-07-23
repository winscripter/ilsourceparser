namespace ILSourceParser.Common;

/// <summary>
/// Represents the type of an access modifier in IL code.
/// </summary>
public enum AccessModifier
{
    /// <summary>
    /// No access modifier is specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Public - equivalent to <see langword="public"/> in C#.
    /// </summary>
    Public = 1,

    /// <summary>
    /// Family - equivalent to <see langword="protected"/> in C#.
    /// </summary>
    Family = 2,

    /// <summary>
    /// Private - equivalent to <see langword="private"/> in C#.
    /// </summary>
    Private = 3,

    /// <summary>
    /// Assembly - equivalent to <see langword="internal"/> in C#.
    /// </summary>
    Assembly = 4,

    /// <summary>
    /// FamOrAssem - equivalent to <see langword="protected private"/> in C#.
    /// </summary>
    FamilyOrAssembly = 5,

    /// <summary>
    /// FamAndAssem - equivalent to <see langword="protected internal"/> in C#.
    /// </summary>
    FamilyAndAssembly = 6,
}
