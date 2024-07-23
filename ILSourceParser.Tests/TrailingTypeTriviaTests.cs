using ILSourceParser.Common;
using ILSourceParser.Syntax;
using ILSourceParser.Trivia;
using Sprache;

namespace ILSourceParser.Tests;

public class TrailingTypeTriviaTests
{
    [Fact]
    public void TestNonGenericType()
    {
        var parser = new Parser();
        var nonGenericParser = parser.ParseNonGenericTypeReference();

        var result = nonGenericParser.Parse("[Unicorn]MyCoolClass.MyUnicorn&[]**");
        Assert.Equal("MyCoolClass.MyUnicorn", result.ClassName);
        Assert.Equal(4, result.TrailingTrivia.Length);
        Assert.True(
            result.TrailingTrivia[0] is TypeAmpersandTrivia &&
            result.TrailingTrivia[1] is TypeArrayTrivia &&
            result.TrailingTrivia[2] is TypeAsteriskTrivia &&
            result.TrailingTrivia[3] is TypeAsteriskTrivia);
    }

    [Fact]
    public void TestGenericType()
    {
        var parser = new Parser();
        var genericParser = parser.ParseGenericTypeReference();

        var result = genericParser.Parse("[Unicorn]MyCoolClass.MyGenericUnicorn<int32, !!T>*[]*");

        Assert.Equal("MyCoolClass.MyGenericUnicorn", result.ClassName);
        Assert.Equal(3, result.TrailingTrivia.Length);
        Assert.True(
            result.TrailingTrivia[0] is TypeAsteriskTrivia &&
            result.TrailingTrivia[1] is TypeArrayTrivia &&
            result.TrailingTrivia[2] is TypeAsteriskTrivia);

        Assert.True(IsFirstParameterInt32(result));
        Assert.Equal("T", GetSecondParameter(result));
    }

    private static bool IsFirstParameterInt32(GenericTypeReferenceSyntax? input) =>
        input!.GenericArguments!.Parameters.ElementAt(0) is GenericParameterPrimitiveSyntax
        {
            UnderlyingType.Kind: PredefinedTypeKind.Int32
        };

    private static string GetSecondParameter(GenericTypeReferenceSyntax? input) =>
        (input!.GenericArguments!.Parameters.ElementAt(1) as GenericParameterReferenceSyntax)?
        .RawParameterReference!;
}
