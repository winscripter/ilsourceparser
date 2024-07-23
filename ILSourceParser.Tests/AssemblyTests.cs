using ILSourceParser.Common;
using ILSourceParser.Syntax;
using ILSourceParser.Utilities;
using Sprache;

namespace ILSourceParser.Tests;

public class AssemblyTests
{
    private static readonly byte[] s_complexCustomAttributePermissionSetDescendantByteData =
    [
        0x2e, 0x01, 0x80, 0x8a, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x53, 0x65, 0x63,
        0x75, 0x72, 0x69, 0x74, 0x79, 0x2e, 0x50, 0x65, 0x72, 0x6d, 0x69, 0x73, 0x73, 0x69,
        0x6f, 0x6e, 0x73, 0x2e, 0x53, 0x65, 0x63, 0x75, 0x72, 0x69, 0x74, 0x79, 0x50, 0x65,
        0x72, 0x6d, 0x69, 0x73, 0x73, 0x69, 0x6f, 0x6e, 0x41, 0x74, 0x74, 0x72, 0x69, 0x62,
        0x75, 0x74, 0x65, 0x2c, 0x20, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x2e, 0x52, 0x75,
        0x6e, 0x74, 0x69, 0x6d, 0x65, 0x2c, 0x20, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6f, 0x6e,
        0x3d, 0x38, 0x2e, 0x30, 0x2e, 0x30, 0x2e, 0x30, 0x2c, 0x20, 0x43, 0x75, 0x6c, 0x74,
        0x75, 0x72, 0x65, 0x3d, 0x6e, 0x65, 0x75, 0x74, 0x72, 0x61, 0x6c, 0x2c, 0x20, 0x50,
        0x75, 0x62, 0x6c, 0x69, 0x63, 0x4b, 0x65, 0x79, 0x54, 0x6f, 0x6b, 0x65, 0x6e, 0x3d,
        0x62, 0x30, 0x33, 0x66, 0x35, 0x66, 0x37, 0x66, 0x31, 0x31, 0x64, 0x35, 0x30, 0x61,
        0x33, 0x61, 0x15, 0x01, 0x54, 0x02, 0x10, 0x53, 0x6b, 0x69, 0x70, 0x56, 0x65, 0x72,
        0x69, 0x66, 0x69, 0x63, 0x61, 0x74, 0x69, 0x6f, 0x6e, 0x01
    ];

