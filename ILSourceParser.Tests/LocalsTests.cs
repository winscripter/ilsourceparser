using ILSourceParser.Syntax;
using Sprache;

namespace ILSourceParser.Tests;

public class LocalsTests
{
    [Fact]
    public void ParseLocals()
    {
        var parser = new Parser();
        var localsParser = parser.ParseLocalsDirective();

        Test1();
        Test2();

        // Parse an empty '.locals' directive
        void Test1()
        {
            var parseResult = localsParser.Parse(".locals ()");
            Assert.False(parseResult.IsLocalsInit); // this is just '.locals', not '.locals init'
            Assert.Empty(parseResult.Variables);
        }

        // Test a more advanced '.locals init' directive with things
        // like explicit variable indexes, special variable names,
        // or even anonymous indexes/type references.
        void Test2()
        {
            var parseResult = localsParser.Parse(@".locals init (
    [0] int32 '\\MyBackslashVariable',
    [1] int32 'Variable With Spaces',
    int32 'Anonymous Index',
    [2] [MyAssembly]MyClass.MyUnicorn 'Cool Variable!'
)");

            Assert.True(parseResult.IsLocalsInit);
            Assert.Equal(4, parseResult.Variables.Count());

            AssertVariable1();
            AssertVariable2();
            AssertVariable3();
            AssertVariable4();

            void AssertVariable1()
            {
                var firstElement = parseResult.Variables.ElementAt(0);
                Assert.Equal("0", firstElement.Index);
                Assert.Equal("int32", (firstElement.VariableType as PredefinedTypeSyntax)!.TypeName);

                // There's actually two backslashes as we specified, since
                // the input string is a verbatim string (where a single backslash
                // is treated as a single backslash) but this string isn't a
                // verbatim string (where backslashes have to be escaped, so this is
                // a double backslash for one).
                Assert.Equal("\\\\MyBackslashVariable", firstElement.VariableName);
            }

            void AssertVariable2()
            {
                var secondVariable = parseResult.Variables.ElementAt(1);
                Assert.Equal("1", secondVariable.Index);
                Assert.Equal("int32", (secondVariable.VariableType as PredefinedTypeSyntax)!.TypeName);
                Assert.Equal("Variable With Spaces", secondVariable.VariableName);
            }

            void AssertVariable3()
            {
                var thirdElement = parseResult.Variables.ElementAt(2);
                Assert.Null(thirdElement.Index);
                Assert.Equal("int32", (thirdElement.VariableType as PredefinedTypeSyntax)!.TypeName);
                Assert.Equal("Anonymous Index", thirdElement.VariableName);
            }

            void AssertVariable4()
            {
                var fourthElement = parseResult.Variables.ElementAt(3);
                Assert.Equal("2", fourthElement.Index);
                Assert.Equal("Cool Variable!", fourthElement.VariableName);
                Assert.Equal("MyClass.MyUnicorn", (fourthElement.VariableType as NonGenericTypeReferenceSyntax)!.ClassName);
                Assert.Equal("MyAssembly", (fourthElement.VariableType as NonGenericTypeReferenceSyntax)!.AssemblyReference!.AssemblyName);
            }
        }
    }
}
