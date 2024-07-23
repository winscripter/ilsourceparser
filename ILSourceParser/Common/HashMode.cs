namespace ILSourceParser.Common;

/// <summary>
/// Represents the hashing mode for the <c>.hash</c> directive.
/// </summary>
public enum HashMode
{
    /// <summary>
    /// The SHA1 algorithm is the default algorithm for .NET Framework apps but is deemed insecure since 2015. Value is 0x00008004.
    /// </summary>
    SHA1 = 0x00008004,

    /// <summary>
    /// The SHA256 algorithm is the default algorithm for .NET Core 1.0 and later apps. Value is 0x0000800C.
    /// </summary>
    SHA256 = 0x0000800C,

    /// <summary>
    /// The MD5 algorithm is considered insecure and is not default for any .NET runtime type. Value is 0x00008003.
    /// </summary>
    MD5 = 0x00008003,

    /// <summary>
    /// The SHA512 algorithm is not default for any .NET runtime type but is considered the most secure option. Value is 0x0000800D.
    /// </summary>
    /// <remarks>
    ///   <b>Remark: </b> using this algorithm in a .NET application could severely drop down performance.
    /// </remarks>
    SHA512 = 0x0000800D,

    /// <summary>
    /// A hashing algorithm that is not recognized. Value is 0x00000000.
    /// </summary>
    /// <remarks>
    ///   <b>Remark: </b> if the algorithm is unknown,
    ///   the application still compiles and runs fine. This is because .NET will
    ///   silently fall back to the default hashing algorithm if the current one
    ///   is unknown. So, on .NET Framework, an unknown hashing algorithm will
    ///   instead result in SHA1, while on .NET Core 1.0 and later, the hashing
    ///   algorithm will be SHA256.
    /// </remarks>
    Unknown = 0x00000000,
}
