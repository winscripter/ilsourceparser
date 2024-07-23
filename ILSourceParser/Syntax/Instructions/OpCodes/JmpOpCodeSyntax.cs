using ILSourceParser.Trivia;
using System;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>jmp</c> instruction syntax.
/// This instruction is used to transfer control to a different method. Each method
/// can use this instruction just once because any code after the instruction is unreachable
/// since control is transferred to a different method. Example:
/// <para/>
/// <example>
///   IL:
///   <code>
/// .method public hidebysig static int32 M() cil managed
/// {
///     .entrypoint
///     jmp void C::X()
///     // Code down here is unreachable
/// }
/// 
/// .method private static int32 X()
/// {
///    ldc.i4 42
///    ret
/// }
///   </code>
///   C#:
///   <code>
/// public static int M()
/// {
///     return X();
/// }
/// 
/// private static int X()
/// {
///     return 42;
/// }
///   </code>
/// </example>
/// </summary>
public class JmpOpCodeSyntax : OpCodeSyntax
{
    public MethodCallSyntax Method { get; init; }

    internal JmpOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        MethodCallSyntax method) : base(leadingTrivia, trailingTrivia)
    {
        Method = method;
    }
}
