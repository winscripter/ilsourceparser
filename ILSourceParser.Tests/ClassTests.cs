using ILSourceParser.Syntax;
using Sprache;

namespace ILSourceParser.Tests;

public class ClassTests
{
    [Fact]
    public void Simple()
    {
        const string input = @".class public static auto ansi beforefieldinit Program {
    .field private static initonly string s_myString

    .method private static void .cctor() {
        ldstr ""Hello, yet again!""
        stsfld string Program::s_myString
        ret
    }

    .method public static void Main(string[] args) {
        .entrypoint
        .maxstack 8

        ldstr ""Hello, World!""
        call void [System.Console]System.Console::WriteLine(string)

        call void Program::DoPrintSomething()        

        ret
    }

    .method public static void DoPrintSomething() {
        ldsfld string Program::s_myString
        call void [System.Console]System.Console::WriteLine(string)
        ret
    }
}";
        var parser = new Parser();
        var classParser = parser.ParseClassDeclaration();
        var result = classParser.Parse(input);

        Assert.Equal(4, result.DescendantNodes.Count());
        Assert.True(result.DescendantNodes.ElementAt(0) is FieldDeclarationSyntax);
        Assert.True(result.DescendantNodes.ElementAt(1) is MethodDeclarationSyntax);
        Assert.True(result.DescendantNodes.ElementAt(2) is MethodDeclarationSyntax);
        Assert.True(result.DescendantNodes.ElementAt(3) is MethodDeclarationSyntax);
    }
}
