using System.Collections.Immutable;

namespace ILSourceParser.Common;

/// <summary>
/// Provides lookups and dictionaries related to IL predefined types.
/// </summary>
public static class PredefinedTypeLookups
{
    private static readonly Lazy<ImmutableDictionary<PredefinedTypeKind, bool>> s_typeFuncLookup =
        new(() =>
        {
            var builder = ImmutableDictionary.CreateBuilder<PredefinedTypeKind, bool>();
            builder.Add(PredefinedTypeKind.Int8, true);
            builder.Add(PredefinedTypeKind.Int16, true);
            builder.Add(PredefinedTypeKind.Int32, true);
            builder.Add(PredefinedTypeKind.Int64, true);
            builder.Add(PredefinedTypeKind.UInt8, true);
            builder.Add(PredefinedTypeKind.UInt16, true);
            builder.Add(PredefinedTypeKind.UInt32, true);
            builder.Add(PredefinedTypeKind.UInt64, true);

            builder.Add(PredefinedTypeKind.String, false);
            builder.Add(PredefinedTypeKind.Char, false);
            builder.Add(PredefinedTypeKind.Boolean, false);
            builder.Add(PredefinedTypeKind.Void, false);
            builder.Add(PredefinedTypeKind.Object, false);
            builder.Add(PredefinedTypeKind.NativeInt, false);
            builder.Add(PredefinedTypeKind.NativeUInt, false);
            builder.Add(PredefinedTypeKind.NativeInt32, false);
            builder.Add(PredefinedTypeKind.NativeUInt32, false);

            return builder.ToImmutableDictionary();
        });

    private static readonly Lazy<ImmutableDictionary<string, PredefinedTypeKind>> s_typeParsingLookups =
        new(() =>
        {
            var builder = ImmutableDictionary.CreateBuilder<string, PredefinedTypeKind>();

            // Integer types
            builder.Add("int8", PredefinedTypeKind.Int8);
            builder.Add("int16", PredefinedTypeKind.Int16);
            builder.Add("int32", PredefinedTypeKind.Int32);
            builder.Add("int64", PredefinedTypeKind.Int64);
            builder.Add("uint8", PredefinedTypeKind.UInt8);
            builder.Add("uint16", PredefinedTypeKind.UInt16);
            builder.Add("uint32", PredefinedTypeKind.UInt32);
            builder.Add("uint64", PredefinedTypeKind.UInt64);

            // Floating point types
            builder.Add("float32", PredefinedTypeKind.Float32);
            builder.Add("float64", PredefinedTypeKind.Float64);

            // Text types
            builder.Add("string", PredefinedTypeKind.String);
            builder.Add("char", PredefinedTypeKind.Char);

            // Miscellaneous
            builder.Add("object", PredefinedTypeKind.Object);
            builder.Add("boolean", PredefinedTypeKind.Boolean);
            builder.Add("void", PredefinedTypeKind.Void);

            // OS dependent integer types
            builder.Add("native int", PredefinedTypeKind.NativeInt);
            builder.Add("native int32", PredefinedTypeKind.NativeInt32);
            builder.Add("native uint", PredefinedTypeKind.NativeUInt);
            builder.Add("native uint32", PredefinedTypeKind.NativeUInt32);

            return builder.ToImmutableDictionary();
        });

    /// <summary>
    /// Represents a lookup that checks whether the given predefined
    /// type can be a function or not. For example, passing <see cref="PredefinedTypeKind.Int32"/>
    /// will return <see langword="true"/> because <c>int32(123)</c> is a valid
    /// function in IL, but passing <see cref="PredefinedTypeKind.String"/> will
    /// return <see langword="false"/> because <c>string("Hi!")</c> is not a valid
    /// function in IL.
    /// </summary>
    public static ImmutableDictionary<PredefinedTypeKind, bool> TypeFunctionLookups
    {
        get
        {
            return s_typeFuncLookup.Value;
        }
    }

    /// <summary>
    /// Represents a lookup that returns <see cref="PredefinedTypeKind"/> based on
    /// the type name. For example, passing the string <c>"int32"</c> will
    /// return <see cref="PredefinedTypeKind.Int32"/>.
    /// </summary>
    public static ImmutableDictionary<string, PredefinedTypeKind> TypeParsingLookups
    {
        get
        {
            return s_typeParsingLookups.Value;
        }
    }
}
