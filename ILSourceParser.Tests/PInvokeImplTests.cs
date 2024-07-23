using Sprache;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace ILSourceParser.Tests;

public class PInvokeImplTests
{
    [Fact]
    public void TestSimplePInvokeImpl()
    {
        var parser = new Parser();
        var pinvokeimplParser = parser.ParsePInvokeImpl();
        var pinvokeResult = pinvokeimplParser.Parse(@"pinvokeimpl(""user32.dll"" as ""DummyEntryPoint"")");

        Assert.Equal("user32.dll", pinvokeResult.DllName);
        Assert.Equal("DummyEntryPoint", pinvokeResult.EntryPoint!.Name);
    }

    [Fact]
    public void TestComplexPInvokeImpl()
    {
        var parser = new Parser();
        var pinvokeimplParser = parser.ParsePInvokeImpl();
        var pinvokeResult = pinvokeimplParser.Parse(@"pinvokeimpl(""user32.dll"" as ""DummyEntryPoint"" nomangle autochar lasterr cdecl)");

        Assert.Equal("user32.dll", pinvokeResult.DllName);
        Assert.Equal("DummyEntryPoint", pinvokeResult.EntryPoint!.Name);
        Assert.True(pinvokeResult.ExactSpelling);
        Assert.True(pinvokeResult.SetLastError);
        Assert.Equal(CallingConvention.Cdecl, pinvokeResult.CallingConvention);
        Assert.Equal(CharSet.Auto, pinvokeResult.CharSet);
    }
}
