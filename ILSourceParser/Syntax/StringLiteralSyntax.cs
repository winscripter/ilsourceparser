using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

/// <summary>
/// Represents an IL string literal, such as <c>"this!\n"</c>.
/// </summary>
public class StringLiteralSyntax : LiteralSyntax, IConvertible
{
    /// <summary>
    /// Represents the value of the string literal, including unescaped escape characters.
    /// </summary>
    public string RawValue { get; init; }

    internal StringLiteralSyntax(
        ImmutableArray<SyntaxTrivia> leadingTrivia,
        ImmutableArray<SyntaxTrivia> trailingTrivia,
        string rawValue) : base(leadingTrivia, trailingTrivia)
    {
        RawValue = rawValue;
    }

    /// <inheritdoc cref="IConvertible.GetTypeCode()" />
    public TypeCode GetTypeCode()
    {
        return RawValue.GetTypeCode();
    }

    /// <inheritdoc cref="IConvertible.ToBoolean(IFormatProvider?)" />
    public bool ToBoolean(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToBoolean(provider);
    }

    /// <inheritdoc cref="IConvertible.ToByte(IFormatProvider?)" />
    public byte ToByte(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToByte(provider);
    }

    /// <inheritdoc cref="IConvertible.ToChar(IFormatProvider?)" />
    public char ToChar(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToChar(provider);
    }

    /// <inheritdoc cref="IConvertible.ToDateTime(IFormatProvider?)" />
    public DateTime ToDateTime(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToDateTime(provider);
    }

    /// <inheritdoc cref="IConvertible.ToDecimal(IFormatProvider?)" />
    public decimal ToDecimal(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToDecimal(provider);
    }

    /// <inheritdoc cref="IConvertible.ToDouble(IFormatProvider?)" />
    public double ToDouble(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToDouble(provider);
    }

    /// <inheritdoc cref="IConvertible.ToInt16(IFormatProvider?)" />
    public short ToInt16(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToInt16(provider);
    }

    /// <inheritdoc cref="IConvertible.ToInt32(IFormatProvider?)" />
    public int ToInt32(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToInt32(provider);
    }

    /// <inheritdoc cref="IConvertible.ToInt64(IFormatProvider?)" />
    public long ToInt64(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToInt64(provider);
    }

    /// <inheritdoc cref="IConvertible.ToSByte(IFormatProvider?)" />
    public sbyte ToSByte(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToSByte(provider);
    }

    /// <inheritdoc cref="IConvertible.ToSingle(IFormatProvider?)" />
    public float ToSingle(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToSingle(provider);
    }

    /// <inheritdoc cref="IConvertible.ToString(IFormatProvider?)" />
    public string ToString(IFormatProvider? provider)
    {
        return RawValue.ToString(provider);
    }

    /// <inheritdoc cref="IConvertible.ToType(Type, IFormatProvider?)" />
    public object ToType(Type conversionType, IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToType(conversionType, provider);
    }

    /// <inheritdoc cref="IConvertible.ToUInt16(IFormatProvider?)" />
    public ushort ToUInt16(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToUInt16(provider);
    }

    /// <inheritdoc cref="IConvertible.ToUInt32(IFormatProvider?)" />
    public uint ToUInt32(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToUInt32(provider);
    }

    /// <inheritdoc cref="IConvertible.ToUInt64(IFormatProvider?)" />
    public ulong ToUInt64(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToUInt64(provider);
    }
}
