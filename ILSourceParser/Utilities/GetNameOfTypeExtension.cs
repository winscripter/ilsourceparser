using ILSourceParser.Syntax;

namespace ILSourceParser.Utilities;

/// <summary>
/// Represents an extension that provides the method <see cref="GetNameOfType(TypeReferenceSyntax)"/> to the
/// syntax node <see cref="TypeReferenceSyntax"/>.
/// </summary>
public static class GetNameOfTypeExtension
{
    /// <summary>
    /// Returns the full name of the given type reference.
    /// </summary>
    /// <param name="syntax">The syntax node that contains the type reference.</param>
    /// <returns>Full name of the type being referenced.</returns>
    public static string GetNameOfType(this TypeReferenceSyntax syntax)
    {
        if (syntax is NonGenericTypeReferenceSyntax nonGeneric)
        {
            string name = nonGeneric.ClassName;
            return name;
        }
        else if (syntax is GenericTypeReferenceSyntax generic)
        {
            string name;
            if (generic.GenericArguments != null)
            {
                name =
                    generic.ClassName +
                    '`' +
                    generic.GenericArguments.Parameters.Count().ToString();
            }
            else
            {
                name = generic.ClassName;
            }
            return name;
        }
        else
        {
            return string.Empty;
        }
    }
}
