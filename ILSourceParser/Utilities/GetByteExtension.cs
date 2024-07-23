using ILSourceParser.Syntax;

namespace ILSourceParser.Utilities;

/// <summary>
/// Represents a single extension method <see cref="GetBoolean(BooleanLiteralSyntax)"/>
/// for <see cref="BooleanLiteralSyntax"/>.
/// </summary>
public static class GetByteExtension
{
    /// <summary>
    /// Returns the boolean for <see cref="BooleanLiteralSyntax"/> based on the value
    /// of the literal, whether it is <c>true</c> or <c>false</c>.
    /// </summary>
    /// <param name="literal">The input boolean literal.</param>
    /// <returns>A boolean from the boolean literal.</returns>
    public static bool GetBoolean(this BooleanLiteralSyntax literal) =>
        literal.Value.Equals(BooleanLiteralSyntax.TrueEqualityString);
}
