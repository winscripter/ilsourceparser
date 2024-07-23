using ILSourceParser.Syntax;
using Sprache;

namespace ILSourceParser.Tests;

public class InstructionTests
{
    /// <summary>
    /// A test to validate the IL ldsfld opcode.
    /// </summary>
    [Fact]
    public void OpCodeLdsFldComplex()
    {
        var parser = new Parser();
        var ldsfld = parser.ParseLdsfldInstruction();
        var r = ldsfld.Parse(@"ldsfld class [System.Linq.Expressions]System.Runtime.CompilerServices.CallSite`1<class [System.Runtime]System.Func`3<class [System.Linq.Expressions]System.Runtime.CompilerServices.CallSite, object, int32>> C/'<>o__0'::'<>p__1'");

        var result = (r.FieldType as GenericTypeReferenceSyntax)!;
        Assert.Equal("System.Linq.Expressions", result.AssemblyReference!.AssemblyName);
        Assert.Equal("System.Runtime.CompilerServices.CallSite`1", result.ClassName!);
        Assert.Single(result.GenericArguments!.Parameters);
        Assert.True(result.GenericArguments!.Parameters.All(arg => arg is GenericParameterTypeConstraintSyntax));

        Assert.Equal("C/'<>o__0'",
            (r.Target.DeclaringTypeReference as NonGenericTypeReferenceSyntax)!.GetNameOfType());

        Assert.Equal("<>p__1", r.Target.FieldOrPropertyName);
    }
}
