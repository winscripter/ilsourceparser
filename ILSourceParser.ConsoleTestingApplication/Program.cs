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
