using ILSourceParser.Trivia;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ILSourceParser.Syntax;

public class PInvokeImplSyntax : SyntaxNode
{
    /// <inheritdoc cref="SyntaxNode.LeadingTrivia" />
    public override ImmutableArray<SyntaxTrivia> LeadingTrivia { get; init; }
    /// <inheritdoc cref="SyntaxNode.TrailingTrivia" />
    public override ImmutableArray<SyntaxTrivia> TrailingTrivia { get; init; }
    public string DllName { get; init; }
    public PInvokeEntryPointSyntax? EntryPoint { get; init; }
    public CharSet CharSet { get; init; }
    public string? RawCharSet { get; init; }
    public bool SetLastError { get; init; }
    public CallingConvention? CallingConvention { get; init; }
    public string? RawCallingConvention { get; init; }
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
