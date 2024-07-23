using ILSourceParser.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ILSourceParser.Utilities;

/// <summary>
/// A bunch of extension methods that return a byte array or enumerable
/// for the given byte node array or syntax node.
/// </summary>
public static class GetRawBytesExtensions
{
    private static byte InternalParseByte(ByteSyntax @byte)
    {
        try
        {
            return byte.Parse(@byte.Value, NumberStyles.HexNumber);
        }
        catch
        {
            // Can still throw unhandled exception if the number is
            // not a valid integer.
            return byte.Parse(@byte.Value);
        }
    }

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="input">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    public static byte[] GetRawBytes(this IEnumerable<ByteSyntax> input) =>
        input.Select(InternalParseByte).ToArray();

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="input">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    public static IEnumerable<byte> GetRawBytesAsEnumerable(this IEnumerable<ByteSyntax> input) =>
        input.Select(InternalParseByte);

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="attribute">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    public static IEnumerable<byte> GetRawBytes(this CustomAttributeSyntax attribute) =>
        attribute.AttributeData.GetRawBytesAsEnumerable();

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="attribute">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static IEnumerable<byte> GetRawBytes(this AnonymousCustomAttributeSyntax attribute) =>
        [];

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="attribute">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    public static IEnumerable<byte> GetRawBytes(this BaseCustomAttributeSyntax attribute) =>
        attribute is CustomAttributeSyntax custom ? custom.GetRawBytes() : [];

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="permissionSet">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    public static IEnumerable<byte> GetRawBytes(this PermissionSetSyntax permissionSet) =>
        permissionSet.ByteArray.Select(b => b.AsByte());

    /// <summary>
    /// Returns raw byte array or enumerable for the given input.
    /// </summary>
    /// <param name="byteArray">The input to convert to a byte sequence.</param>
    /// <returns>A sequence of bytes that represent the array, node, or enumerable.</returns>
    public static IEnumerable<byte> GetRawBytes(this ByteArraySyntax byteArray) =>
        byteArray.Bytes.Select(b => b.AsByte());
}
