namespace ILSourceParser.Common;

/// <summary>
/// Represents the well-known primitive type in IL code.
/// </summary>
public enum PredefinedTypeKind
{
    /// <summary>
    /// <c>int8</c> - equivalent to <see cref="sbyte"/> in C#.
    /// </summary>
    Int8,

    /// <summary>
    /// <c>int16</c> - equivalent to <see cref="short"/> in C#.
    /// </summary>
    Int16,

    /// <summary>
    /// <c>int32</c> - equivalent to <see cref="int"/> in C#.
    /// </summary>
    Int32,

    /// <summary>
    /// <c>int64</c> - equivalent to <see cref="long"/> in C#.
    /// </summary>
    Int64,

    /// <summary>
    /// <c>uint8</c> - equivalent to <see cref="byte"/> in C#.
    /// </summary>
    UInt8,

    /// <summary>
    /// <c>uint16</c> - equivalent to <see cref="ushort"/> in C#.
    /// </summary>
    UInt16,

    /// <summary>
    /// <c>uint32</c> - equivalent to <see cref="uint"/> in C#.
    /// </summary>
    UInt32,

    /// <summary>
    /// <c>uint64</c> - equivalent to <see cref="ulong"/> in C#.
    /// </summary>
    UInt64,

    /// <summary>
    /// <c>float32</c> - equivalent to <see cref="float"/> in C#.
    /// </summary>
    Float32,

    /// <summary>
    /// <c>float64</c> - equivalent to <see cref="double"/> in C#.
    /// </summary>
    Float64,

    /// <summary>
    /// <c>string</c> - equivalent to <see cref="string"/> in C#.
    /// </summary>
    String,

    /// <summary>
    /// <c>char</c> - equivalent to <see cref="char"/> in C#.
    /// </summary>
    Char,

    /// <summary>
    /// <c>bool</c> - equivalent to <see cref="bool"/> in C#.
    /// </summary>
    Boolean,

    /// <summary>
    /// <c>object</c> - equivalent to <see cref="object"/> in C#.
    /// </summary>
    Object,

    /// <summary>
    /// <c>void</c> - equivalent to <see cref="void"/> in C#.
    /// </summary>
    Void,

    /// <summary>
    /// <c>native int</c> - equivalent to <see cref="nint"/> in C#.
    /// </summary>
    NativeInt,

    /// <summary>
    /// <c>native uint</c> - equivalent to <see cref="nuint"/> in C#.
    /// </summary>
    NativeUInt,

    /// <summary>
    /// <c>native int32</c> - equivalent to <see cref="nint"/> in C#.
    /// </summary>
    NativeInt32,

    /// <summary>
    /// <c>native uint32</c> - equivalent to <see cref="nuint"/> in C#.
    /// </summary>
    NativeUInt32,
}
