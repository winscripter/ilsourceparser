using ILSourceParser.Common;
using ILSourceParser.Syntax;
using ILSourceParser.Syntax.Marshaling;
using Sprache;

namespace ILSourceParser.Tests;

public class FieldParsingTests
{
    internal static class Utils
    {
        private static readonly Lazy<Parser> s_parser = new(() => new());

        public static Parser ParserShared => s_parser.Value;
    }

    internal static class Inputs
    {
        public const string SimpleField = ".field public int32 MyField = int32(123)";
        public const string ComplexField =
".field famorassem initonly marshal(custom(\"System.String, System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\", \"Chocolate Chip Cookie!🍪\")) [MyCoolAssembly]MyCoolClass.Unicorn unicorn";
    }

    [Fact]
    public void TestSimpleField()
    {
        var parser = Utils.ParserShared;
        var fieldParser = parser.ParseFieldDeclaration();
        var result = fieldParser.Parse(Inputs.SimpleField);

        Assert.Equal("MyField", result.Name);
        Assert.Equal(AccessModifier.Public, result.AccessModifier);
        Assert.False(result.IsLiteral);
        Assert.False(result.IsStatic);
        Assert.False(result.IsInitOnly);
        Assert.Equal("int32", (result.FieldType as PredefinedTypeSyntax)!.TypeName);
    }

    [Fact]
    public void TestComplexField()
    {
        var parser = Utils.ParserShared;
        var fieldParser = parser.ParseFieldDeclaration();
        var result = fieldParser.Parse(Inputs.ComplexField);

        Assert.Equal("unicorn", result.Name);
        Assert.Equal(AccessModifier.FamilyOrAssembly, result.AccessModifier);
        Assert.False(result.IsLiteral);
        Assert.False(result.IsStatic);
        Assert.True(result.IsInitOnly);
        Assert.NotNull(result.Marshalling);
        Assert.Equal(typeof(CustomMarshalTypeSyntax), result.Marshalling.MarshalType.GetType());

        Assert.Equal("System.String, System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", (result.Marshalling.MarshalType as CustomMarshalTypeSyntax)!.AssemblyString);
        Assert.Equal("Chocolate Chip Cookie!🍪", (result.Marshalling.MarshalType as CustomMarshalTypeSyntax)!.Cookie);

        Assert.Equal("MyCoolAssembly", (result.FieldType as NonGenericTypeReferenceSyntax)!.AssemblyReference!.AssemblyName);
    }
}