    [Fact]
    public void Simple()
    {
        var parser = new Parser();
        var asmParser = parser.ParseAssemblyDeclaration();
        var result = asmParser.Parse(@".assembly extern System.Private.CoreLib
{
    .line 123
}");

        Assert.True(result.IsExtern);
        Assert.Equal("System.Private.CoreLib", result.AssemblyName);
        Assert.Single(result.DescendantNodes);

        Assert.True(result.DescendantNodes.ElementAt(0) is LineDirectiveSyntax);
    }

    [Fact]
    public void Complex()
    {
        var parser = new Parser();
        var asmParser = parser.ParseAssemblyDeclaration();
        var result = asmParser.Parse(@".assembly _
{
    .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = (
        01 00 08 00 00 00 00 00
    )
    .custom instance void [System.Runtime]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = (
        01 00 01 00 54 02 16 57 72 61 70 4e 6f 6e 45 78
        63 65 70 74 69 6f 6e 54 68 72 6f 77 73 01
    )
    .custom instance void [System.Runtime]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [System.Runtime]System.Diagnostics.DebuggableAttribute/DebuggingModes) = (
        01 00 02 00 00 00 00 00
    )
    .permissionset reqmin = (
        2e 01 80 8a 53 79 73 74 65 6d 2e 53 65 63 75 72
        69 74 79 2e 50 65 72 6d 69 73 73 69 6f 6e 73 2e
        53 65 63 75 72 69 74 79 50 65 72 6d 69 73 73 69
        6f 6e 41 74 74 72 69 62 75 74 65 2c 20 53 79 73
        74 65 6d 2e 52 75 6e 74 69 6d 65 2c 20 56 65 72
        73 69 6f 6e 3d 38 2e 30 2e 30 2e 30 2c 20 43 75
        6c 74 75 72 65 3d 6e 65 75 74 72 61 6c 2c 20 50
        75 62 6c 69 63 4b 65 79 54 6f 6b 65 6e 3d 62 30
        33 66 35 66 37 66 31 31 64 35 30 61 33 61 15 01
        54 02 10 53 6b 69 70 56 65 72 69 66 69 63 61 74
        69 6f 6e 01
    )
    .hash algorithm 0x00008004 // SHA1
    .ver 0:0:0:0
}");

        Assert.False(result.IsExtern);
        Assert.Equal("_", result.AssemblyName);
        Assert.Equal(7, result.DescendantNodes.Count()); // Don't forget that we also have an inline comment with text "SHA1" next to ".hash algorithm" node.

        Assert.True(result.DescendantNodes.ElementAt(0) is CustomAttributeSyntax);
        Assert.True(result.DescendantNodes.ElementAt(1) is CustomAttributeSyntax);
        Assert.True(result.DescendantNodes.ElementAt(2) is CustomAttributeSyntax);
        Assert.True(result.DescendantNodes.ElementAt(3) is PermissionSetSyntax);
        Assert.True(result.DescendantNodes.ElementAt(4) is HashAlgorithmSyntax);
        Assert.True(result.DescendantNodes.ElementAt(5) is InlineCommentSyntax);
        Assert.True(result.DescendantNodes.ElementAt(6) is VerDirectiveSyntax);

        TestElement1();
        TestElement2();
        TestElement3();
        TestElement4();
        TestElement5();
        TestElement6();
        TestElement7();

        void TestElement1()
        {
            var element = (CustomAttributeSyntax)result.DescendantNodes.ElementAt(0);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.TypeReference is NonGenericTypeReferenceSyntax);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.MethodName == ".ctor");
            Assert.True(((NonGenericTypeReferenceSyntax)element.AttributeConstructorTarget.MethodInvocation.TypeReference).ClassName == "System.Runtime.CompilerServices.CompilationRelaxationsAttribute");
            Assert.Single(element.AttributeConstructorTarget.MethodInvocation.Arguments.Arguments);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.Arguments.Arguments.ElementAt(0) is PredefinedTypeSyntax predefinedType
                && predefinedType.Kind == PredefinedTypeKind.Int32
                && predefinedType.TypeName == "int32");
            Assert.True(
                Enumerable.SequenceEqual(
                    element.GetRawBytes(),
                    new byte[] { 0x01, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00 }
                )
            );
        }

        void TestElement2()
        {
            var element = (CustomAttributeSyntax)result.DescendantNodes.ElementAt(1);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.TypeReference is NonGenericTypeReferenceSyntax);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.MethodName == ".ctor");
            Assert.True(((NonGenericTypeReferenceSyntax)element.AttributeConstructorTarget.MethodInvocation.TypeReference).ClassName == "System.Runtime.CompilerServices.RuntimeCompatibilityAttribute");
            Assert.Empty(element.AttributeConstructorTarget.MethodInvocation.Arguments.Arguments);
            Assert.True(
                Enumerable.SequenceEqual(
                    element.GetRawBytes(),
                    new byte[]
                    {
                        0x01, 0x00, 0x01, 0x00, 0x54, 0x02, 0x16, 0x57,
                        0x72, 0x61, 0x70, 0x4e, 0x6f, 0x6e, 0x45, 0x78, 0x63, 0x65,
                        0x70, 0x74, 0x69, 0x6f, 0x6e, 0x54, 0x68, 0x72, 0x6f, 0x77,
                        0x73, 0x01
                    }
                )
            );
        }

        void TestElement3()
        {
            var element = (CustomAttributeSyntax)result.DescendantNodes.ElementAt(2);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.TypeReference is NonGenericTypeReferenceSyntax);
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.MethodName == ".ctor");
            Assert.Equal("System.Runtime", ((NonGenericTypeReferenceSyntax)element.AttributeConstructorTarget.MethodInvocation.TypeReference).AssemblyReference?.AssemblyName);
            Assert.Equal("System.Diagnostics.DebuggableAttribute", (((NonGenericTypeReferenceSyntax)element.AttributeConstructorTarget.MethodInvocation.TypeReference).ClassName));
            Assert.True(element.AttributeConstructorTarget.MethodInvocation.Arguments.Arguments.First() is NonGenericTypeReferenceSyntax nonGeneric &&
                nonGeneric.Prefix == TypePrefix.ValueType &&
                nonGeneric.ClassName == "System.Diagnostics.DebuggableAttribute/DebuggingModes");
            Assert.True(
                Enumerable.SequenceEqual(
                    element.GetRawBytes(),
                    new byte[]
                    {
                        0x01, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00
                    }
                )
            );
        }

        void TestElement4()
        {
            var element = (PermissionSetSyntax)result.DescendantNodes.ElementAt(3);
            Assert.Equal(SecurityAction.ReqMin, element.SecurityAction);
            Assert.True(element.GetRawBytes().SequenceEqual(
                s_complexCustomAttributePermissionSetDescendantByteData));
        }

        void TestElement5()
        {
            var hash = (HashAlgorithmSyntax)result.DescendantNodes.ElementAt(4);
            Assert.Equal("0x00008004", hash.Value);
        }

        void TestElement6()
        {
            var inlineComment = (InlineCommentSyntax)result.DescendantNodes.ElementAt(5);
            Assert.Equal("SHA1", inlineComment.CommentText.Trim()); // remove leading and trailing whitespaces in comment text
        }

        void TestElement7()
        {
            var verDirective = (VerDirectiveSyntax)result.DescendantNodes.ElementAt(6);
            Assert.Equal('0', verDirective.Major);
            Assert.Equal('0', verDirective.Minor);
            Assert.Equal('0', verDirective.Build);
            Assert.Equal('0', verDirective.Revision);
        }
    }
}
