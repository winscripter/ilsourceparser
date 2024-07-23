namespace ILSourceParser;

/// <summary>
/// Represents the Intermediate Language syntax tree.
/// </summary>
public class SyntaxTree
{
    private readonly ILRootNode _rootNode;

    private string? _filePath;

    internal SyntaxTree(ILRootNode rootNode)
    {
        _rootNode = rootNode;
    }

    /// <summary>
    /// Returns the root node for the IL syntax tree.
    /// </summary>
    /// <returns>A root syntax node that specifies the top-level syntax nodes.</returns>
    public ILRootNode GetRoot() => _rootNode;

    /// <summary>
    /// Returns the root node for the IL syntax tree asynchronously.
    /// </summary>
    /// <returns>A root syntax node that specifies the top-level syntax nodes.</returns>
    public async Task<ILRootNode> GetRootAsync()
    {
        return await Task.Run(() =>
        {
            return _rootNode;
        });
    }

    /// <summary>
    /// Returns the name of the source file where its contents were parsed into
    /// IL syntax tree. This throws an <see cref="InvalidOperationException"/>
    /// by default while getting the file path.
    /// To ensure the source file is specified, use the <see cref="IsFilePathInitialized"/>
    /// property.
    /// </summary>
    public string FilePath
    {
        get
        {
            return _filePath ?? throw new InvalidOperationException("File path is missing");
        }
        set
        {
            string path = value;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            _filePath = path;
        }
    }

    /// <summary>
    /// Checks whether the file path is specified. Returns <see langword="false" />. To specify
    /// the source file, change the <see cref="FilePath"/> property.
    /// </summary>
    public bool IsFilePathInitialized
    {
        get
        {
            return _filePath != null;
        }
    }
}
