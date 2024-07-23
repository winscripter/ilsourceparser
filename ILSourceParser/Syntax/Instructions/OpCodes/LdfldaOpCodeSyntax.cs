using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>ldflda</c> instruction.
/// This instruction is used to load the address of the given field onto the stack. Example:
/// <para/>
/// <example>
///   IL:
///   <code>
/// .class public C
/// {
///     .field initonly string MyString = "abc"
///     
///     .method public void X()
///     {
///         // create new instance of C 'C c = new C()'
///         newobj instance void C::.ctor()
///         
///         // 'ref string reference = ref c.MyString'
///         ldflda string C::MyString
///         
///         ret
///     }
/// }
///   </code>
///   C#:
///   <code>
/// public class C
/// {
///     public string MyString = "abc"; // It is not recommended to keep fields public like this but this is just a demo after all
///     
///     public void X()
///     {
///         C c = new C();
///         ref string reference = ref c.MyString;
///     }
/// }
///   </code>
/// </example>
/// </summary>
public class LdfldaOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax FieldType { get; init; }
    public FieldOrPropertyReferenceSyntax Target { get; init; }

    internal LdfldaOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax fieldType,
        FieldOrPropertyReferenceSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        FieldType = fieldType;
        Target = target;
    }
}
