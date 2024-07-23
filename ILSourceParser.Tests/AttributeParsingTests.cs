using ILSourceParser.Syntax;
using ILSourceParser.Utilities;
using Sprache;

namespace ILSourceParser.Tests;

public class AttributeParsingTests
{
    [Fact]
    public void TestCustomAttributeSimple()
    {
        var parser = new Parser();
        // .ParseCustomAttribute will choose either anonymous or custom
        // .CustomAttributeWithData enforces custom
        var attributeParser = parser.ParseCustomAttributeWithData();
        var parseResult = attributeParser.Parse(@".custom instance void [System.Private.CoreLib]System.STAThreadAttribute::.ctor() = (
    01 00 00 00
)");
        byte[] expectedAttributeData = [0x01, 0x00, 0x00, 0x00];

        Assert.Equal("System.Private.CoreLib", parseResult.GetAssemblyName());
        Assert.Equal("System.STAThreadAttribute", (parseResult.AttributeConstructorTarget.MethodInvocation.TypeReference as NonGenericTypeReferenceSyntax)!.ClassName);
        Assert.Equal(expectedAttributeData, parseResult.GetRawBytes());
    }

    [Fact]
    public void TestAnonymousCustomAttribute()
    {
        var parser = new Parser();
        var attributeParser = parser.ParseAnonymousCustomAttribute();
        var parseResult = attributeParser.Parse(@".custom instance void [System.Private.CoreLib]System.STAThreadAttribute::.ctor()");
        Assert.Equal("System.Private.CoreLib", parseResult.GetAssemblyName());
        Assert.Equal("System.STAThreadAttribute", (parseResult.AttributeConstructorTarget.MethodInvocation.TypeReference as NonGenericTypeReferenceSyntax)!.ClassName);
        Assert.Equal([], parseResult.GetRawBytes());
    }

    [Fact]
    public void TestParserAttributeDecisions()
    {
        var parser = new Parser();
        string input1 = @".custom instance void [System.Private.CoreLib]System.STAThreadAttribute::.ctor() = (
    01 00 00 00
)";
        string input2 = @".custom instance void [System.Private.CoreLib]System.STAThreadAttribute::.ctor()";

        // .ParseCustomAttribute() automatically returns an anonymous or custom
        // attribute based on the input. .ParseCustomAttributeWithData() and
        // .ParseAnonymousCustomAttribute() enforce to parse custom and anonymous
        // attribute, respectively.
        var customAttributeParser = parser.ParseCustomAttribute();

        Assert.True(customAttributeParser.Parse(input1) is CustomAttributeSyntax);
        Assert.True(customAttributeParser.Parse(input2) is AnonymousCustomAttributeSyntax);
    }
}
