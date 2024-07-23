using ILSourceParser.Syntax;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ILSourceParser.Utilities;

/// <summary>
/// Extensions for <see cref="MethodFlagSyntax"/>.
/// </summary>
public static class MethodFlagSyntaxExtensions
{
    /// <summary>
    /// Returns the flag of the <see cref="MethodFlagSyntax"/> as <see cref="uint"/>.
    /// </summary>
    /// <param name="syntax">Input syntax node to process.</param>
    /// <returns><see cref="uint"/></returns>
    public static uint GetRawValueAsUInt32(this MethodFlagSyntax syntax)
    {
        try
        {
            return uint.Parse(syntax.Flag, NumberStyles.HexNumber);
        }
        catch
        {
            return uint.Parse(syntax.Flag);
        }
    }

    /// <summary>
    /// Returns <see cref="MethodImplOptions"/> from the given <see cref="MethodFlagSyntax"/>.
    /// </summary>
    /// <param name="syntax">Input syntax node to process.</param>
    /// <returns><see cref="MethodImplOptions"/></returns>
    public static MethodImplOptions GetMethodImplOptions(this MethodFlagSyntax syntax)
    {
        return (MethodImplOptions)syntax.GetRawValueAsUInt32();
    }
}
