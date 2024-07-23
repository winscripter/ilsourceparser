using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>isinst</c> instruction. This instruction
/// is used to test whether the last object on the evaluation stack is of given type or
/// not. It pushes a boolean on stack. Example:
/// <example>
///   IL:
///   <code>
///     newobj instance void [System.Private.CoreLib]System.Random::.ctor()
///     isinst [System.Private.CoreLib]System.Text.StringBuilder
///     // Pushes 'false' because last item on the stack is of type Random, not StringBuilder
///   </code>
///   C# equivalent:
///   <code>
///     using System;
///     using System.Text;
///     
///     var random = new Random();
///     var flag = random is StringBuilder; // this is isinst instruction here
///   </code>
/// </example>
/// </summary>
public class IsinstOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Target { get; init; }

    internal IsinstOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
