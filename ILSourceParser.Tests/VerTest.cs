using Sprache;

namespace ILSourceParser.Tests;

public class VerTest
{
    [Fact]
    public void Test()
    {
        const string input = @".ver 1:2:3:4";
        var result = new Parser().ParseVerDirective().Parse(input);

        Assert.Equal('1', result.Major);
        Assert.Equal('2', result.Minor);
        Assert.Equal('3', result.Build);
        Assert.Equal('4', result.Revision);
    }
}
