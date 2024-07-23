using ILSourceParser.Common;
using ILSourceParser.Syntax.Marshaling;
using Sprache;

namespace ILSourceParser.Tests;

public class ParameterTests
{
    [Fact]
    public void TestWithoutMarshalling()
    {
        var parser = new Parser();
        var parameterParser = parser.ParseParameter();
        var result = parameterParser.Parse("[out] string* myString");

        Assert.True(result.Modifiers.First().ModifierType == ParameterModifierType.Out);
        Assert.Equal("myString", result.Name);
    }

    [Fact]
    public void TestWithMarshalling()
    {
        var parser = new Parser();
        var parameterParser = parser.ParseParameter();
        var result = parameterParser.Parse("[out] string* marshal(lpwstr) myString");

        Assert.True(result.Modifiers.First().ModifierType == ParameterModifierType.Out);
        Assert.Equal("myString", result.Name);

        Assert.NotNull(result.Marshalling);
        Assert.Equal("lpwstr", (result.Marshalling!.MarshalType as SimpleMarshalTypeSyntax)!.Value);
    }
}
