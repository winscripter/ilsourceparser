using ILSourceParser.Trivia;
using System.Collections.Immutable;

namespace ILSourceParser.Syntax;

public class StringLiteralSyntax : LiteralSyntax, IConvertible
{
    public string RawValue { get; init; }

    internal StringLiteralSyntax(ImmutableArray<SyntaxTrivia> leadingTrivia, ImmutableArray<SyntaxTrivia> trailingTrivia, string rawValue) : base(leadingTrivia, trailingTrivia)
    {
        RawValue = rawValue;
    }

    public TypeCode GetTypeCode()
    {
        return RawValue.GetTypeCode();
    }

    public bool ToBoolean(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToBoolean(provider);
    }

    public byte ToByte(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToByte(provider);
    }

    public char ToChar(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToChar(provider);
    }

    public DateTime ToDateTime(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToDateTime(provider);
    }

    public decimal ToDecimal(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToDecimal(provider);
    }

    public double ToDouble(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToDouble(provider);
    }

    public short ToInt16(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToInt16(provider);
    }

    public int ToInt32(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToInt32(provider);
    }

    public long ToInt64(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToInt64(provider);
    }

    public sbyte ToSByte(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToSByte(provider);
    }

    public float ToSingle(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToSingle(provider);
    }

    public string ToString(IFormatProvider? provider)
    {
        return RawValue.ToString(provider);
    }

    public object ToType(Type conversionType, IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToType(conversionType, provider);
    }

    public ushort ToUInt16(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToUInt16(provider);
    }

    public uint ToUInt32(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToUInt32(provider);
    }

    public ulong ToUInt64(IFormatProvider? provider)
    {
        return ((IConvertible)RawValue).ToUInt64(provider);
    }
}
