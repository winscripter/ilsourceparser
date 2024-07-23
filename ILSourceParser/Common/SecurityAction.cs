namespace ILSourceParser.Common;

/// <summary>
/// Represents a security action- that is, what is used in the <c>.permissionset</c> assembler directive.
/// These are part of Code Access Security (CAS) feature. By default, this assembler directive is
/// only emitted by the compiler in .NET Framework apps - .NET Core and later do.
/// </summary>
/// <remarks>
///   <b>Remark: </b> please note that the security actions are now deprecated in modern .NET versions and should not be used anymore.
///   However, they're still ordinarily seen even in modern .NET Framework code, even though
///   Code Access Security was deprecated way back in 2010. However, by compiling the IL
///   application with a more modern .NET, you will likely not see the <c>.permissionset</c>
///   assembler directive in the decompiled code at all.
/// </remarks>
public enum SecurityAction
{
    /// <summary>
    /// <c>request</c>: Specifies that the assembly requests the specified permissions.
    /// The CLR checks if the requested permissions are granted.
    /// </summary>
    Request,

    /// <summary>
    /// <c>demand</c>:  Requires that the assembly has the specified permissions.
    /// If not, a security exception is thrown.
    /// </summary>
    Demand,

    /// <summary>
    /// <c>assert</c>: Allows the assembly to assert that it has the specified permissions, even if it doesn’t.
    /// Use with caution before running the application with this security action.
    /// </summary>
    Assert,

    /// <summary>
    /// <c>deny</c>: Explicitly denies the specified permissions to the assembly.
    /// </summary>
    Deny,

    /// <summary>
    /// <c>permitonly</c>: Grants only the specified permissions to the assembly, denying all others.
    /// </summary>
    PermitOnly,

    /// <summary>
    /// <c>linkcheck</c>: Used during verification to ensure that the assembly doesn’t link against forbidden types or methods.
    /// </summary>
    LinkCheck,

    /// <summary>
    /// <c>inheritcheck</c>: Similar to linkcheck but also checks inherited members.
    /// </summary>
    InheritCheck,

    /// <summary>
    /// <c>reqopt</c>: Specifies optional permissions that the assembly requests.
    /// </summary>
    ReqOpt,

    /// <summary>
    /// <c>reqrefuse</c>: Specifies permissions that the assembly refuses.
    /// </summary>
    ReqRefuse,

    /// <summary>
    /// <c>prejitgrant</c>: Specifies permissions granted during pre-JIT compilation.
    /// </summary>
    PreJitGrant,

    /// <summary>
    /// <c>prejitdeny</c>: Specifies permissions denied during pre-JIT compilation.
    /// </summary>
    PreJitDeny,

    /// <summary>
    /// <c>noncasdemand</c>: Used for non-CAS (Code Access Security) demands.
    /// </summary>
    NonCasDemand,

    /// <summary>
    /// <c>noncaslinkdemand</c>: Used for non-CAS link demands.
    /// </summary>
    NonCasLinkDemand,

    /// <summary>
    /// <c>noncasinheritance</c>: Used for non-CAS inheritance demands
    /// </summary>
    NonCasInheritance,

    /// <summary>
    /// <c>reqmin</c>: Set specifies the minimum permissions required for the assembly to execute.
    /// If one of the permissions is not met, a security exception is thrown. This is the default
    /// for .NET Framework apps, but is no longer used in .NET.
    /// </summary>
    ReqMin,
}
