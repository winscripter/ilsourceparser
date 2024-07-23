using ILSourceParser.Syntax;
using ILSourceParser.Syntax.Instructions;

namespace ILSourceParser;

/// <summary>
/// Represents the Microsoft Intermediate Language (IL) root node.
/// </summary>
public class ILRootNode : IEquatable<ILRootNode?>
{
    /// <summary>
    /// Represents every top-level syntax node, excluding
    /// descendant nodes. Examples include the following nodes:
    /// <list type="bullet">
    ///   <item><see cref="AssemblyDeclarationSyntax"/></item>
    ///   <item><see cref="ClassDeclarationSyntax"/></item>
    ///   <item><see cref="FileAlignmentDirectiveSyntax"/></item>
    ///   <item><see cref="ImageBaseDirectiveSyntax"/></item>
    /// </list>
    /// ... etc
    /// <para/>
    /// But, not these:
    /// <list type="bullet">
    ///   <item><see cref="FieldOrPropertyReferenceSyntax"/> - only inside of a class</item>
    ///   <item><see cref="PermissionSetSyntax"/> - only inside of an assembly</item>
    ///   <item><see cref="CustomAttributeSyntax"/> - only inside of a method, class, property, assembly, etc</item>
    ///   <item><see cref="InstructionSyntax"/> - only inside of a method</item>
    /// </list>
    /// ... etc
    /// <para/>
    /// Example IL code to demonstrate:
    /// <example>
    ///   <code>
    /// // Root here
    /// .assembly MyAssembly
    /// {
    ///     // Descendant nodes not associated here, but are part
    ///     // of AssemblyDeclarationSyntax
    ///     .ver 1:2:3:4
    /// }
    /// 
    /// // This node will be included
    /// .imagebase 0x00400000
    ///   </code>
    /// </example>
    /// </summary>
    public IEnumerable<SyntaxNode> DescendantNodes { get; init; }

    /// <summary>
    /// Checks whether the syntax root does not contain any code (e.g. it is
    /// either empty or consists of comment syntax nodes only).
    /// </summary>
    public bool HasNoCode
    {
        get
        {
            int items = DescendantNodes.Count(node => node is not BaseCommentSyntax);
            return items == 0;
        }
    }

    /// <summary>
    /// Checks whether the syntax root does not contain any syntax nodes at all.
    /// </summary>
    public bool HasDescendantNodes
    {
        get
        {
            return DescendantNodes.Any();
        }
    }

    /// <summary>
    /// Calculates sum of leading and trailing trivias of every top-level syntax node.
    /// </summary>
    /// <returns>A number of leading and trailing trivias in root scoped syntax nodes.</returns>
    public int CountRootScopedSyntaxNodeTrivia()
    {
        return HasNoCode ? 0 : CountAll();

        int CountAll()
        {
            var leading = DescendantNodes.Select(node => node.LeadingTrivia.Length).Sum();
            var trailing = DescendantNodes.Select(node => node.TrailingTrivia.Length).Sum();
            return leading + trailing;
        }
    }

    /// <inheritdoc cref="object.Equals(object?)" />
    public override bool Equals(object? obj)
    {
        return Equals(obj as ILRootNode);
    }

    /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
    public bool Equals(ILRootNode? other)
    {
        return other is not null &&
               EqualityComparer<IEnumerable<SyntaxNode>>.Default.Equals(DescendantNodes, other.DescendantNodes) &&
               HasNoCode == other.HasNoCode &&
               HasDescendantNodes == other.HasDescendantNodes;
    }

    /// <inheritdoc cref="object.GetHashCode()" />
    public override int GetHashCode()
    {
        return HashCode.Combine(DescendantNodes, HasNoCode, HasDescendantNodes);
    }

    internal ILRootNode(IEnumerable<SyntaxNode> descendantNodes)
    {
        DescendantNodes = descendantNodes;
    }

    /// <summary>
    /// Checks whether the left root node is equal to the right root node.
    /// </summary>
    /// <param name="left">The left root node to compare from.</param>
    /// <param name="right">The right root node to compare with.</param>
    /// <returns>A boolean, indicating whether the left root node is equal to another or not.</returns>
    public static bool operator ==(ILRootNode? left, ILRootNode? right)
    {
        return EqualityComparer<ILRootNode>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks whether the left root node is different from the right root node.
    /// </summary>
    /// <param name="left">The left root node to compare from.</param>
    /// <param name="right">The right root node to compare with.</param>
    /// <returns>A boolean, indicating whether the left root node is different from another or not.</returns>
    public static bool operator !=(ILRootNode? left, ILRootNode? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns a sequence containing syntax nodes different from each other compared
    /// to <paramref name="other"/> syntax root.
    /// </summary>
    /// <param name="other">The other syntax root to compare descendant nodes with.</param>
    /// <returns>Sequence containing top-level syntax nodes different with this and other syntax root.</returns>
    /// <exception cref="ArgumentException">Thrown when this and other syntax root do not have equal amounts of descendant nodes.</exception>
    public IEnumerable<(SyntaxNode, SyntaxNode)> DifferentiateRootNodes(ILRootNode other)
    {
        if (DescendantNodes.Count() != other.DescendantNodes.Count())
        {
            int expected = CountRootScopedSyntaxNodeTrivia();
            int actual = other.CountRootScopedSyntaxNodeTrivia();
            throw new ArgumentException($"This root node does not have an equal amount of descendant nodes than the other node: expected {expected} nodes, got {actual} nodes",
                nameof(other));
        }

        List<(SyntaxNode, SyntaxNode)> nodes = [];
        foreach ((SyntaxNode Left, SyntaxNode Right) in DescendantNodes.Zip(other.DescendantNodes))
        {
            if (Left != Right)
            {
                nodes.Add((Left, Right));
            }
        }

        return nodes;
    }
}
