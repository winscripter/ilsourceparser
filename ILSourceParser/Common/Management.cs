namespace ILSourceParser.Common;

/// <summary>
/// Specifies the management type of a function pointer.
/// </summary>
public enum Management
{
    /// <summary>
    /// The function pointer is a pointer referring to the managed method.
    /// </summary>
    Managed,

    /// <summary>
    /// The function pointer is a pointer referring to the unmanaged method.
    /// </summary>
    Unmanaged
}
