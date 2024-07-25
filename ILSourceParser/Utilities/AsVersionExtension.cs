using ILSourceParser.Syntax;

namespace ILSourceParser.Utilities;

/// <summary>
/// Represents a single extension that provides the method <see cref="AsVersion(VerDirectiveSyntax)"/> to <see cref="VerDirectiveSyntax"/>.
/// </summary>
public static class AsVersionExtension
{
    /// <summary>
    /// Converts values of the <c>.ver</c> directive into <see cref="Version"/>.
    /// </summary>
    /// <param name="directive">The information about the <c>.ver</c> IL directive.</param>
    /// <returns>A new instance of <see cref="Version"/>.</returns>
    public static Version AsVersion(this VerDirectiveSyntax directive) =>
        new(directive.Major - '0', directive.Minor - '0', directive.Build - '0', directive.Revision - '0');
}
