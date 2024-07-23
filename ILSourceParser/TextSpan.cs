using Sprache;

namespace ILSourceParser;

public record struct TextSpan(int Line, int Column, int Index)
{
    internal static TextSpan Convert(Position pos)
    {
        return new(pos.Line, pos.Column, pos.Pos);
    }
}
