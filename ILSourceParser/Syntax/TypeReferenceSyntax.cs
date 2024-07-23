using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// This node is associated with the following kinds:
/// <list type="bullet">
///   <item>
///     <see cref="NonGenericTypeReferenceSyntax"/>
///   </item>
///   <item>
///     <see cref="GenericTypeReferenceSyntax"/>
///   </item>
/// </list>
/// </summary>
public abstract class TypeReferenceSyntax : TypeBaseSyntax
{
    protected TypeReferenceSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia) : base(leadingTrivia, trailingTrivia)
    {
    }

    public string GetNameOfType()
    {
        if (this is NonGenericTypeReferenceSyntax nonGeneric)
        {
            string name = nonGeneric.ClassName;
            return name;
        }
        else if (this is GenericTypeReferenceSyntax generic)
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
