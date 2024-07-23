using ILSourceParser.Syntax;
using System.Globalization;

namespace ILSourceParser.Utilities;

/// <summary>
/// Represents a single extension method that provides the <see cref="AsByte(ByteSyntax)"/> method
/// to <see cref="ByteSyntax"/>.
/// </summary>
public static class AsByteExtension
{
    private const NumberStyles s_preferredAsByteNumberStyle = NumberStyles.HexNumber;

    /// <summary>
    /// Converts the string value of <see cref="ByteSyntax"/> as <see cref="byte"/>.
    /// </summary>
    /// <param name="syntax">The input <see cref="ByteSyntax"/>.</param>
    /// <returns><see cref="ByteSyntax"/> value converted to <see cref="byte"/>.</returns>
    public static byte AsByte(this ByteSyntax syntax)
        => byte.Parse(syntax.Value, s_preferredAsByteNumberStyle);
}
