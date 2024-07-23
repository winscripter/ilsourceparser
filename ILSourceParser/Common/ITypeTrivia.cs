using ILSourceParser.Trivia;

namespace ILSourceParser.Common;

/// <summary>
/// Marks a syntax trivia as a type. Those include:
/// <list type="bullet">
///   <item><see cref="TypeAmpersandTrivia"/></item>
///   <item><see cref="TypeArrayTrivia"/></item>
///   <item><see cref="TypeAsteriskTrivia"/></item>
/// </list>
/// </summary>
public interface ITypeTrivia
{
}
