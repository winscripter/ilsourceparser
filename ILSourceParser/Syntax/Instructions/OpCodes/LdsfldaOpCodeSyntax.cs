using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>ldsflda</c> instruction.
/// This instruction is used to load the address of the given static field onto the stack. Example:
/// <para/>
/// <example>
///   IL:
///   <code>
/// .class public C
/// {
///     .field static initonly string MyString = "abc"
///     
///     .method public void X()
///     {
///         // 'ref string reference = ref c.MyString'
///         ldsflda string C::MyString
///         
///         ret
///     }
/// }
///   </code>
///   C#:
///   <code>
/// public class C
/// {
///     public static string MyString = "abc"; // It is not recommended to keep fields public like this but this is just a demo after all
///     
///     public void X()
///     {
///         ref string reference = MyString;
///     }
/// }
///   </code>
/// </example>
/// </summary>
public class LdsfldaOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax FieldType { get; init; }
    public FieldOrPropertyReferenceSyntax Target { get; init; }

    internal LdsfldaOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax fieldType,
        FieldOrPropertyReferenceSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        FieldType = fieldType;
        Target = target;
    }
}
