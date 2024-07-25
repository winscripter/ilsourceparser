using ILSourceParser.Common;
using ILSourceParser.Syntax.Marshaling;
using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class FieldDeclarationSyntax : TypeSyntax
{
    internal FieldDeclarationSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        bool isLiteral,
        bool isStatic,
        bool isInitOnly,
        AccessModifier accessModifier,
        TypeSyntax? initialValue,
        string name,
        TypeSyntax fieldType,
        MarshalSyntax? marshalling) : base(leadingTrivia, trailingTrivia)
    {
        IsLiteral = isLiteral;
        IsStatic = isStatic;
        IsInitOnly = isInitOnly;
        AccessModifier = accessModifier;
        InitialValue = initialValue;
        Name = name;
        FieldType = fieldType;
        Marshalling = marshalling;
    }

    public bool IsLiteral { get; init; }
    public bool IsStatic { get; init; }
    public bool IsInitOnly { get; init; }
    public AccessModifier AccessModifier { get; init; }
    public TypeSyntax? InitialValue { get; init; }
    public string Name { get; init; }
    public TypeSyntax FieldType { get; init; }
    public MarshalSyntax? Marshalling { get; init; }
}
