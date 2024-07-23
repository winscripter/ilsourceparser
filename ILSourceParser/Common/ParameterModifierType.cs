using System.Runtime.InteropServices;

namespace ILSourceParser.Common;

/// <summary>
/// Represents the type of parameter modifier.
/// </summary>
public enum ParameterModifierType
{
    /// <summary>
    /// Equivalent to <see cref="OutAttribute"/>.
    /// </summary>
    Out = 0,

    /// <summary>
    /// Equivalent to <see cref="InAttribute"/>.
    /// </summary>
    In = 1,

    /// <summary>
    /// Equivalent to <see cref="OptionalAttribute"/>.
    /// </summary>
    Opt = 2,

    /// <summary>
    /// Unknown parameter modifier was specified.
    /// </summary>
    Unknown = 3,
}
