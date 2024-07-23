using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax.Instructions;

public sealed class ArgumentLoadOpCodeSyntax : InstructionSyntax
{
    public string Name { get; init; }

    /// <summary>
    /// The index of the argument. Example:
    /// <example>
    ///   <code>
    /// ldarg 42
    ///   </code>
    /// </example>
    /// In the example above, the value of this property will be 42.
    /// </summary>
    /// <remarks>
    /// <b>Remark</b>: If the value of this property is <see langword="NULL"/>, this
    /// means that the opcode that loads argument by index does not require an
    /// explicit index to be specified. For example, the instruction <c>ldarg.0</c>
    /// doesn't take any parameters and is just an equivalent to <c>ldarg 0</c>, although
    /// that instruction is still categorized as <see cref="ArgumentLoadOpCodeSyntax"/>.
    /// </remarks>
    public ushort? Index { get; init; }

    internal ArgumentLoadOpCodeSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string name,
        ushort? index) : base(leadingTrivia, trailingTrivia)
    {
        Name = name;
        Index = index;
    }
}
