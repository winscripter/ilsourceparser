using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>newarr</c> instruction syntax.
/// This instruction is used to create a new array of given type. It also pops one integer
/// from the stack, which specifies the amount of items in the array. Example:
/// <para/>
/// <example>
///   IL:
///   <code>
///     ldc.i4 24
///     newarr [MyAssembly]MyClass
///   </code>
///   C#:
///   <code>
///     MyClass[] myClass = new MyClass[24];
///   </code>
/// </example>
/// </summary>
public class NewarrOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax ArrayType { get; init; }

    internal NewarrOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax arrayType) : base(leadingTrivia, trailingTrivia)
    {
        ArrayType = arrayType;
    }
}
