namespace ILSourceParser.Common;

/// <summary>
/// Represents the type of calling convention in IL code.
/// </summary>
public enum CallKind
{
    /// <summary>
    /// Cdecl calling convention.
    /// </summary>
    Cdecl,

    /// <summary>
    /// Stdcall calling convention.
    /// </summary>
    StdCall,

    /// <summary>
    /// Thiscall calling convention.
    /// </summary>
    ThisCall,

    /// <summary>
    /// Fastcall calling convention.
    /// </summary>
    FastCall,

    /// <summary>
    /// Default calling convention - this is not a calling convention, rather,
    /// the default calling convention is used based on the application.
    /// </summary>
    Default
}
