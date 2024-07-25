using ILSourceParser.Trivia;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL <c>pinvokeimpl()</c> function, typically declared in
/// method flags before the method return type and name.
/// </summary>
public class PInvokeImplSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }

    /// <summary>
    /// Represents the DLL name. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be "User32.dll".
    /// </summary>
    public string DllName { get; init; }

    /// <summary>
    /// Represents the entry point of the DLL. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be "SampleEntryPoint".
    /// </summary>
    public PInvokeEntryPointSyntax? EntryPoint { get; init; }

    /// <summary>
    /// Represents the character set. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be <see cref="CharSet.Unicode"/> due to the <c>unicode</c> flag specified in the <c>pinvokeimpl</c> function.
    /// </summary>
    /// <remarks>
    ///   <b>Remark</b>: If the character set is not specified, the value of this property
    ///   defaults to <see cref="CharSet.None"/>.
    /// </remarks>
    public CharSet CharSet { get; init; }

    /// <summary>
    /// Represents the character set as a string. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be "unicode".
    /// </summary>
    /// <remarks>
    ///   <b>Remark</b>: To get <see cref="System.Runtime.InteropServices.CharSet"/> based
    ///   on the character set passed to the pinvokeimpl function, use the <see cref="CharSet"/>
    ///   property instead.
    /// </remarks>
    public string? RawCharSet { get; init; }

    /// <summary>
    /// Specifies whether the last error should be set. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be <see langword="TRUE" />. If we omit the
    /// <c>lasterr</c> flag in the pinvokeimpl function, the value of this property will
    /// be <see langword="FALSE"/>.
    /// </summary>
    public bool SetLastError { get; init; }

    /// <summary>
    /// Specifies the calling convention. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be <see cref="CallingConvention.Cdecl"/> because
    /// of the <c>cdecl</c> flag in the pinvokeimpl() function.
    /// </summary>
    public CallingConvention? CallingConvention { get; init; }

    /// <summary>
    /// Represents the calling convention as a string. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be "cdecl".
    /// </summary>
    /// <remarks>
    ///   <b>Remark</b>: To get <see cref="System.Runtime.InteropServices.CallingConvention"/> based
    ///   on the calling convention passed to the pinvokeimpl function, use the
    ///   <see cref="CallingConvention"/> property instead.
    /// </remarks>
    public string? RawCallingConvention { get; init; }

    /// <summary>
    /// Specifies whether exact spelling is specified. In this example:
    /// <example>
    ///   <code>
    ///     pinvokeimpl("User32.dll" as "SampleEntryPoint" nomangle lasterr unicode cdecl)
    ///   </code>
    /// </example>
    /// .. the value of this property will be <see langword="TRUE"/> because of the <c>nomangle</c>
    /// flag. If we omit this flag, the value of this property will be <see langword="FALSE"/>.
    /// </summary>
    public bool ExactSpelling { get; init; }

    internal PInvokeImplSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string dllName,
        PInvokeEntryPointSyntax? entryPoint,
        CharSet charSet,
        string? rawCharSet,
        bool setLastError,
        CallingConvention? callingConvention,
        string? rawCallingConvention,
        bool exactSpelling)
    {
        LeadingTrivia = leadingTrivia;
        TrailingTrivia = trailingTrivia;
        DllName = dllName;
        EntryPoint = entryPoint;
        CharSet = charSet;
        RawCharSet = rawCharSet;
        SetLastError = setLastError;
        CallingConvention = callingConvention;
        RawCallingConvention = rawCallingConvention;
        ExactSpelling = exactSpelling;
    }
}
