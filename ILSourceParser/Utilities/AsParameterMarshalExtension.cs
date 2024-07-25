using ILSourceParser.Syntax.Marshaling;

namespace ILSourceParser.Utilities;

/// <summary>
/// Converts <see cref="MarshalTypeSyntax"/> to <see cref="ParameterMarshalSyntax"/>.
/// </summary>
public static class AsParameterMarshalExtension
{
    /// <summary>
    /// Converts <see cref="MarshalTypeSyntax"/> to <see cref="ParameterMarshalSyntax"/>.
    /// </summary>
    /// <param name="marshalType">The input type of marshal to process.</param>
    /// <returns>An output marshal.</returns>
    public static ParameterMarshalSyntax AsParameterMarshal(
        this MarshalTypeSyntax marshalType)
    {
        return new ParameterMarshalSyntax(
            leadingTrivia: marshalType.LeadingTrivia,
            trailingTrivia: marshalType.TrailingTrivia,
            marshalType: marshalType);
    }
}
