using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions.OpCodes;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) <c>castclass</c> instruction syntax.
/// This instruction is used to cast an instance of an object to another type. Casting is
/// guaranteed to complete at compile time, but may not complete at runtime type.
/// </summary>
/// <remarks>
///   IL:
///   <code>
/// // public class A { }
/// .class public A
/// {
/// }
/// 
/// // public class B { }
/// .class public B
/// {
/// }
/// 
/// .class public sealed Program
/// {
///     .method public static void Main()
///     {
///         .entrypoint
///         .locals init (
///             [0] A 'My A Variable'
///         )
/// 
///         // A a = new A();
///         newobj instance void A::.ctor()
///         stloc.0
///         ldloc.0
///         
///         // B b = (B)(object)a;
///         castclass B // Convert A to B
///         pop
///         ret
///     }
/// }
///   </code>
/// </remarks>
public class CastclassOpCodeSyntax : OpCodeSyntax
{
    public TypeSyntax Target { get; init; }

    internal CastclassOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        TypeSyntax target) : base(leadingTrivia, trailingTrivia)
    {
        Target = target;
    }
}
