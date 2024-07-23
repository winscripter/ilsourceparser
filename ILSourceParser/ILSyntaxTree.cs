using Sprache;

namespace ILSourceParser;

/// <summary>
/// Provides functionality for parsing Microsoft Intermediate Language (IL) syntax
/// into a syntax tree.
/// </summary>
public static class ILSyntaxTree
{
    /// <summary>
    /// Parses IL source into a syntax tree.
    /// </summary>
    /// <param name="text">A string that represents the IL source code.</param>
    /// <returns>A new <see cref="SyntaxTree"/> that represents IL syntax tree.</returns>
    public static SyntaxTree ParseText(string text)
    {
        return ParseText(text: text, filePath: null);
    }

    /// <summary>
    /// Parses IL source into a syntax tree.
    /// </summary>
    /// <param name="text">A string that represents the IL source code.</param>
    /// <param name="filePath">The path to the source file containing IL code.</param>
    /// <returns>A new <see cref="SyntaxTree"/> that represents IL syntax tree.</returns>
    public static SyntaxTree ParseText(string text, string? filePath = null)
    {
        if (filePath is not null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
        }

        var parser = new Parser();
        var rootParser = parser.Root();
        var result = rootParser.Parse(text);

        var syntaxTree = new SyntaxTree(result);
        if (filePath is not null)
        {
            syntaxTree.FilePath = filePath;
        }

        return syntaxTree;
    }

    /// <summary>
    /// Parses IL source into a syntax tree.
    /// </summary>
    /// <param name="text">A string that represents the IL source code.</param>
    /// <returns>A new <see cref="SyntaxTree"/> that represents IL syntax tree.</returns>
    public static async Task<SyntaxTree> ParseTextAsync(string text)
    {
        return await Task.Run(() => ParseText(text));
    }

    /// <summary>
    /// Parses IL source into a syntax tree.
    /// </summary>
    /// <param name="text">A string that represents the IL source code.</param>
    /// <param name="filePath">The path to the source file containing IL code.</param>
    /// <returns>A new <see cref="SyntaxTree"/> that represents IL syntax tree.</returns>
    public static async Task<SyntaxTree> ParseTextAsync(string text, string? filePath)
    {
        return await Task.Run(() => ParseText(text, filePath));
    }
}
