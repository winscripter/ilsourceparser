# ILSourceParser
This library for .NET provides functionality to parse IL source code. If you're not aware, Microsoft Intermediate Language (IL or CIL, formerly known as MSIL) is
what the .NET application typically consists of. C#, Visual Basic, F#, and other well-known official .NET languages compile to IL bytecode. However,
IL actually has its syntax as well. ILSourceParser parses the IL syntax into syntax nodes, making it easier to analyze IL.

This library does not rely on any existing project and is not a port from another language, so chances are there could be some issues with this library. If
you encounter one, please create a new issue post about it!

The known issue is that ILSourceParser might not be the fastest thing in the world, because it took nearly 1 second to analyze a 200-line of code IL file. Maybe
it just needs some additional JIT warming, although there could be a different reason - parsing IL is generally harder than parsing C#, mostly because IL has almost 300
instructions, and we have to do over 300 checks when parsing method body to parse one instruction.

### Example
```cs
var syntaxTree = ILSyntaxTree.ParseText(@".assembly MyAssembly { }");
var root = syntaxTree.GetRoot();
foreach (SyntaxNode node in root.DescendantNodes)
{
    // do something with node
}
```
ILSourceParser is generally really similar to [Roslyn](https://github.com/dotnet/roslyn), even though it doesn't rely on it at all.

A more complex example here:
```cs
using ILSourceParser;
using ILSourceParser.Syntax;
using ILSourceParser.Syntax.Instructions;
using Sprache;

string input = @".assembly MyAssembly
{
    .ver 1:2:3:4
}
.assembly extern System.Runtime
{
}
.assembly extern System.Console
{
}

// My Cool Application !

.imagebase 0x00400000
.line 123

.class public sequential auto ansi beforefieldinit MyValueType extends [System.Runtime]System.ValueType
{
    .field private static initonly string s_myString

    .method public static hidebysig void .cctor() cil managed
    {
        ldstr ""Hello, World!""
        stsfld string MyValueType::s_myString
    }
    
    .method public static void PrintText() cil managed
    {
        .maxstack 8
        
        ldsfld string MyValueType::s_myString
        call void [System.Console]System.Console::WriteLine(string)
        
        ret
    }
}";
var syntaxRoot = ILSyntaxTree.ParseText(input).GetRoot();

foreach (var descendant in syntaxRoot.DescendantNodes)
{
    switch (descendant)
    {
        case AssemblyDeclarationSyntax asm:
            Console.WriteLine($"Assembly {asm.AssemblyName}, is external: {asm.IsExtern}");
            break;
        case BaseCommentSyntax comment:
            if (comment is InlineCommentSyntax inline)
            {
                Console.WriteLine($"Inline comment, text: {inline.CommentText}");
            }
            else if (comment is MultilineCommentSyntax multiline)
            {
                Console.WriteLine($"Multiline comment, text: {multiline.CommentText}");
            }
            break;
        case ImageBaseDirectiveSyntax imageBase:
            Console.WriteLine($"Image base {imageBase.ImageBase}");
            break;
        case LineDirectiveSyntax line:
            Console.WriteLine($"Line {line.Line}");
            break;
        case ClassDeclarationSyntax @class:
            string flags = string.Join(", ", @class.Flags);
            Console.WriteLine($"Class named {@class.Name}; flags: {flags}");
            break;
    }
}
```
