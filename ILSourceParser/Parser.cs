using ILSourceParser.Common;
using ILSourceParser.Syntax;
using ILSourceParser.Syntax.Instructions;
using ILSourceParser.Syntax.Instructions.OpCodes;
using ILSourceParser.Syntax.Marshaling;
using ILSourceParser.Trivia;
using ILSourceParser.Utilities;
using Sprache;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace ILSourceParser;

/// <summary>
/// Core parser for the Intermediate Language (IL) source syntax.
/// </summary>
internal sealed class Parser
{
    const string nameParseSymbolName = "ParseSymbolName()";
    const string nameParseGenericParameterTypeConstraint = "ParseGenericParameterTypeConstraint()";

    const string FieldFallbackName = "<null>";

    private delegate T ParsingInvocationCallback<T>();

    private readonly List<ILError> m_errors;

    public Parser(List<ILError> errors)
    {
        m_errors = errors;
    }

    public Parser() : this([])
    {
    }

    private T AddErrorAndGet<T>(Position position, T value)
    {
        m_errors.Add(new ILError(TextSpan.Convert(position)));
        return value;
    }

    private static Parser<WhitespaceTrivia> ParseWhiteSpaceTrivia()
    {
        return from whitespaces in Parse.WhiteSpace.Many().Text()
               select new WhitespaceTrivia(whitespaces.Length);
    }

    private static Parser<FunctionNameTrivia> ParseFunctionNameTrivia(string funcName)
    {
        return from _ in Parse.String(funcName)
               select new FunctionNameTrivia(funcName);
    }

    private static Parser<OpenParenthesisTrivia> ParseOpenParenthesis()
    {
        return from parseChar in Parse.Char('(')
               select new OpenParenthesisTrivia();
    }

    private static Parser<CloseParenthesisTrivia> ParseCloseParenthesis()
    {
        return from parseChar in Parse.Char(')')
               select new CloseParenthesisTrivia();
    }

    private static Parser<string> ParseNumberStyle(bool isFloatingPoint)
    {
        static string ComputeWithFloatingPoint(
            IOption<char> dash,
            string numbersL,
            char point,
            string numbersR,
            bool hasDash)
        {
            if (hasDash)
            {
                return $"{dash.Get()}{numbersL}{point}{numbersR}";
            }
            else
            {
                return $"{numbersL}{point}{numbersR}";
            }
        }

        if (isFloatingPoint)
        {
            return from dash in Parse.Char('-').Optional()
                   from numbersL in Parse.Digit.AtLeastOnce().Text()
                   from point in Parse.Char('.')
                   from numbersR in Parse.Digit.AtLeastOnce().Text()
                   let hasDash = dash.IsDefined
                   select ComputeWithFloatingPoint(dash, numbersL, point, numbersR, hasDash);
        }
        else
        {
            return from dash in Parse.Char('-').Optional()
                   from numbersL in Parse.Digit.AtLeastOnce().Text()
                   let hasDash = dash.IsDefined
                   select ComputeWithFloatingPoint(dash, numbersL, '.', "", hasDash);
        }
    }

    private Parser<PredefinedTypeSyntax> IntegerFunctionFactory(string functionName, PredefinedTypeKind kind, bool floatingPoint = false)
    {
        try
        {
            return from whitespaces in ParseWhiteSpaceTrivia()
                   from funcStart in ParseFunctionNameTrivia(functionName)
                   from openParen in ParseOpenParenthesis()
                   from whitespacesAfterFunc in ParseWhiteSpaceTrivia()
                   from numbers in ParseNumberStyle(floatingPoint)
                   from whitespacesAfterNumbers in ParseWhiteSpaceTrivia()
                   from closeParen in ParseCloseParenthesis()
                   select new PredefinedTypeSyntax(
                       leadingTrivia: [whitespaces, funcStart, openParen, whitespacesAfterFunc],
                       trailingTrivia: [whitespacesAfterNumbers, closeParen],
                       typeName: functionName,
                       kind: kind,
                       value: numbers);
        }
        catch (ParseException e)
        {
            m_errors.Add(new ILError(TextSpan.Convert(e.Position)));
            return Parse.Return(new PredefinedTypeSyntax([], [], "int32", PredefinedTypeKind.Int32, null));
        }
    }

    private Parser<PredefinedTypeSyntax> Int32Function()
    {
        return IntegerFunctionFactory("int32", PredefinedTypeKind.Int32);
    }

    private Parser<PredefinedTypeSyntax> Int8Function()
    {
        return IntegerFunctionFactory("int8", PredefinedTypeKind.Int8);
    }

    private Parser<PredefinedTypeSyntax> Int16Function()
    {
        return IntegerFunctionFactory("int16", PredefinedTypeKind.Int16);
    }

    private Parser<PredefinedTypeSyntax> Int64Function()
    {
        return IntegerFunctionFactory("int64", PredefinedTypeKind.Int64);
    }

    private Parser<PredefinedTypeSyntax> UInt32Function()
    {
        return IntegerFunctionFactory("uint32", PredefinedTypeKind.UInt32);
    }

    private Parser<PredefinedTypeSyntax> UInt8Function()
    {
        return IntegerFunctionFactory("uint8", PredefinedTypeKind.UInt8);
    }

    private Parser<PredefinedTypeSyntax> UInt16Function()
    {
        return IntegerFunctionFactory("uint16", PredefinedTypeKind.UInt16);
    }

    private Parser<PredefinedTypeSyntax> UInt64Function()
    {
        return IntegerFunctionFactory("uint64", PredefinedTypeKind.UInt64);
    }

    private Parser<PredefinedTypeSyntax> Float32Function()
    {
        return IntegerFunctionFactory("float32", PredefinedTypeKind.Float32, true);
    }

    private Parser<PredefinedTypeSyntax> Float64Function()
    {
        return IntegerFunctionFactory("float64", PredefinedTypeKind.Float64, true);
    }

    public Parser<MetadataTokenSyntax> MetadataToken()
    {
        try
        {
            var factory = IntegerFunctionFactory("mdtoken", PredefinedTypeKind.UInt32);
            return from element in factory
                   select new MetadataTokenSyntax(
                       leadingTrivia: [.. element.LeadingTrivia],
                       trailingTrivia: [.. element.TrailingTrivia],
                       value: element.Value!);
        }
        catch (ParseException ex)
        {
            return AddErrorAndGet(ex.Position, Parse.Return(new MetadataTokenSyntax([], [], "0")));
        }
    }

    internal static Parser<char> ParseSingleHexChar()
    {
        return from hexChar in Parse.Char('a').Token().Or(Parse.Char('A').Token())
                            .Or(Parse.Char('b').Token()).Or(Parse.Char('B').Token())
                            .Or(Parse.Char('c').Token()).Or(Parse.Char('C').Token())
                            .Or(Parse.Char('d').Token()).Or(Parse.Char('D').Token())
                            .Or(Parse.Char('e').Token()).Or(Parse.Char('E').Token())
                            .Or(Parse.Char('f').Token()).Or(Parse.Char('F').Token())
                            .Or(Parse.Char('0').Token()).Or(Parse.Char('1').Token())
                            .Or(Parse.Char('2').Token()).Or(Parse.Char('3').Token())
                            .Or(Parse.Char('4').Token()).Or(Parse.Char('5').Token())
                            .Or(Parse.Char('6').Token()).Or(Parse.Char('7').Token())
                            .Or(Parse.Char('8').Token()).Or(Parse.Char('9').Token())
                            .Token()
               select hexChar;
    }

    internal Parser<StringLiteralSyntax> ParseStringLiteral()
    {
        Parser<string> Escapable() =>
            from backSlash in Parse.String("\\a").Or(Parse.String("\\b")).Or(Parse.String("\\f"))
                    .Or(Parse.String("\\n")).Or(Parse.String("\\r")).Or(Parse.String("\\t"))
                    .Or(Parse.String("\\v")).Or(Parse.String("\\0")).Or(Parse.String("\\u"))
                    .Or(Parse.String("\\\\")).Or(Parse.String("\\\"")).Or(Parse.String("\\\'"))
                    .Text()
            select FormatEscapable(backSlash.Last());

        Parser<string> EncodeEscapable(char baseEscapeChar)
        {
            return baseEscapeChar switch
            {
                'u' => from utf16Sequence in EscapableUtf16UnicodeSequence() select utf16Sequence,
                'U' => from utf32Sequence in EscapableUtf32UnicodeSequence() select utf32Sequence,
                _ => Parse.Return(baseEscapeChar.ToString())
            };
        }

        string FormatEscapable(char baseEscapeChar)
        {
            return baseEscapeChar switch
            {
                'u' => $"\\u{EncodeEscapable(baseEscapeChar)}",
                'U' => $"\\U{EncodeEscapable(baseEscapeChar)}",
                _ => $"\\{baseEscapeChar}"
            };
        }

        Parser<string> EscapableUtf16UnicodeSequence() =>
            from c1 in ParseSingleHexChar()
            from c2 in ParseSingleHexChar()
            from c3 in ParseSingleHexChar()
            from c4 in ParseSingleHexChar()
            select $"{c1}{c2}{c3}{c4}";

        Parser<string> EscapableUtf32UnicodeSequence() =>
            from c1 in ParseSingleHexChar()
            from c2 in ParseSingleHexChar()
            from c3 in ParseSingleHexChar()
            from c4 in ParseSingleHexChar()
            from c5 in ParseSingleHexChar()
            from c6 in ParseSingleHexChar()
            from c7 in ParseSingleHexChar()
            from c8 in ParseSingleHexChar()
            select $"{c1}{c2}{c3}{c4}{c5}{c6}{c7}{c8}";

        Parser<string> Unescapable() =>
            from ch in Parse.Char(c => c != '"', "ParseStringLiteral()+Unescapable()")
            select ch.ToString();

        Parser<string> StringParser() =>
            from startMarker in Parse.Char('"')
            from body in Escapable().Or(Unescapable()).Until(Parse.Char('"'))
            select string.Join(string.Empty, body);

        Parser<StringLiteralSyntax> StringLiteral() =>
            from whitespace in ParseWhiteSpaceTrivia()
            from rawValue in StringParser()
            let startMarker = new StringStartTrivia()
            let endMarker = new StringEndTrivia()
            select new StringLiteralSyntax(
                [whitespace, startMarker],
                [endMarker],
                rawValue);

        try
        {
            return from literal in StringLiteral()
                   select literal;
        }
        catch (ParseException e)
        {
            m_errors.Add(new ILError(TextSpan.Convert(e.Position)));
            return Parse.Return(new StringLiteralSyntax([], [], string.Empty));
        }
    }

    internal Parser<StringLiteralSyntax> ParseSingleQuotedStringLiteral()
    {
        Parser<string> Escapable() =>
            from backSlash in Parse.String("\\a").Or(Parse.String("\\b")).Or(Parse.String("\\f"))
                    .Or(Parse.String("\\n")).Or(Parse.String("\\r")).Or(Parse.String("\\t"))
                    .Or(Parse.String("\\v")).Or(Parse.String("\\0")).Or(Parse.String("\\u"))
                    .Or(Parse.String("\\\\")).Or(Parse.String("\\\"")).Or(Parse.String("\\\'"))
                    .Text()
            select FormatEscapable(backSlash.Last());

        Parser<string> EncodeEscapable(char baseEscapeChar)
        {
            return baseEscapeChar switch
            {
                'u' => from utf16Sequence in EscapableUtf16UnicodeSequence() select utf16Sequence,
                'U' => from utf32Sequence in EscapableUtf32UnicodeSequence() select utf32Sequence,
                _ => Parse.Return(baseEscapeChar.ToString())
            };
        }

        string FormatEscapable(char baseEscapeChar)
        {
            return baseEscapeChar switch
            {
                'u' => $"\\u{EncodeEscapable(baseEscapeChar)}",
                'U' => $"\\U{EncodeEscapable(baseEscapeChar)}",
                _ => $"\\{baseEscapeChar}"
            };
        }

        Parser<string> EscapableUtf16UnicodeSequence() =>
            from c1 in ParseSingleHexChar()
            from c2 in ParseSingleHexChar()
            from c3 in ParseSingleHexChar()
            from c4 in ParseSingleHexChar()
            select $"{c1}{c2}{c3}{c4}";

        Parser<string> EscapableUtf32UnicodeSequence() =>
            from c1 in ParseSingleHexChar()
            from c2 in ParseSingleHexChar()
            from c3 in ParseSingleHexChar()
            from c4 in ParseSingleHexChar()
            from c5 in ParseSingleHexChar()
            from c6 in ParseSingleHexChar()
            from c7 in ParseSingleHexChar()
            from c8 in ParseSingleHexChar()
            select $"{c1}{c2}{c3}{c4}{c5}{c6}{c7}{c8}";

        Parser<string> Unescapable() =>
            from ch in Parse.Char(c => c != '"', "ParseSingleQuotedStringLiteral()+Unescapable()")
            select ch.ToString();

        Parser<string> StringParser() =>
            from startMarker in Parse.Char('\'')
            from body in Escapable().Or(Unescapable()).Until(Parse.Char('\''))
            select string.Join(string.Empty, body);

        Parser<StringLiteralSyntax> StringLiteral() =>
            from whitespace in ParseWhiteSpaceTrivia()
            from rawValue in StringParser()
            let startMarker = new StringStartTrivia(isDoubleQuote: false)
            let endMarker = new StringEndTrivia(isDoubleQuote: false)
            select new StringLiteralSyntax(
                [whitespace, startMarker],
                [endMarker],
                rawValue);

        try
        {
            return from literal in StringLiteral()
                   select literal;
        }
        catch (ParseException e)
        {
            m_errors.Add(new ILError(TextSpan.Convert(e.Position)));
            return Parse.Return(new StringLiteralSyntax([], [], string.Empty));
        }
    }

    internal static Parser<ByteSyntax> ParseByte()
    {
        return from whitespaceTrivia in ParseWhiteSpaceTrivia()
               from c1 in ParseSingleHexChar()
               from c2 in ParseSingleHexChar()
               select new ByteSyntax(
                   leadingTrivia: [whitespaceTrivia],
                   trailingTrivia: [],
                   value: $"{c1}{c2}");
    }

    internal static Parser<ByteSyntax> ParseByteStatic()
    {
        return from whitespaceTrivia in ParseWhiteSpaceTrivia()
               from possible0x in ParseHexPrefix()
               from c1 in from hexChar in Parse.Chars(ParserResources.HexChars.ToArray()) select hexChar
               from c2 in from hexChar in Parse.Chars(ParserResources.HexChars.ToArray()) select hexChar
               let is0xDefined = possible0x != null
               select new ByteSyntax(
                   leadingTrivia: [whitespaceTrivia],
                   trailingTrivia: is0xDefined ? [possible0x] : [],
                   value: $"{c1}{c2}");
    }

    internal static Parser<HexPrefixTrivia?> ParseHexPrefix()
    {
        return from option in Parse.String("0x").Text().Optional()
               select option.IsDefined ? new HexPrefixTrivia() : null;
    }

    internal Parser<ByteArrayKeywordTrivia> ParseByteArrayTrivia()
    {
        try
        {
            return from keyword in Parse.String("bytearray")
                   select new ByteArrayKeywordTrivia();
        }
        catch (ParseException e)
        {
            m_errors.Add(new ILError(TextSpan.Convert(e.Position)));
            return Parse.Return(new ByteArrayKeywordTrivia());
        }
    }

    internal Parser<ByteArraySyntax> ParseByteArray()
    {
        try
        {
            return from whiteSpaceTrivia in ParseWhiteSpaceTrivia()
                   from byteArrayTrivia in ParseByteArrayTrivia()
                   from whiteSpaceAfterBytearray in ParseWhiteSpaceTrivia()
                   from openParenthesis in ParseOpenParenthesis()
                   from bytes in ParseByte().Until(Parse.Char(')').Token())
                   select new ByteArraySyntax(
                       leadingTrivia: [whiteSpaceTrivia, byteArrayTrivia, whiteSpaceAfterBytearray],
                       trailingTrivia: [new CloseParenthesisTrivia()],
                       bytes: bytes);
        }
        catch (ParseException ex)
        {
            m_errors.Add(new ILError(TextSpan.Convert(ex.Position)));
            return Parse.Return(new ByteArraySyntax([], [], []));
        }
    }

    internal Parser<OpenBracketTrivia> ParseOpenBracketTrivia()
    {
        try
        {
            return from bracket in Parse.Char('[')
                   select new OpenBracketTrivia();
        }
        catch (ParseException ex)
        {
            return AddErrorAndGet(ex.Position, Parse.Return(new OpenBracketTrivia()));
        }
    }

    internal Parser<CloseBracketTrivia> ParseCloseBracketTrivia()
    {
        try
        {
            return from bracket in Parse.Char(']')
                   select new CloseBracketTrivia();
        }
        catch (ParseException ex)
        {
            return AddErrorAndGet(ex.Position, Parse.Return(new CloseBracketTrivia()));
        }
    }

    internal Parser<AssemblyReferenceSyntax> ParseAssemblyReference()
    {
        try
        {
            return from whitespaceTrivia in ParseWhiteSpaceTrivia()
                   from openBracket in ParseOpenBracketTrivia().Token()
                   from name in Parse.Char(IdentifierParseAction, ParserResources.AssemblyNameParser).AtLeastOnce().Text() /*assembly name cannot be empty*/
                   from closeBracket in ParseCloseBracketTrivia()
                   select new AssemblyReferenceSyntax(
                       leadingTrivia: [whitespaceTrivia, openBracket],
                       trailingTrivia: [closeBracket],
                       assemblyName: name);
        }
        catch (ParseException ex)
        {
            return AddErrorAndGet(ex.Position, Parse.Return(new AssemblyReferenceSyntax([], [], "<error parsing assembly name>")));
        }
    }

    private static bool IdentifierParseAction(char c)
        => char.IsLetterOrDigit(c) || c is '_' or '.' or '`' or '\\' or '/' or '+';

    internal static Parser<GenericParameterReferenceSyntax> ParseGenericParameterReference()
    {
        return from wspace in ParseWhiteSpaceTrivia()
               from banger in Parse.Char('!').AtLeastOnce().Text().Token()
               from id in Parse.LetterOrDigit.AtLeastOnce().Text()
               select new GenericParameterReferenceSyntax(banger.Length, id, [wspace], []);
    }

    internal static Parser<GenericParameterReferenceTypeSyntax> ParseGenericParameterReferenceType()
    {
        return from wspace in ParseWhiteSpaceTrivia()
               from banger in Parse.Char('!').AtLeastOnce().Text().Token()
               from id in Parse.LetterOrDigit.AtLeastOnce().Text()
               select new GenericParameterReferenceTypeSyntax([wspace], [], banger.Length, id);
    }

    internal static Parser<char> ParseVerDirectiveDigit(bool includeColonChar)
    {
        if (includeColonChar)
        {
            return from ch in Parse.Digit
                   from _ in Parse.Char(':')
                   select ch;
        }
        else
        {
            return from ch in Parse.Digit
                   select ch;
        }
    }

    internal static Parser<char> ParseHexChar()
    {
        return from chars in Parse.Chars(ParserResources.HexChars.ToArray())
               select chars;
    }

    internal static Parser<string> ParseHexNumber()
    {
        return (
                from prefix in ParseHexPrefix()
                from numbers in ParseHexChar().AtLeastOnce().Text()
                select $"0x{numbers}"
            )
            .Or(
                from numbers in ParseHexChar().AtLeastOnce().Text()
                select numbers
            );
    }

    internal Parser<VerDirectiveSyntax> ParseVerDirective()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from ver in Parse.String(".ver").Token()
                   from major in ParseVerDirectiveDigit(true)
                   from minor in ParseVerDirectiveDigit(true)
                   from build in ParseVerDirectiveDigit(true)
                   from rev in ParseVerDirectiveDigit(false)
                   select new VerDirectiveSyntax(major, minor, build, rev, [whitespace], []);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(new VerDirectiveSyntax(
                    '0', '0', '0', '0', [], [])));
        }
    }

    internal static Parser<HashAlgorithmTrivia> ParseHashAlgorithmTrivia()
    {
        return from dHashToken in Parse.String(".hash").Token()
               from algorithmToken in Parse.String("algorithm").Token()
               select new HashAlgorithmTrivia();
    }

    internal Parser<HashAlgorithmSyntax> ParseHashAlgorithm()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from trivia in ParseHashAlgorithmTrivia()
                   from id in ParseHexNumber()
                   select new HashAlgorithmSyntax(
                       leadingTrivia: [whitespace, trivia],
                       trailingTrivia: [],
                       value: id);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(new HashAlgorithmSyntax([], [], "0x00000000")));
        }
    }

    internal static Parser<SecurityAction> ParseSecurityAction()
    {
        return (
                from action in Parse.String("request")
                select SecurityAction.Request
            )
            .Or(
                from action in Parse.String("demand")
                select SecurityAction.Demand
            )
            .Or(
                from action in Parse.String("assert")
                select SecurityAction.Assert
            )
            .Or(
                from action in Parse.String("deny")
                select SecurityAction.Deny
            )
            .Or(
                from action in Parse.String("permitonly")
                select SecurityAction.PermitOnly
            )
            .Or(
                from action in Parse.String("demand")
                select SecurityAction.Demand
            )
            .Or(
                from action in Parse.String("assert")
                select SecurityAction.Assert
            )
            .Or(
                from action in Parse.String("deny")
                select SecurityAction.Deny
            )
            .Or(
                from action in Parse.String("permitonly")
                select SecurityAction.PermitOnly
            )
            .Or(
                from action in Parse.String("linkcheck")
                select SecurityAction.LinkCheck
            )
            .Or(
                from action in Parse.String("inheritcheck")
                select SecurityAction.InheritCheck
            )
            .Or(
                from action in Parse.String("reqopt")
                select SecurityAction.ReqOpt
            )
            .Or(
                from action in Parse.String("reqrefuse")
                select SecurityAction.ReqRefuse
            )
            .Or(
                from action in Parse.String("prejitgrant")
                select SecurityAction.PreJitGrant
            )
            .Or(
                from action in Parse.String("prejitdeny")
                select SecurityAction.PreJitDeny
            )
            .Or(
                from action in Parse.String("noncasdemand")
                select SecurityAction.NonCasDemand
            )
            .Or(
                from action in Parse.String("noncaslinkdemand")
                select SecurityAction.NonCasLinkDemand
            )
            .Or(
                from action in Parse.String("noncasinheritance")
                select SecurityAction.NonCasInheritance
            )
            .Or(
                from action in Parse.String("reqmin")
                select SecurityAction.ReqMin
            );
    }

    internal static Parser<PermissionSetTrivia> ParsePermissionSetTrivia()
    {
        return from permissionset in Parse.String(".permissionset")
               select new PermissionSetTrivia();
    }

    internal static Parser<PermissionSetSyntax> ParsePermissionSet()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from trivia in ParsePermissionSetTrivia()
               from algorithm in ParseSecurityAction().Token()
               from equalsChar in Parse.Char('=').Token()
               from openParenthesis in ParseOpenParenthesis().Token()
               from bytes in ParseByte().Token().Until(Parse.Char(')').Token())
               select new PermissionSetSyntax(
                   leadingTrivia: [whitespace, trivia],
                   trailingTrivia: [new CloseParenthesisTrivia()],
                   securityAction: algorithm,
                   byteArray: bytes
               );
    }

    internal static Parser<string> ParseSymbolName()
    {
        return (
                from open in Parse.Char('\'')
                from untilClose in Parse.AnyChar.Until(Parse.Char('\'')).Text() /*IL identifier names can be empty if you emit single quotes at start and end xD*/
                select untilClose
            )
            .Or(
                from id in (
                        from ch in Parse.Char(c =>
                            char.IsLetterOrDigit(c) || c == '_' || c == '\'' ||
                            c == '/' || c == '.' || c == '`', nameParseSymbolName)
                        select ch.ToString()
                    )
                    .Or(
                        from str in Parse.String("<>").Text()
                        select str
                    )
                    .Many()
                select string.Join(string.Empty, id)
            );
    }

    internal static Parser<TypePrefix?> ParseTypePrefix()
    {
        return from prefix in Parse.String("class").Or(Parse.String("valuetype")).Text()
               select prefix == "class" ? TypePrefix.Class : (TypePrefix?)TypePrefix.ValueType;
    }

    internal static Parser<KnownSpecialMethodType?> ParseSpecialMethodType()
    {
        return from name in Parse.String(".ctor").Or(Parse.String(".cctor")).Or(Parse.String("Finalize"))
               select name switch
               {
                   ".ctor" => KnownSpecialMethodType.Ctor,
                   ".cctor" => KnownSpecialMethodType.Cctor,
                   "Finalize" => KnownSpecialMethodType.Finalize,
                   _ => (KnownSpecialMethodType?)null
               };
    }

    internal Parser<NonGenericTypeReferenceSyntax> ParseNonGenericTypeReference()
    {
        return (
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from specialType in ParseSpecialMethodType().Token()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    assemblyRef,
                    typePrefix,
                    typeName,
                    specialType)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from specialType in ParseSpecialMethodType().Token()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    assemblyRef,
                    null,
                    typeName,
                    specialType)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    assemblyRef,
                    typePrefix,
                    typeName,
                    null)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    assemblyRef,
                    null,
                    typeName,
                    null)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from specialType in ParseSpecialMethodType().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    null,
                    typePrefix,
                    typeName,
                    specialType)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from specialType in ParseSpecialMethodType().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    null,
                    null,
                    typeName,
                    specialType)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    null,
                    typePrefix,
                    typeName,
                    null)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typeName in ParseSymbolName()
                from trailingTrivia in ParseTypeTrivias()
                select new NonGenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivia],
                    null,
                    null,
                    typeName,
                    null)
            );
    }

    internal static Parser<PredefinedTypeKind> ParsePredefinedTypeKind()
    {
        return from predefinedType in Parse.String("int8")
               .Or(Parse.String("int16"))
               .Or(Parse.String("int32"))
               .Or(Parse.String("int64"))
               .Or(Parse.String("uint8"))
               .Or(Parse.String("uint16"))
               .Or(Parse.String("uint32"))
               .Or(Parse.String("uint64"))
               .Or(Parse.String("float32"))
               .Or(Parse.String("float64"))
               .Or(Parse.String("char"))
               .Or(Parse.String("string"))
               .Or(Parse.String("native int"))
               .Or(Parse.String("native uint"))
               .Or(Parse.String("native int32"))
               .Or(Parse.String("native uint32"))
               .Or(Parse.String("object"))
               .Or(Parse.String("void"))
               .Or(Parse.String("boolean"))
               .Text()
               select PredefinedTypeLookups.TypeParsingLookups[predefinedType]!;

    }

    internal Parser<PredefinedTypeSyntax> ParseManyPredefinedTypes()
    {
        return from types in Int8Function()
               .Or(Int16Function())
               .Or(Int32Function())
               .Or(Int64Function())
               .Or(UInt8Function())
               .Or(UInt16Function())
               .Or(UInt32Function())
               .Or(UInt64Function())
               .Or(Float32Function())
               .Or(Float64Function())
               select types;
    }

    internal static Parser<PredefinedTypeSyntax> ParseUnParenthesizedPrimitive()
    {
        return from whitespaces in ParseWhiteSpaceTrivia()
               from predefinedType in ParsePredefinedTypeKind()
               from trailingTrivia in ParseTypeTrivias()
               select new PredefinedTypeSyntax(
                   leadingTrivia: [whitespaces],
                   trailingTrivia: [.. trailingTrivia],
                   typeName: predefinedType.ToString().ToLowerInvariant(),
                   kind: predefinedType,
                   value: null);
    }

    internal Parser<PredefinedTypeSyntax> ParsePredefinedType()
    {
        try
        {
            return ParseManyPredefinedTypes().Or(ParseUnParenthesizedPrimitive());
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(new PredefinedTypeSyntax([], [], "int32", PredefinedTypeKind.Int32, null)));
        }
    }

    internal Parser<GenericParameterPrimitiveSyntax> ParseGenericParameterPrimitive()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from predefinedType in ParsePredefinedType()
                   select new GenericParameterPrimitiveSyntax(
                       underlyingType: predefinedType,
                       leadingTrivia: [whitespace],
                       trailingTrivia: []);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new GenericParameterPrimitiveSyntax(
                        underlyingType: new PredefinedTypeSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            typeName: "int32",
                            PredefinedTypeKind.Int32,
                            null
                        ),
                        leadingTrivia: [],
                        trailingTrivia: []
                    )
                )
            );
        }
    }

    internal Parser<TypeReferenceSyntax> ParseTypeReference()
    {
        try
        {
            return ParseGenericTypeReference().Or<TypeReferenceSyntax>(ParseNonGenericTypeReference());
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    ParserResources.DummyTypeReference()
                )
            );
        }
    }

    internal static Parser<IEnumerable<SyntaxTrivia>> ParseTypeTrivias()
    {
        return (
                    from asteriskChar in Parse.Char('*')
                    select new TypeAsteriskTrivia()
                )
                .Or<SyntaxTrivia>(
                    from ampersandChar in Parse.Char('&')
                    select new TypeAmpersandTrivia()
                )
                .Or(
                    from arrayMark in Parse.String("[]")
                    select new TypeArrayTrivia()
                )
                .Many();
    }

    internal Parser<GenericTypeReferenceSyntax> ParseGenericTypeReference()
    {
        return (
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from specialType in ParseSpecialMethodType().Token()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    assemblyRef,
                    typePrefix,
                    typeName,
                    specialType,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from specialType in ParseSpecialMethodType().Token()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    assemblyRef,
                    null,
                    typeName,
                    specialType,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    assemblyRef,
                    typePrefix,
                    typeName,
                    null,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from assemblyRef in ParseAssemblyReference().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    assemblyRef,
                    null,
                    typeName,
                    null,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from specialType in ParseSpecialMethodType().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    null,
                    typePrefix,
                    typeName,
                    specialType,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from specialType in ParseSpecialMethodType().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    null,
                    null,
                    typeName,
                    specialType,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typePrefix in ParseTypePrefix().Token()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    null,
                    typePrefix,
                    typeName,
                    null,
                    args)
            )
            .Or(
                from trailingSpaces in ParseWhiteSpaceTrivia()
                from typeName in ParseSymbolName()
                from _ in Parse.Char('<')
                from args in ParseGenericArgsReference()
                from __ in Parse.Char('>')
                from trailingTrivias in ParseTypeTrivias()
                select new GenericTypeReferenceSyntax(
                    leadingTrivia: [trailingSpaces],
                    trailingTrivia: [.. trailingTrivias],
                    null,
                    null,
                    typeName,
                    null,
                    args)
            );
    }

    internal Parser<GenericArgumentsDefinitionSyntax> ParseGenericArgsDefinition()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from args in ParseGenericParameterPrimitive()
                            .Or<BaseGenericParameterSyntax>(ParseGenericParameterReference())
                            .Or(ParseGenericParameterTypeConstraint())
                            .DelimitedBy(Parse.Char(',').Token())
                   select new GenericArgumentsDefinitionSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       parameters: args);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new GenericArgumentsDefinitionSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        parameters: []
                    )
                )
            );
        }
    }

    internal Parser<GenericArgumentsReferenceSyntax> ParseGenericArgsReference()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from args in (
                        from _ in ParseWhiteSpaceTrivia() // We don't use whitespace here, we just need to skip whitespace
                        from arg in ParseGenericParameterTypeConstraint()
                           .Or<BaseGenericParameterSyntax>(ParseGenericParameterPrimitive())
                           .Or(ParseGenericParameterReference())
                           .Token()
                        from __ in Parse.Char(',').Optional().Token()
                        select arg
                   ).AtLeastOnce()
                   select new GenericArgumentsReferenceSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       parameters: args);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new GenericArgumentsReferenceSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        parameters: []
                    )
                )
            );
        }
    }

    internal Parser<GenericParameterTypeConstraintSyntax> ParseGenericParameterTypeConstraint()
    {
        try
        {
            return from whitespaces in ParseWhiteSpaceTrivia()
                   from constraint in ParseTypeReference().Token()
                   from genericArgName in Parse.Char(
                       ParserResources.GenericArgumentParseAction, nameParseGenericParameterTypeConstraint)
                        .Token().AtLeastOnce().Text().Optional()
                   select new GenericParameterTypeConstraintSyntax(
                       typeReference: constraint,
                       leadingTrivia: [whitespaces],
                       trailingTrivia: [],
                       parameterName: genericArgName.IsDefined ? genericArgName.Get() : string.Empty);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new GenericParameterTypeConstraintSyntax(
                        typeReference: ParserResources.DummyTypeReference(),
                        leadingTrivia: [],
                        trailingTrivia: [],
                        parameterName: "null"
                    )
                )
            );
        }
    }

    internal static Parser<BoolKeywordTrivia> ParseBoolTrivia()
    {
        return from keyword in Parse.String("bool")
               select new BoolKeywordTrivia();
    }

    internal Parser<BooleanLiteralSyntax> ParseBooleanLiteral()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from trueOrFalse in Parse.String("true").Or(Parse.String("false")).Text()
                   select new BooleanLiteralSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       value: trueOrFalse);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new BooleanLiteralSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        value: "false"
                    )
                )
            );
        }
    }

    internal Parser<BooleanFunctionSyntax> ParseBooleanFunction()
    {
        try
        {
            return from startWhitespace in ParseWhiteSpaceTrivia()
                   from _ in Parse.String("bool") // unused but important
                   from openParenthesis in ParseOpenParenthesis()
                   from boolLiteral in ParseBooleanLiteral()
                   from endWhitespaces in ParseWhiteSpaceTrivia()
                   from finalParenthesis in ParseCloseParenthesis()
                   select new BooleanFunctionSyntax(
                       leadingTrivia: [startWhitespace, openParenthesis],
                       trailingTrivia: [endWhitespaces, finalParenthesis],
                       value: boolLiteral);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new BooleanFunctionSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        value: new BooleanLiteralSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            value: "false"
                        )
                    )
                )
            );
        }
    }

    internal Parser<StringTypeSyntax> ParseStringType()
    {
        try
        {
            return (
                    from wspace in ParseWhiteSpaceTrivia()
                    from keyword in Parse.String("string")
                    from trivias in ParseTypeTrivias()
                    select new StringTypeSyntax(
                        leadingTrivia: [wspace],
                        trailingTrivia: [.. trivias],
                        value: null)
                )
                .Or(
                    from wspace in ParseWhiteSpaceTrivia()
                    from literal in ParseStringLiteral()
                    select new StringTypeSyntax(
                        leadingTrivia: [wspace, .. literal.LeadingTrivia],
                        trailingTrivia: [.. literal.TrailingTrivia],
                        value: literal.RawValue)
                );
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new StringTypeSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        value: null
                    )
                )
            );
        }
    }

    internal Parser<TypeSyntax> ParseWellKnownType()
    {
        try
        {
            return from type in ParseStringType()
                .Or<TypeSyntax>(ParsePredefinedType())
                .Or(ParseBooleanFunction())
                   select type;
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new PredefinedTypeSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        typeName: "int32",
                        PredefinedTypeKind.Int32,
                        value: null
                    )
                )
            );
        }
    }

    internal Parser<TypeSyntax> ParseType()
    {
        return ParseWellKnownType().Or(ParseTypeReference()).Or(ParseGenericParameterReferenceType());
    }

    internal static Parser<FieldDefinitionTrivia> ParseFieldDefinitionTrivia()
    {
        return from field in Parse.String(".field")
               select new FieldDefinitionTrivia();
    }

    internal static Parser<EqualsCharacterTrivia> ParseEqualsToken()
    {
        return from equalsChar in Parse.Char('=')
               select new EqualsCharacterTrivia();
    }

    internal static Parser<AccessModifier> ParseAccessModifier()
    {
        return (
                from modifier in Parse.String("public")
                                .Or(Parse.String("private"))
                                .Or(Parse.String("assembly"))
                                .Or(Parse.String("family"))
                                .Or(Parse.String("famorassem"))
                                .Or(Parse.String("famandassem"))
                                .Text()
                select modifier switch
                {
                    "public" => AccessModifier.Public,
                    "private" => AccessModifier.Private,
                    "assembly" => AccessModifier.Assembly,
                    "family" => AccessModifier.Family,
                    "famorassem" => AccessModifier.FamilyOrAssembly,
                    "famandassem" => AccessModifier.FamilyAndAssembly,
                    _ => AccessModifier.None
                }
            )
            .Or(
                Parse.Return(AccessModifier.None)
            );
    }

    internal Parser<FieldDeclarationSyntax> ParseFieldDeclaration()
    {
        try
        {
            return (
                    from whitespace in ParseWhiteSpaceTrivia()
                    from fieldTrivia in ParseFieldDefinitionTrivia()
                    from extraWhitespaces in ParseWhiteSpaceTrivia()
                    from accessMod in ParseAccessModifier()
                    from fieldModifiers in Parse.String("initonly").Token().Text()
                        .Or(Parse.String("static").Token().Text())
                        .Or(Parse.String("literal").Token().Text())
                        .Many()
                        .Token()
                    from marshalling in ParseMarshalFunction().Token().Optional()
                    from fieldType in ParseType().Token()
                    from id in ParseSymbolName().Token()
                    from equalToken in Parse.Char('=').Token()
                        // Auto-initialized fields in IL can only be primitive types,
                        // anything else is just initialized thru the constructor
                    from wellKnownType in ParseWellKnownType()
                    from finalWhitespaces in ParseWhiteSpaceTrivia()
                    let hasLiteralModifier = fieldModifiers.Any(mod => mod.Equals("literal"))
                    let hasStaticModifier = fieldModifiers.Any(mod => mod.Equals("static"))
                    let hasInitOnlyModifier = fieldModifiers.Any(mod => mod.Equals("initonly"))
                    let hasMarshalling = marshalling.IsDefined
                    select new FieldDeclarationSyntax(
                        leadingTrivia: [whitespace, fieldTrivia, extraWhitespaces],
                        trailingTrivia: [finalWhitespaces],
                        isLiteral: hasLiteralModifier,
                        isStatic: hasStaticModifier,
                        isInitOnly: hasInitOnlyModifier,
                        accessModifier: accessMod,
                        initialValue: wellKnownType,
                        name: id,
                        fieldType: fieldType,
                        marshalling: hasMarshalling
                        ? new FieldMarshalSyntax(
                            leadingTrivia: marshalling.Get().LeadingTrivia,
                            trailingTrivia: marshalling.Get().TrailingTrivia,
                            marshalType: marshalling.Get()
                        )
                        : null
                    )
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from fieldTrivia in ParseFieldDefinitionTrivia()
                from extraWhitespaces in ParseWhiteSpaceTrivia()
                from accessMod in ParseAccessModifier()
                from fieldModifiers in Parse.String("initonly").Token().Text()
                    .Or(Parse.String("static").Token().Text())
                    .Or(Parse.String("literal").Token().Text())
                    .Many()
                    .Token()
                from marshalling in ParseMarshalFunction().Token().Optional()
                from fieldType in ParseType().Token()
                from id in ParseSymbolName().Token()
                from finalWhitespaces in ParseWhiteSpaceTrivia()
                let hasLiteralModifier = fieldModifiers.Any(mod => mod.Equals("literal"))
                let hasStaticModifier = fieldModifiers.Any(mod => mod.Equals("static"))
                let hasInitOnlyModifier = fieldModifiers.Any(mod => mod.Equals("initonly"))
                let hasMarshalling = marshalling.IsDefined
                select new FieldDeclarationSyntax(
                    leadingTrivia: [whitespace, fieldTrivia, extraWhitespaces],
                    trailingTrivia: [finalWhitespaces],
                    isLiteral: hasLiteralModifier,
                    isStatic: hasStaticModifier,
                    isInitOnly: hasInitOnlyModifier,
                    accessModifier: accessMod,
                    initialValue: null,
                    name: id,
                    fieldType: fieldType,
                    marshalling: hasMarshalling
                        ? new FieldMarshalSyntax(
                            leadingTrivia: marshalling.Get().LeadingTrivia,
                            trailingTrivia: marshalling.Get().TrailingTrivia,
                            marshalType: marshalling.Get()
                        )
                        : null
                )
            );
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new FieldDeclarationSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        isLiteral: false,
                        isStatic: false,
                        isInitOnly: false,
                        accessModifier: AccessModifier.None,
                        initialValue: null,
                        name: FieldFallbackName,
                        fieldType: new PredefinedTypeSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            typeName: "int32",
                            kind: PredefinedTypeKind.Int32,
                            value: null
                        ),
                        marshalling: null
                    )
                )
            );
        }
    }

    internal Parser<ArgumentListReferenceSyntax> ParseArgumentListReference()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from arguments in (
                        from arg in ParseType().Token()
                        from comma in Parse.Char(',').Token().Optional()
                        select arg
                   ).Many()
                   select new ArgumentListReferenceSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       arguments: arguments);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new ArgumentListReferenceSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        []
                    )
                )
            );
        }
    }

    internal Parser<MethodInvocationSyntax> ParseMethodInvocation()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from typeReference in ParseTypeReference().Token()
                   from _ in Parse.String("::").Token()
                   from methodName in ParseSymbolName().Token()
                   from __ in ParseOpenParenthesis().Token()
                   from arguments in ParseArgumentListReference() /*() is legal in IL*/
                   from ___ in ParseCloseParenthesis().Token()
                   select new MethodInvocationSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       typeReference: typeReference,
                       methodName: methodName,
                       arguments: arguments);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new MethodInvocationSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        typeReference: ParserResources.DummyTypeReference(),
                        methodName: "<unknown>",
                        arguments: new ArgumentListReferenceSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            arguments: []
                        )
                    )
                )
            );
        }
    }

    internal Parser<MethodCallSyntax> ParseMethodCall()
    {
        try
        {
            return (
                    from wspace in ParseWhiteSpaceTrivia()
                    from instance in Parse.String("instance").Token()
                    from returnType in ParseType().Token()
                    from modifiers in ParseModifierOptional().Or<ModifierNotationSyntax>(ParseModifierRequired()).Token().Many()
                    from methodInvocation in ParseMethodInvocation()
                    select new MethodCallSyntax(
                        leadingTrivia: [wspace],
                        trailingTrivia: [],
                        isReturnInstance: true,
                        returnType: returnType,
                        methodInvocation: methodInvocation,
                        modifiers: modifiers
                    )
                )
                .Or(
                    from wspace in ParseWhiteSpaceTrivia()
                    from returnType in ParseType().Token()
                    from modifiers in ParseModifierOptional().Or<ModifierNotationSyntax>(ParseModifierRequired()).Token().Many()
                    from methodInvocation in ParseMethodInvocation()
                    select new MethodCallSyntax(
                        leadingTrivia: [wspace],
                        trailingTrivia: [],
                        isReturnInstance: false,
                        returnType: returnType,
                        methodInvocation: methodInvocation,
                        modifiers: modifiers
                    )
                );
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    ParserResources.DummyMethodCall()
                )
            );
        }
    }

    internal static Parser<CustomDirectiveKeywordTrivia> ParseCustomDirectiveKeywordTrivia()
    {
        return from custom in Parse.String(".custom").Token()
               select new CustomDirectiveKeywordTrivia();
    }

    internal Parser<CustomAttributeSyntax> ParseCustomAttributeWithData()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from custom in ParseCustomDirectiveKeywordTrivia().Token()
               from callee in ParseMethodCall().Token()
               from equalsToken in Parse.Char('=').Token()
               from openParenToken in Parse.Char('(').Token()
               from bytes in ParseByte().Until(Parse.Char(')').Token())
               select new CustomAttributeSyntax(
                   leadingTrivia: [whitespace, custom],
                   trailingTrivia: [],
                   attributeData: bytes,
                   attributeConstructorTarget: callee
               );
    }

    internal Parser<AnonymousCustomAttributeSyntax> ParseAnonymousCustomAttribute()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from custom in ParseCustomDirectiveKeywordTrivia().Token()
               from callee in ParseMethodCall()
               select new AnonymousCustomAttributeSyntax(
                   leadingTrivia: [whitespace, custom],
                   trailingTrivia: [],
                   attributeConstructorTarget: callee
               );
    }

    internal Parser<BaseCustomAttributeSyntax> ParseCustomAttribute()
    {
        return ParseCustomAttributeWithData()
            .Or<BaseCustomAttributeSyntax>(ParseAnonymousCustomAttribute());
    }

    internal static Parser<InlineCommentStartTrivia> ParseInlineCommentStart() =>
        from start in Parse.String("//")
        select new InlineCommentStartTrivia();

    internal static Parser<MultilineCommentStartTrivia> ParseMultilineCommentStart() =>
        from start in Parse.String("/*")
        select new MultilineCommentStartTrivia();

    internal static Parser<MultilineCommentEndTrivia> ParseMultilineCommentEnd() =>
        from end in Parse.String("*/")
        select new MultilineCommentEndTrivia();

    internal static Parser<InlineCommentSyntax> ParseInlineComment()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from startTrivia in ParseInlineCommentStart()
               from text in Parse.AnyChar.Until(Parse.LineEnd).Text()
               select new InlineCommentSyntax(
                   leadingTrivia: [whitespace, startTrivia],
                   trailingTrivia: [],
                   commentText: text);
    }

    internal static Parser<MultilineCommentSyntax> ParseMultilineComment()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from startTrivia in ParseMultilineCommentStart()
               from text in Parse.AnyChar.Until(Parse.String("*/")).Text()
               select new MultilineCommentSyntax(
                   leadingTrivia: [whitespace, startTrivia],
                   trailingTrivia: [new MultilineCommentEndTrivia()],
                   commentText: text);
    }

    internal static Parser<BaseCommentSyntax> ParseComment()
    {
        return ParseInlineComment().Or<BaseCommentSyntax>(ParseMultilineComment());
    }

    internal static Parser<ParameterlessOpCodeSyntax> ParseParameterlessOpCode()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("add")
                    .Or(Parse.String("add.ovf"))
                    .Or(Parse.String("add.ovf.un"))
                    .Or(Parse.String("and"))
                    .Or(Parse.String("arglist"))
                    .Or(Parse.String("break"))
                    .Or(Parse.String("ckfinite"))
                    .Or(Parse.String("cpblk"))
                    .Or(Parse.String("div"))
                    .Or(Parse.String("div.un"))
                    .Or(Parse.String("dup"))
                    .Or(Parse.String("endfault"))
                    .Or(Parse.String("endfinally"))
                    .Or(Parse.String("endfilter"))
                    .Or(Parse.String("initblk"))
                    .Or(Parse.String("ldlen"))
                    .Or(Parse.String("ldnull"))
                    .Or(Parse.String("localloc"))
                    .Or(Parse.String("mul"))
                    .Or(Parse.String("mul.ovf"))
                    .Or(Parse.String("mul.ovf.un"))
                    .Or(Parse.String("neg"))
                    .Or(Parse.String("nop"))
                    .Or(Parse.String("not"))
                    .Or(Parse.String("or"))
                    .Or(Parse.String("pop"))
                    .Or(Parse.String("refanytype"))
                    .Or(Parse.String("ret"))
                    .Or(Parse.String("rem"))
                    .Or(Parse.String("rem.ovf"))
                    .Or(Parse.String("rem.ovf.un"))
                    .Or(Parse.String("rethrow"))
                    .Or(Parse.String("shl"))
                    .Or(Parse.String("shr"))
                    .Or(Parse.String("shr.un"))
                    .Or(Parse.String("sub"))
                    .Or(Parse.String("sub.ovf"))
                    .Or(Parse.String("sub.ovf.un"))
                    .Or(Parse.String("throw"))
                    .Or(Parse.String("xor"))
                    .Text()
               select new ParameterlessOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction);
    }

    internal static Parser<BranchOpCodeSyntax> ParseBranchOpCode()
    {
        static Parser<string> S(string name)
        {
            return from instructionName in Parse.String(name).Text()
                   from dot in Parse.Char('.')
                   from s in Parse.Char('s')
                   select instructionName + ".s";
        }

        static Parser<string> Un(string name)
        {
            return from instructionName in Parse.String(name).Text()
                   from dot in Parse.Char('.')
                   from s in Parse.String("un")
                   select instructionName + ".un";
        }

        static Parser<string> UnS(string name)
        {
            return from instructionName in Parse.String(name).Text()
                   from dot in Parse.Char('.')
                   from s in Parse.Char('s')
                   from dot2 in Parse.Char('.')
                   from un in Parse.String("un")
                   select instructionName + ".un.s";
        }

        static Parser<string> Simple(string name) => from instr in Parse.String(name) select name;

        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in S("beq")
                        .Or(Simple("beq"))
                        .Or(UnS("bge.un.s")) /*bge*/
                        .Or(Un("bge"))
                        .Or(S("bge"))
                        .Or(Parse.String("bge"))
                        .Or(Parse.String("bgt.s")) /*bgt*/
                        .Or(Parse.String("bgt.un.s"))
                        .Or(Parse.String("bgt.un"))
                        .Or(Parse.String("bgt"))
                        .Or(Parse.String("ble.un.s")) /*ble*/
                        .Or(Parse.String("ble.un"))
                        .Or(Parse.String("ble.s"))
                        .Or(Parse.String("ble"))
                        .Or(Parse.String("blt.un.s")) /*blt*/
                        .Or(Parse.String("blt.un"))
                        .Or(Parse.String("blt.s"))
                        .Or(Parse.String("blt"))
                        .Or(Parse.String("bne.un.s")) /*bne.un*/
                        .Or(Parse.String("bne.un"))
                        .Or(Parse.String("br.s")) /*br*/
                        .Or(Parse.String("br"))
                        .Or(Parse.String("brfalse.s")) /*brfalse*/
                        .Or(Parse.String("brfalse"))
                        .Or(Parse.String("brinst.s"))
                        .Or(Parse.String("brinst"))
                        .Or(Parse.String("brnull.s"))
                        .Or(Parse.String("brnull"))
                        .Or(Parse.String("brtrue.s"))
                        .Or(Parse.String("brtrue"))
                        .Or(Parse.String("brzero.s"))
                        .Or(Parse.String("brzero"))
                        .Token()
                        .Text()
               from _ in ParseWhiteSpaceTrivia()
               from target in Parse.Char(ParserResources.LabelParseAction, "ParseBranchOpCode()")
                            .AtLeastOnce().Text()
               select new BranchOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction,
                   targetLabel: target);
    }

    internal static Parser<ComparisonOpCodeSyntax> ParseComparisonOpCode()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("ceq")
                            .Or(Parse.String("cgt"))
                            .Or(Parse.String("cgt.un"))
                            .Or(Parse.String("clt"))
                            .Or(Parse.String("clt.un"))
                            .Text()
               select new ComparisonOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction);
    }

    internal static Parser<ConversionOpCodeSyntax> ParseConversionOpCode()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("conv.i")
                            .Or(Parse.String("conv.i1"))
                            .Or(Parse.String("conv.i2"))
                            .Or(Parse.String("conv.i4"))
                            .Or(Parse.String("conv.i8"))
                            .Or(Parse.String("conv.u"))
                            .Or(Parse.String("conv.u1"))
                            .Or(Parse.String("conv.u2"))
                            .Or(Parse.String("conv.u4"))
                            .Or(Parse.String("conv.u8"))
                            .Or(Parse.String("conv.ovf.i"))
                            .Or(Parse.String("conv.ovf.i.un"))
                            .Or(Parse.String("conv.ovf.i1"))
                            .Or(Parse.String("conv.ovf.i1.un"))
                            .Or(Parse.String("conv.ovf.i2"))
                            .Or(Parse.String("conv.ovf.i2.un"))
                            .Or(Parse.String("conv.ovf.i4"))
                            .Or(Parse.String("conv.ovf.i4.un"))
                            .Or(Parse.String("conv.ovf.i8"))
                            .Or(Parse.String("conv.ovf.i8.un"))
                            .Or(Parse.String("conv.ovf.u"))
                            .Or(Parse.String("conv.ovf.u.un"))
                            .Or(Parse.String("conv.ovf.u1"))
                            .Or(Parse.String("conv.ovf.u1.un"))
                            .Or(Parse.String("conv.ovf.u2"))
                            .Or(Parse.String("conv.ovf.u2.un"))
                            .Or(Parse.String("conv.ovf.u4"))
                            .Or(Parse.String("conv.ovf.u4.un"))
                            .Or(Parse.String("conv.ovf.u8"))
                            .Or(Parse.String("conv.ovf.u8.un"))
                            .Or(Parse.String("conv.r.un"))
                            .Or(Parse.String("conv.r4"))
                            .Or(Parse.String("conv.r8"))
                            .Text()
               select new ConversionOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction);
    }

    internal Parser<LoadElementOpCodeSyntax> ParseLoadElementOpCode()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldelem.i")
                            .Or(Parse.String("ldelem.i1"))
                            .Or(Parse.String("ldelem.i2"))
                            .Or(Parse.String("ldelem.i4"))
                            .Or(Parse.String("ldelem.i8"))
                            .Or(Parse.String("ldelem.r4"))
                            .Or(Parse.String("ldelem.r8"))
                            .Or(Parse.String("ldelem.ref"))
                            .Or(Parse.String("ldelem.u1"))
                            .Or(Parse.String("ldelem.u2"))
                            .Or(Parse.String("ldelem.u4"))
                            .Or(Parse.String("ldelem.u8"))
                            .Text()
                select new LoadElementOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    elementType: null)
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldelem").Or(Parse.String("ldelema")).Token().Text()
                from type in ParseType()
                select new LoadElementOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    elementType: type)
            );
    }

    internal Parser<PushNumberToStackOpCodeSyntax> ParsePushNumberToStackOpCode()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldc.i4.0")
                            .Or(Parse.String("ldc.i4.1"))
                            .Or(Parse.String("ldc.i4.2"))
                            .Or(Parse.String("ldc.i4.3"))
                            .Or(Parse.String("ldc.i4.4"))
                            .Or(Parse.String("ldc.i4.5"))
                            .Or(Parse.String("ldc.i4.6"))
                            .Or(Parse.String("ldc.i4.7"))
                            .Or(Parse.String("ldc.i4.8"))
                            .Or(Parse.String("ldc.i4.m1")) // M1 = m1
                            .Or(Parse.String("ldc.i4.M1")) // m1 = M1
                            .Text()
                select new PushNumberToStackOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    parameter: null)
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldc.i4")
                            .Or(Parse.String("ldc.i4.s"))
                            .Or(Parse.String("ldc.i8"))
                            .Or(Parse.String("ldc.r4"))
                            .Or(Parse.String("ldc.r8"))
                            .Text()
                            .Token()
                from parameter in ParsePredefinedType() /*Those instructions only accept numbers/floats*/
                select new PushNumberToStackOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    parameter: parameter)
            );
    }

    internal static Parser<LoadIndirectOpCodeSyntax> ParseLoadIndirectOpCode()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("ldind.i")
                            .Or(Parse.String("ldind.i1"))
                            .Or(Parse.String("ldind.i2"))
                            .Or(Parse.String("ldind.i4"))
                            .Or(Parse.String("ldind.i8"))
                            .Or(Parse.String("ldind.u1"))
                            .Or(Parse.String("ldind.u2"))
                            .Or(Parse.String("ldind.u4"))
                            .Or(Parse.String("ldind.u8"))
                            .Or(Parse.String("ldind.r4"))
                            .Or(Parse.String("ldind.r8"))
                            .Or(Parse.String("ldind.ref"))
                            .Text()
               select new LoadIndirectOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction);
    }

    internal static Parser<LoadLocalOpCodeSyntax> ParseLoadLocalOpCode()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldloc.0")
                            .Or(Parse.String("ldloc.1"))
                            .Or(Parse.String("ldloc.2"))
                            .Or(Parse.String("ldloc.3"))
                            .Text()
                select new LoadLocalOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    index: uint.Parse(instruction.Split('.').Last())
                )
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldloc")
                            .Or(Parse.String("ldloc.s"))
                            .Or(Parse.String("ldloca"))
                            .Or(Parse.String("ldloca.s"))
                            .Text()
                            .Token()
                from index in Parse.Digit.AtLeastOnce().Text()
                select new LoadLocalOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    index: uint.Parse(index))
            );
    }

    internal Parser<StoreElementOpCodeSyntax> ParseStoreElementOpCode()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("stelem.i")
                            .Or(Parse.String("stelem.i1"))
                            .Or(Parse.String("stelem.i2"))
                            .Or(Parse.String("stelem.i4"))
                            .Or(Parse.String("stelem.i8"))
                            .Or(Parse.String("stelem.r4"))
                            .Or(Parse.String("stelem.r8"))
                            .Or(Parse.String("stelem.ref"))
                            .Or(Parse.String("stelem.u1"))
                            .Or(Parse.String("stelem.u2"))
                            .Or(Parse.String("stelem.u4"))
                            .Or(Parse.String("stelem.u8"))
                            .Text()
                select new StoreElementOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    elementType: null)
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("stelem").Token().Text()
                from type in ParseType()
                select new StoreElementOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    elementType: type)
            );
    }

    internal static Parser<StoreLocalOpCodeSyntax> ParseStoreLocalOpCode()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("stloc.0")
                            .Or(Parse.String("stloc.1"))
                            .Or(Parse.String("stloc.2"))
                            .Or(Parse.String("stloc.3"))
                            .Text()
                select new StoreLocalOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    index: uint.Parse(instruction.Split('.').Last())
                )
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("stloc")
                            .Or(Parse.String("stloc.s"))
                            .Text()
                            .Token()
                from index in Parse.Digit.AtLeastOnce().Text()
                select new StoreLocalOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    index: uint.Parse(index))
            );
    }

    internal static Parser<StoreIndirectOpCodeSyntax> ParseStoreIndirectOpCode()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("stind.i")
                            .Or(Parse.String("stind.i1"))
                            .Or(Parse.String("stind.i2"))
                            .Or(Parse.String("stind.i4"))
                            .Or(Parse.String("stind.i8"))
                            .Or(Parse.String("stind.r4"))
                            .Or(Parse.String("stind.r8"))
                            .Or(Parse.String("stind.ref"))
                            .Text()
               select new StoreIndirectOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction);
    }

    internal static Parser<ArgumentLoadOpCodeSyntax> ParseArgumentLoadOpCode()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldarg.0")
                            .Or(Parse.String("ldarg.1"))
                            .Or(Parse.String("ldarg.2"))
                            .Or(Parse.String("ldarg.3"))
                            .Text()
                select new ArgumentLoadOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    index: null)
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from instruction in Parse.String("ldarg")
                            .Or(Parse.String("ldarg.s"))
                            .Or(Parse.String("ldarga"))
                            .Or(Parse.String("ldarga.s"))
                            .Text()
                from target in Parse.Digit.AtLeastOnce().Text()
                select new ArgumentLoadOpCodeSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    name: instruction,
                    index: ushort.Parse(target))
            );
    }

    internal static Parser<ArgumentStoreOpCodeSyntax> ParseArgumentStoreOpCode()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("starg").Or(Parse.String("starg.s")).Text().Token()
               from index in Parse.Digit.AtLeastOnce().Text()
               select new ArgumentStoreOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   name: instruction,
                   index: ushort.Parse(index));
    }

    internal static Parser<ModoptKeywordTrivia> ParseModOptTrivia()
    {
        return from str in Parse.String("modopt")
               select new ModoptKeywordTrivia();
    }

    internal static Parser<ModreqKeywordTrivia> ParseModReqTrivia()
    {
        return from str in Parse.String("modreq")
               select new ModreqKeywordTrivia();
    }

    internal Parser<ModOptSyntax> ParseModifierOptional()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from modopt in ParseModOptTrivia().Token()
               from openParen in ParseOpenParenthesis().Token()
               from type in ParseTypeReference().Token()
               from closeParen in ParseCloseParenthesis()
               select new ModOptSyntax(
                   leadingTrivia: [whitespace, modopt],
                   trailingTrivia: [closeParen],
                   typeReference: type);
    }

    internal Parser<ModReqSyntax> ParseModifierRequired()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from modopt in ParseModReqTrivia().Token()
               from openParen in ParseOpenParenthesis().Token()
               from type in ParseTypeReference().Token()
               from closeParen in ParseCloseParenthesis()
               select new ModReqSyntax(
                   leadingTrivia: [whitespace, modopt],
                   trailingTrivia: [closeParen],
                   typeReference: type);
    }

    internal static Parser<MethodKeywordTrivia> ParseMethodKeywordTrivia()
    {
        return from str in Parse.String("method")
               select new MethodKeywordTrivia();
    }

    internal static Parser<Management> ParseFunctionPointerManagement()
    {
        return from managedOrUnmanaged in Parse.String("managed").Or(Parse.String("unmanaged"))
                            .Text()
               select managedOrUnmanaged is "managed" ? Management.Managed : Management.Unmanaged;
    }

    internal static Parser<CallKind> ParseCallKind()
    {
        return from kindString in Parse.Letter.AtLeastOnce().Text()
               select kindString switch
               {
                   "cdecl" => CallKind.Cdecl,
                   "thiscall" => CallKind.ThisCall,
                   "fastcall" => CallKind.FastCall,
                   "stdcall" => CallKind.StdCall,
                   _ => CallKind.Default /*default calling convention if unrecognized*/
               };
    }

    internal Parser<FunctionPointerSyntax> ParseFunctionPointer()
    {
        return (
                   from whitespace in ParseWhiteSpaceTrivia()
                   from methodKeyword in ParseMethodKeywordTrivia().Token()
                   from management in ParseFunctionPointerManagement().Token()
                   from returnType in ParseType().Token()
                   from modifiers in ParseModifierOptional()
                        .Or<ModifierNotationSyntax>(ParseModifierRequired()).Many().Token() /*modopt/modreq for function pointer is not strict*/
                   from asterisk in Parse.Char('*').Token()
                   from openFunc in ParseOpenParenthesis().Token()
                   from arguments in (
                        from argument in ParseType().Token()
                        from optionalComma in Parse.Char(',').Token().Optional()
                        select argument
                   ).Many()
                   from closeFunc in ParseCloseParenthesis().Token()
                   select new FunctionPointerSyntax(
                       leadingTrivia: [whitespace, methodKeyword],
                       trailingTrivia: [closeFunc],
                       returnType: returnType,
                       arguments: arguments.ToImmutableArray(),
                       management: management,
                       callingConvention: CallKind.Default,
                       modifiers: modifiers)
            )
            .Or(
                 from whitespace in ParseWhiteSpaceTrivia()
                 from methodKeyword in ParseMethodKeywordTrivia().Token()
                 from management in ParseFunctionPointerManagement().Token()
                 from call in ParseCallKind().Token()
                 from returnType in ParseType().Token()
                 from asterisk in Parse.Char('*').Token()
                 from openFunc in ParseOpenParenthesis().Token()
                 from arguments in (
                      from argument in ParseType().Token()
                      from optionalComma in Parse.Char(',').Token().Optional()
                      select argument
                 ).Many()
                 from closeFunc in ParseCloseParenthesis().Token()
                 select new FunctionPointerSyntax(
                     leadingTrivia: [whitespace, methodKeyword],
                     trailingTrivia: [closeFunc],
                     returnType: returnType,
                     arguments: arguments.ToImmutableArray(),
                     management: management,
                     callingConvention: call,
                     modifiers: [])
            );
    }

    internal Parser<ManagedFunctionPointerInvocationSyntax> ParseManagedFunctionPointerInvocation()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from returnType in ParseType().Token()
               from openParen in ParseOpenParenthesis().Token()
               from arguments in (
                    from arg in ParseType().Token()
                    from optionalComma in Parse.Char(',').Token().Optional()
                    from _ in ParseWhiteSpaceTrivia()
                    select arg
               ).Many()
               from closeParen in ParseCloseParenthesis().Token()
               select new ManagedFunctionPointerInvocationSyntax(
                   leadingTrivia: [whitespace, openParen],
                   trailingTrivia: [closeParen],
                   returnType: returnType,
                   parameters: arguments);
    }

    internal static Parser<UnmanagedKeywordTrivia> ParseUnmanagedKeyword()
    {
        return from keyword in Parse.String("unmanaged")
               select new UnmanagedKeywordTrivia();
    }

    internal Parser<UnmanagedFunctionPointerInvocationSyntax> ParseUnmanagedFunctionPointerInvocation()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from unmanagedNotation in ParseUnmanagedKeyword().Token()
               from callingConvention in ParseCallKind().Token()
                   // HACK: we'll parse managed function pointer invocation under unmanaged
                   // one to parse the actual invocation body (f.e. 'int32(int32, int32)')
                   // in order to not write too much boilerplate code, we don't want
                   // to include 'parsing time info' also
               from managed in ParseManagedFunctionPointerInvocation()
               select new UnmanagedFunctionPointerInvocationSyntax(
                   leadingTrivia: [whitespace, unmanagedNotation],
                   trailingTrivia: [],
                   parameters: managed.Parameters,
                   returnType: managed.ReturnType,
                   callingConv: callingConvention);
    }

    internal Parser<FunctionPointerInvocationSyntax> ParseFunctionPointerInvocation()
    {
        try
        {
            return ParseUnmanagedFunctionPointerInvocation()
                .Or<FunctionPointerInvocationSyntax>(ParseManagedFunctionPointerInvocation());
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new ManagedFunctionPointerInvocationSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        returnType: new PredefinedTypeSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            "int32",
                            PredefinedTypeKind.Int32,
                            null
                        ),
                        parameters: []
                    )
                )
            );
        }
    }

    internal Parser<FieldOrPropertyReferenceSyntax> ParseFieldOrPropertyReference()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from typeRef in ParseTypeReference().Token()
                   from prefix in ParseTypeReference().Token()
                   from coloncolon in Parse.String("::").Token()
                   from symbolName in ParseSymbolName()
                   select new FieldOrPropertyReferenceSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       fieldType: typeRef,
                       objectName: symbolName,
                       declaringTypeReference: prefix);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new FieldOrPropertyReferenceSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        declaringTypeReference: ParserResources.DummyTypeReference(),
                        objectName: "<unknown>",
                        fieldType: ParserResources.DummyTypeReference()
                    )
                )
            );
        }
    }

    internal Parser<BoxOpCodeSyntax> ParseBoxInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from box in Parse.String("box").Token()
               from target in ParseType()
               select new BoxOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   boxedType: target);
    }

    internal Parser<CalliOpCodeSyntax> ParseCalliInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from box in Parse.String("calli").Token()
               from target in ParseFunctionPointerInvocation()
               select new CalliOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   functionPointer: target);
    }

    internal Parser<CallOpCodeSyntax> ParseCallInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from call in Parse.String("call").Token()
               from invocation in ParseMethodCall()
               select new CallOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   method: invocation);
    }

    internal Parser<CallvirtOpCodeSyntax> ParseCallvirtInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from call in Parse.String("callvirt").Token()
               from invocation in ParseMethodInvocation()
               select new CallvirtOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: invocation);
    }

    internal Parser<CastclassOpCodeSyntax> ParseCastclassInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from castclass in Parse.String("castclass").Token()
               from target in ParseType()
               select new CastclassOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: target);
    }

    internal Parser<CpobjOpCodeSyntax> ParseCpobjInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from castclass in Parse.String("cpobj").Token()
               from target in ParseType()
               select new CpobjOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: target);
    }

    internal Parser<InitobjOpCodeSyntax> ParseInitobjInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from castclass in Parse.String("initobj").Token()
               from target in ParseType()
               select new InitobjOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: target);
    }

    internal Parser<IsinstOpCodeSyntax> ParseIsinstInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from castclass in Parse.String("isinst").Token()
               from target in ParseType()
               select new IsinstOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: target);
    }

    internal Parser<JmpOpCodeSyntax> ParseJmpInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from jmp in Parse.String("jmp").Token()
               from target in ParseMethodCall()
               select new JmpOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   method: target);
    }

    internal Parser<LdfldaOpCodeSyntax> ParseLdfldaInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldflda").Token()
               from fieldType in ParseType().Token()
               from reference in ParseFieldOrPropertyReference()
               select new LdfldaOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fieldType: fieldType,
                   target: reference);
    }

    internal Parser<LdfldOpCodeSyntax> ParseLdfldInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldfld").Token()
               from fieldType in ParseType().Token()
               from reference in ParseFieldOrPropertyReference()
               select new LdfldOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fieldType: fieldType,
                   target: reference);
    }

    internal Parser<LdsfldaOpCodeSyntax> ParseLdsfldaInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldsflda").Token()
               from fieldType in ParseType().Token()
               from reference in ParseFieldOrPropertyReference()
               select new LdsfldaOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fieldType: fieldType,
                   target: reference);
    }

    internal Parser<LdsfldOpCodeSyntax> ParseLdsfldInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldsfld").Token()
               from reference in ParseFieldOrPropertyReference()
               select new LdsfldOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fieldType: reference.FieldType,
                   target: reference);
    }

    internal Parser<LdftnOpCodeSyntax> ParseLdftnInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldftn").Token()
               from invocation in ParseMethodInvocation()
               select new LdftnOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: invocation);
    }

    internal Parser<LdstrOpCodeSyntax> ParseLdstrInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldstr").Token()
               from str in ParseStringLiteral()
               select new LdstrOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   @string: str);
    }

    internal Parser<LdvirtftnOpCodeSyntax> ParseLdvirtftnInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("ldvirtftn").Token()
               from target in ParseMethodCall()
               select new LdvirtftnOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: target);
    }

    internal static Parser<LeaveOpCodeSyntax> ParseLeaveInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from leave in Parse.String("leave").Token()
               from target in Parse
                    .Char((c) => char.IsLetterOrDigit(c) || c == '_', "ParseLeaveInstruction()")
                    .AtLeastOnce()
                    .Text()
               select new LeaveOpCodeSyntax(
                   leadingTrivia: [],
                   trailingTrivia: [],
                   target: target);
    }

    internal static Parser<LeaveSOpCodeSyntax> ParseLeaveSInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from leave in Parse.String("leave.s").Token()
               from target in Parse
                    .Char((c) => char.IsLetterOrDigit(c) || c == '_', "ParseLeaveSInstruction()")
                    .AtLeastOnce()
                    .Text()
               select new LeaveSOpCodeSyntax(
                   leadingTrivia: [],
                   trailingTrivia: [],
                   target: target);
    }

    internal Parser<MkrefanyOpCodeSyntax> ParseMkrefanyInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from instruction in Parse.String("mkrefany").Token()
               from boxedType in ParseType()
               select new MkrefanyOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   boxedType: boxedType);
    }

    internal Parser<NewarrOpCodeSyntax> ParseNewarrInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from newarr in Parse.String("newarr").Token()
               from arrayType in ParseType()
               select new NewarrOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   arrayType: arrayType);
    }

    internal Parser<NewobjOpCodeSyntax> ParseNewobjInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from newobj in Parse.String("newobj").Token()
               from constructorInvocation in ParseMethodCall()
               select new NewobjOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   constructor: constructorInvocation);
    }

    internal Parser<RefanyvalOpCodeSyntax> ParseRefanyvalInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from refanyval in Parse.String("refanyval").Token()
               from type in ParseType()
               select new RefanyvalOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   type: type);
    }

    internal Parser<SizeofOpCodeSyntax> ParseSizeofInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from refanyval in Parse.String("sizeof").Token()
               from type in ParseType()
               select new SizeofOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   type: type);
    }

    internal Parser<StfldOpCodeSyntax> ParseStfldInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("stfld").Token()
               from fieldType in ParseType().Token()
               from reference in ParseFieldOrPropertyReference()
               select new StfldOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fieldType: fieldType,
                   target: reference);
    }

    internal Parser<StobjOpCodeSyntax> ParseStobjInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from refanyval in Parse.String("stobj").Token()
               from type in ParseType()
               select new StobjOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   type: type);
    }

    internal Parser<StsfldOpCodeSyntax> ParseStsfldInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from ldflda in Parse.String("stsfld").Token()
               from fieldType in ParseType().Token()
               from reference in ParseFieldOrPropertyReference()
               select new StsfldOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fieldType: fieldType,
                   target: reference);
    }

    internal static Parser<SwitchInstructionBodySyntax> ParseSwitchInstructionBody()
    {
        return from wspace in ParseWhiteSpaceTrivia()
               from openParen in Parse.Char('(').Token()
               from labels in (
                    from label in Parse
                        .Char(c => char.IsLetterOrDigit(c) || c == '_', "ParseSwitchInstructionBody()")
                        .AtLeastOnce()
                        .Text()
                        .Token()
                    from commaOptional in Parse.Char(',').Optional().Token()
                    select label
                ).Many()
               from closeParen in Parse.Char(')')
               select new SwitchInstructionBodySyntax(
                   leadingTrivia: [wspace],
                   trailingTrivia: [],
                   labels: labels);
    }

    internal static Parser<SwitchOpCodeSyntax> ParseSwitchInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from @switch in Parse.String("switch").Token()
               from body in ParseSwitchInstructionBody()
               select new SwitchOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   instructionBody: body);
    }

    internal Parser<UnboxAnyOpCodeSyntax> ParseUnboxAnyInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from unbox in Parse.String("unbox.any").Token()
               from type in ParseType()
               select new UnboxAnyOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   type: type);
    }

    internal Parser<UnboxOpCodeSyntax> ParseUnboxInstruction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from unbox in Parse.String("unbox").Token()
               from type in ParseType()
               select new UnboxOpCodeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   type: type);
    }

    internal Parser<InstructionSyntax> ParseInstruction()
    {
        try
        {
            return from instruction in ParseBranchOpCode()
                             .Or<InstructionSyntax>(ParseArgumentStoreOpCode())
                             .Or(ParseArgumentLoadOpCode())
                             .Or(ParseComparisonOpCode())
                             .Or(ParseConversionOpCode())
                             .Or(ParseLoadElementOpCode())
                             .Or(ParseLoadIndirectOpCode())
                             .Or(ParseLoadLocalOpCode())
                             .Or(ParseParameterlessOpCode())
                             .Or(ParsePushNumberToStackOpCode())
                             .Or(ParseStoreElementOpCode())
                             .Or(ParseStoreIndirectOpCode())
                             .Or(ParseStoreLocalOpCode())
                             .Or(ParseBoxInstruction())
                             .Or(ParseCalliInstruction())
                             .Or(ParseCallInstruction())
                             .Or(ParseCallvirtInstruction())
                             .Or(ParseCpobjInstruction())
                             .Or(ParseCpobjInstruction())
                             .Or(ParseInitobjInstruction())
                             .Or(ParseIsinstInstruction())
                             .Or(ParseJmpInstruction())
                             .Or(ParseLdfldaInstruction())
                             .Or(ParseLdfldInstruction())
                             .Or(ParseLdftnInstruction())
                             .Or(ParseLdsfldaInstruction())
                             .Or(ParseLdsfldInstruction())
                             .Or(ParseLdstrInstruction())
                             .Or(ParseLdvirtftnInstruction())
                             .Or(ParseLeaveInstruction())
                             .Or(ParseLeaveSInstruction())
                             .Or(ParseMkrefanyInstruction())
                             .Or(ParseNewarrInstruction())
                             .Or(ParseNewobjInstruction())
                             .Or(ParseRefanyvalInstruction())
                             .Or(ParseSizeofInstruction())
                             .Or(ParseStfldInstruction())
                             .Or(ParseStobjInstruction())
                             .Or(ParseStsfldInstruction())
                             .Or(ParseSwitchInstruction())
                             .Or(ParseUnboxAnyInstruction())
                             .Or(ParseUnboxInstruction())
                   select instruction;
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new ParameterlessOpCodeSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        name: "/*Unknown opcode, defaulting to nop*/ nop"
                    )
                )
            );
        }
    }

    internal static Parser<MaxStackDirectiveSyntax> ParseMaxStackDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from maxstack in Parse.String(".maxstack").Token()
               from depth in Parse.Digit.AtLeastOnce().Text()
               select new MaxStackDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   depth);
    }

    internal static Parser<EntryPointDirectiveSyntax> ParseEntryPointDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from entrypoint in Parse.String(".entrypoint")
               select new EntryPointDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: []);
    }

    internal static Parser<CorFlagsDirectiveSyntax> ParseCorFlagsDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from corflags in Parse.String(".corflags").Token()
               from value in ParseHexNumber()
               select new CorFlagsDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   flags: value);
    }

    internal static Parser<SubsystemDirectiveSyntax> ParseSubsystemDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from corflags in Parse.String(".subsystem").Token()
               from value in ParseHexNumber()
               select new SubsystemDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   subsystem: value);
    }

    internal static Parser<StackReserveDirectiveSyntax> ParseStackReserveDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from corflags in Parse.String(".stackreserve").Token()
               from value in ParseHexNumber()
               select new StackReserveDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   stackReserve: value);
    }

    internal Parser<CustomMarshalTypeSyntax> ParseCustomMarshalType()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from custom in Parse.String("custom").Token()
               from openParen in ParseOpenParenthesis().Token()
               from typeRef in ParseStringLiteral().Token()
               from comma in Parse.Char(',').Token()
               from marshalCookie in ParseStringLiteral().Token()
               from closeParen in ParseCloseParenthesis().Token()
               select new CustomMarshalTypeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   value: "custom",
                   assemblyString: typeRef.RawValue,
                   cookie: marshalCookie.RawValue);
    }

    internal Parser<ByValTStrMarshalTypeSyntax> ParseByValTStrMarshalType()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from fixedSysString in Parse.String("fixed sysstring").Token()
               from openBracket in ParseOpenBracketTrivia().Token()
               from sizeConst in ParseHexNumber().Token()
               from closeBracket in ParseCloseBracketTrivia()
               select new ByValTStrMarshalTypeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [closeBracket],
                   value: $"fixed sysstring[{sizeConst}]",
                   sizeConst: int.Parse(sizeConst));
    }

    internal Parser<ByValArrayMarshalTypeSyntax> ParseByValArrayMarshalType()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from fixedArray in Parse.String("fixed array").Token()
               from openBracket in ParseOpenBracketTrivia().Token()
               from value in ParseHexNumber().Token()
               from closeBracket in ParseCloseBracketTrivia()
               select new ByValArrayMarshalTypeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [closeBracket],
                   value: $"fixed array [{value}]",
                   sizeConst: int.Parse(value));
    }

    internal static Parser<SimpleMarshalTypeSyntax> ParseSimpleMarshalType()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from marshal in Parse.String("ansi bstr")
                            .Or(Parse.String("as any"))
                            .Or(Parse.String("bstr"))
                            .Or(Parse.String("currency"))
                            .Or(Parse.String("error"))
                            .Or(Parse.String("Func"))
                            .Or(Parse.String("47")) /*HString*/
                            .Or(Parse.String("int8"))
                            .Or(Parse.String("int16"))
                            .Or(Parse.String("int32"))
                            .Or(Parse.String("int64"))
                            .Or(Parse.String("uint8"))
                            .Or(Parse.String("uint16"))
                            .Or(Parse.String("uint32"))
                            .Or(Parse.String("uint64"))
                            .Or(Parse.String("float32"))
                            .Or(Parse.String("float64"))
                            .Or(Parse.String("idispatch"))
                            .Or(Parse.String("46")) /*IInspectable*/
                            .Or(Parse.String("interface"))
                            .Or(Parse.String("iunknown"))
                            .Or(Parse.String("[]"))
                            .Or(Parse.String("lpstr"))
                            .Or(Parse.String("lptstr"))
                            .Or(Parse.String("lpstruct"))
                            .Or(Parse.String("lpwstr"))
                            .Or(Parse.String("48")) /*LPUTF8Str*/
                            .Or(Parse.String("safearray"))
                            .Or(Parse.String("struct"))
                            .Or(Parse.String("int")) /*SysInt*/
                            .Or(Parse.String("unsigned int")) /*SysUInt*/
                            .Or(Parse.String("tbstr"))
                            .Or(Parse.String("variant bool"))
                            .Or(Parse.String("byvalstr"))
                            .Token()
                            .Text()
               select new SimpleMarshalTypeSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   value: marshal);
    }

    internal Parser<MarshalTypeSyntax> ParseMarshalType() =>
        ParseCustomMarshalType()
        .Or<MarshalTypeSyntax>(ParseByValTStrMarshalType())
        .Or(ParseByValArrayMarshalType())
        .Or(ParseSimpleMarshalType());

    internal Parser<MarshalTypeSyntax> ParseMarshalFunction()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from marshal in Parse.String("marshal").Token()
               from openParen in ParseOpenParenthesis().Token()
               from marshalType in ParseMarshalType().Token()
               from closeParen in ParseCloseParenthesis()
               select marshalType;
    }

    internal Parser<FileDirectiveSyntax> ParseFileDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from file in Parse.String(".file").Token()
               from location in ParseSingleQuotedStringLiteral()
               select new FileDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   fileName: location.RawValue);
    }

    internal static Parser<LineDirectiveSyntax> ParseLineDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from line in Parse.String(".line").Token()
               from location in ParseHexNumber()
               select new LineDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   line: location);
    }

    internal Parser<ModuleDirectiveSyntax> ParseModuleDirective()
    {
        return (
                from whitespace in ParseWhiteSpaceTrivia()
                from module in Parse.String(".module").Token()
                from name in ParseSingleQuotedStringLiteral()
                select new ModuleDirectiveSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    moduleName: name.RawValue)
            )
            .Or(
                from whitespace in ParseWhiteSpaceTrivia()
                from module in Parse.String(".module").Token()
                select new ModuleDirectiveSyntax(
                    leadingTrivia: [whitespace],
                    trailingTrivia: [],
                    moduleName: null)
            );
    }

    internal static Parser<VTableEntrySyntax> ParseVTEntryDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from vtentry in Parse.String(".vtentry").Token()
               from entry in ParseHexNumber().Token()
               from separator in Parse.Char(':').Token()
               from slot in ParseHexNumber()
               select new VTableEntrySyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   entryNumber: entry,
                   slotNumber: slot);
    }

    internal Parser<FileAlignmentDirectiveSyntax> ParseFileAlignmentDirective()
    {
        try
        {
            return from whitespace in ParseWhiteSpaceTrivia()
                   from file in Parse.String(".file").Token()
                   from alignment in Parse.String("alignment").Token()
                   from fileAlignment in ParseHexNumber()
                   select new FileAlignmentDirectiveSyntax(
                       leadingTrivia: [whitespace],
                       trailingTrivia: [],
                       rawValue: fileAlignment);
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new FileAlignmentDirectiveSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        rawValue: ParserResources.FileAlignmentDirectiveError
                    )
                )
            );
        }
    }

    internal Parser<ParameterModifierSyntax> ParseParameterModifier()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from openBracket in ParseOpenBracketTrivia().Token()
               from modifier in Parse.String("out").Or(Parse.String("in")).Or(Parse.String("opt")).Token().Text()
               from closeBracket in ParseCloseBracketTrivia()
               select new ParameterModifierSyntax(
                   leadingTrivia: [whitespace, openBracket],
                   trailingTrivia: [closeBracket],
                   modifierType: modifier switch
                   {
                       "out" => ParameterModifierType.Out,
                       "opt" => ParameterModifierType.Opt,
                       "in" => ParameterModifierType.In,
                       _ => ParameterModifierType.Unknown
                   }
                );
    }

    internal Parser<ParameterSyntax> ParseParameter()
    {
        try
        {
            return (
                    from whitespace in ParseWhiteSpaceTrivia()
                    from modifiers in ParseParameterModifier().Token().Optional().Many()
                    from parameterType in ParseType().Token()
                    from marshalling in ParseMarshalFunction().Token().Optional()
                    from parameterName in ParseSymbolName()
                    select new ParameterSyntax(
                        leadingTrivia: [whitespace],
                        trailingTrivia: [],
                        modifiers: modifiers.Where(mod => mod.IsDefined).Select(mod => mod.Get()),
                        marshalling: marshalling.IsDefined ? marshalling.Get().AsParameterMarshal() : null,
                        parameterType: parameterType,
                        name: parameterName)
                )
                .Or(
                    from whitespace in ParseWhiteSpaceTrivia()
                    from modifiers in ParseParameterModifier().Token().Optional().Many()
                    from parameterType in ParseType().Token()
                    from marshalling in ParseMarshalFunction().Optional()
                    select new ParameterSyntax(
                        leadingTrivia: [whitespace],
                        trailingTrivia: [],
                        modifiers: modifiers.Where(mod => mod.IsDefined).Select(mod => mod.Get()),
                        marshalling: marshalling.IsDefined ? marshalling.Get().AsParameterMarshal() : null,
                        parameterType: parameterType,
                        name: null)
                );
        }
        catch (ParseException e)
        {
            return AddErrorAndGet(
                e.Position,
                Parse.Return(
                    new ParameterSyntax(
                        leadingTrivia: [],
                        trailingTrivia: [],
                        modifiers: [],
                        marshalling: new ParameterMarshalSyntax(
                            leadingTrivia: [],
                            trailingTrivia: [],
                            marshalType: new MarshalTypeSyntax(
                                leadingTrivia: [],
                                trailingTrivia: [],
                                value: "/*Unknown marshalling type*/"
                            )
                        ),
                        parameterType: ParserResources.DummyTypeReference(),
                        name: null
                    )
                )
            );
        }
    }

    internal Parser<MethodReferenceWithOmittedArgsSyntax> ParseMethodReferenceWithOmittedArgs()
    {
        // HACK: return ParseFieldOrPropertyReference() first
        return from fieldOrProp in ParseFieldOrPropertyReference()
               select new MethodReferenceWithOmittedArgsSyntax(
                   leadingTrivia: fieldOrProp.LeadingTrivia,
                   trailingTrivia: fieldOrProp.TrailingTrivia,
                   typeReference: fieldOrProp.FieldType,
                   methodName: fieldOrProp.FieldOrPropertyName,
                   methodDeclaringTypeReference: fieldOrProp.DeclaringTypeReference);
    }

    internal static Parser<IEnumerable<string>> ParsePreMethodDeclarationFlags()
    {
        return (
                from arg in Parse.String("specialname")
                        .Or(Parse.String("rtspecialname"))
                        .Or(Parse.String("virtual"))
                        .Or(Parse.String("newslot"))
                        .Or(Parse.String("public"))
                        .Or(Parse.String("private"))
                        .Or(Parse.String("assembly"))
                        .Or(Parse.String("famorassem"))
                        .Or(Parse.String("famandassem"))
                        .Or(Parse.String("family"))
                        .Or(Parse.String("static"))
                        .Or(Parse.String("hidebysig"))
                        .Or(Parse.String("final"))
                        .Token()
                        .Text()
                select arg
            )
            .Many();
    }

    internal static Parser<IEnumerable<string>> ParseAfterMethodDeclarationFlags()
    {
        return (
                from instruction in Parse.String("preservesig")
                        .Or(Parse.String("synchronized"))
                        .Or(Parse.String("cil managed"))
                        .Or(Parse.String("cil unmanaged"))
                        .Or(Parse.String("runtime managed"))
                        .Or(Parse.String("runtime unmanaged"))
                        .Token()
                        .Text()
                select instruction
            )
            .Many();
    }

    internal static Parser<MethodFlagSyntax> ParseMethodFlag()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from flag in Parse.String("flag").Token()
               from openParen in ParseOpenParenthesis().Token()
               from hex in ParseHexNumber().Token()
               from closeParen in ParseCloseParenthesis()
               select new MethodFlagSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [closeParen],
                   flag: hex);
    }

    internal Parser<PInvokeImplSyntax> ParsePInvokeImpl()
    {
        static CharSet GetCharSet(IEnumerable<string> pinvokeFlags)
        {
            if (pinvokeFlags.Contains("autochar"))
            {
                return CharSet.Auto;
            }
            else if (pinvokeFlags.Contains("ansi"))
            {
                return CharSet.Ansi;
            }
            else if (pinvokeFlags.Contains("unicode"))
            {
                return CharSet.Unicode;
            }
            else
            {
                return CharSet.None;
            }
        }

        static CallingConvention GetCallingConvention(IEnumerable<string> pinvokeFlags)
        {
            if (pinvokeFlags.Contains("winapi"))
            {
                return CallingConvention.Winapi;
            }
            else if (pinvokeFlags.Contains("cdecl"))
            {
                return CallingConvention.Cdecl;
            }
            else if (pinvokeFlags.Contains("fastcall"))
            {
                return CallingConvention.FastCall;
            }
            else if (pinvokeFlags.Contains("stdcall"))
            {
                return CallingConvention.StdCall;
            }
            else if (pinvokeFlags.Contains("thiscall"))
            {
                return CallingConvention.ThisCall;
            }
            else
            {
                return ParserResources.DefaultCallingConvention;
            }
        }

        return from whitespace in ParseWhiteSpaceTrivia()
               from pinvokeimpl in Parse.String("pinvokeimpl").Token()
               from openParen in ParseOpenParenthesis().Token()
               from dllName in ParseStringLiteral().Token()
               from entrypoint in ParsePInvokeEntryPoint().Token().Optional()
               from flags in Parse.String("nomangle")
                        .Or(Parse.String("autochar"))
                        .Or(Parse.String("ansi"))
                        .Or(Parse.String("unicode"))
                        .Or(Parse.String("lasterr"))
                        .Or(Parse.String("winapi"))
                        .Or(Parse.String("cdecl"))
                        .Or(Parse.String("fastcall"))
                        .Or(Parse.String("thiscall"))
                        .Or(Parse.String("stdcall"))
                        .Token()
                        .Text()
                        .Many()
                        .Token()
               from closeParen in ParseCloseParenthesis()
               let characterSet = GetCharSet(flags)
               let callingConvention = GetCallingConvention(flags)
               let isLastErr = flags.Contains("lasterr")
               let isExactSpelling = flags.Contains("nomangle")
               select new PInvokeImplSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [closeParen],
                   dllName: dllName.RawValue,
                   entryPoint: entrypoint.IsDefined ? entrypoint.Get() : null,
                   charSet: characterSet,
                   rawCharSet: characterSet.ToString().ToLowerInvariant(),
                   setLastError: isLastErr,
                   callingConvention: callingConvention,
                   rawCallingConvention: callingConvention.ToString().ToLowerInvariant(),
                   exactSpelling: isExactSpelling);

    }

    internal Parser<PInvokeEntryPointSyntax> ParsePInvokeEntryPoint()
    {
        return from @as in Parse.String("as").Token()
               from entryPoint in ParseStringLiteral().Token()
               select new PInvokeEntryPointSyntax(
                   leadingTrivia: [],
                   trailingTrivia: [],
                   name: entryPoint.RawValue);
    }

    internal static Parser<CharSet> ParseCharSet()
    {
        return from charset in Parse.String("autochar") /*CharSet.Auto*/
                            .Or(Parse.String("ansi")) /*CharSet.Ansi*/
                            .Or(Parse.String("unicode")) /*CharSet.Unicode*/
                            .Text()
               select charset switch
               {
                   "autochar" => CharSet.Auto,
                   "ansi" => CharSet.Ansi,
                   "unicode" => CharSet.Unicode,
                   _ => CharSet.None
               };
    }

    internal Parser<LocalVariableSyntax> ParseLocalVariable()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from index in (
                   from _ in Parse.Char('[').Token()
                   from idx in ParseHexNumber().Token()
                   from __ in Parse.Char(']').Token()
                   select idx
                ).Optional()
               from varType in ParseType()
               from variableName in ParseSymbolName().Token().Optional()
               select new LocalVariableSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   index: index.IsDefined ? index.Get() : null,
                   variableType: varType,
                   variableName: variableName.IsDefined ? variableName.Get() : null);
    }

    internal Parser<LocalsDirectiveSyntax> ParseLocalsDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from _localsDirective in Parse.String(".locals").Token()
               from optionalInit in Parse.String("init").Token().Optional()
               from openParen in ParseOpenParenthesis().Token()
               from locals in (
                    from varLocal in ParseLocalVariable().Token()
                    from comma in Parse.Char(',').Token().Optional()
                    select varLocal
               ).Many().Token()
               from closeParen in ParseCloseParenthesis()
               select new LocalsDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   isLocalsInit: optionalInit.IsDefined,
                   variables: locals);
    }

    internal static Parser<LabelSyntax> ParseLabel()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from labelName in Parse.AnyChar.Until(Parse.Char(':')).Text()
               select new LabelSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   labelName: labelName);
    }

    internal Parser<ParamDirectiveSyntax> ParseParamDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from directive in Parse.String(".param").Token()
               from openBrace in ParseOpenBracketTrivia().Token()
               from index in ParseHexNumber().Token()
               from closeBrace in ParseCloseBracketTrivia().Token()
               from attributes in ParseCustomAttribute().Token().Many().Token()
               select new ParamDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   parameterIndex: index,
                   customAttributes: attributes);
    }

    internal Parser<SyntaxNode> ParseNodePartOfMethodBody()
    {
        return ParseLocalsDirective()
            .Or<SyntaxNode>(ParseComment())
            .Or(ParseMaxStackDirective())
            .Or(ParseEntryPointDirective())
            .Or(ParseTryFinallyBlock())
            .Or(ParseInstruction())
            .Or(ParseParamDirective())
            .Or(ParseCustomAttribute())
            .Or(ParseVTEntryDirective())
            .Or(ParseLineDirective())
            .Or(ParseLabel());
    }

    internal Parser<MethodDeclarationSyntax> ParseMethodDeclaration()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from method in Parse.String(".method").Token()
               from flags in ParsePreMethodDeclarationFlags().Token()
               from pinvoke in ParsePInvokeImpl().Token().Optional()
               from possibleInstance in Parse.String("instance").Token().Optional()
               from returnType in ParseType().Token()
               from methodName in ParseSymbolName().Token()
               from openParen in ParseOpenParenthesis().Token()
               from args in (
                    from arg in ParseParameter().Token()
                    from comma in Parse.Char(',').Token().Optional()
                    select arg
               ).Many()
               from closeParen in ParseCloseParenthesis().Token()
               from afterMethodFlags in ParseAfterMethodDeclarationFlags().Token().Optional()
               from flag in ParseMethodFlag().Token().Optional()
               from openCurlyBrace in Parse.Char('{').Token()
               from body in ParseNodePartOfMethodBody().Until(Parse.Char('}').Token()).Token()
               select new MethodDeclarationSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   methodFlags: flags,
                   methodImplementationFlags: afterMethodFlags.IsDefined ? afterMethodFlags.Get() : [],
                   additionalMethodImplementationFlags: flag.IsDefined ? [flag.Get()] : [],
                   returnType: returnType,
                   methodName: methodName,
                   pInvokeInfo: pinvoke.IsDefined ? pinvoke.Get() : null,
                   parameters: args,
                   descendantNodes: body,
                   isInstanceReturnType: possibleInstance.IsDefined);
    }

    internal Parser<GetAccessorSyntax> ParseGetAccessor()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from getAccessor in Parse.String(".get").Token()
               from methodCall in ParseMethodCall()
               select new GetAccessorSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: methodCall);
    }

    internal Parser<SetAccessorSyntax> ParseSetAccessor()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from getAccessor in Parse.String(".set").Token()
               from methodCall in ParseMethodCall()
               select new SetAccessorSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: methodCall);
    }

    internal Parser<AccessorSyntax> ParsePropertyAccessor()
    {
        return ParseGetAccessor().Or<AccessorSyntax>(ParseSetAccessor());
    }

    internal Parser<AddOnAccessorSyntax> ParseAddOnAccessor()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from getAccessor in Parse.String(".addon").Token()
               from methodCall in ParseMethodCall()
               select new AddOnAccessorSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: methodCall);
    }

    internal Parser<RemoveOnAccessorSyntax> ParseRemoveOnAccessor()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from getAccessor in Parse.String(".removeon").Token()
               from methodCall in ParseMethodCall()
               select new RemoveOnAccessorSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   target: methodCall);
    }

    internal Parser<AccessorSyntax> ParseEventAccessor()
    {
        return ParseAddOnAccessor().Or<AccessorSyntax>(ParseRemoveOnAccessor());
    }

    internal Parser<PropertyDeclarationSyntax> ParsePropertyDeclaration()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from property in Parse.String(".property").Token()
               from possibleInstance in Parse.String("instance").Token().Optional()
               from returnType in ParseType().Token()
               from methodName in ParseSymbolName().Token()
               from openCloseParenthesis in Parse.String("()").Token()
               from openCurlyBrace in Parse.Char('{').Token()
               from customAttributes in ParseCustomAttribute().Token().Many().Token()
               from accessor1 in ParsePropertyAccessor().Token()
               from accessor2 in ParsePropertyAccessor().Token().Optional() // There should be at least one accessor, but there can be only 2 accessors maximum
               from closeCurlyBrace in Parse.Char('}')
               let getAccessorOrNull =
                    accessor1 is GetAccessorSyntax syntax ? syntax
                    : accessor2.IsDefined
                      ? (accessor2.Get() is GetAccessorSyntax getAccessor ? getAccessor : null)
                      : null
               let setAccessorOrNull =
                    accessor1 is SetAccessorSyntax syntax ? syntax
                    : accessor2.IsDefined
                      ? (accessor2.Get() is SetAccessorSyntax setAccessor ? setAccessor : null)
                      : null
               select new PropertyDeclarationSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   returnType: returnType,
                   isReturnTypeInstance: possibleInstance.IsDefined,
                   name: methodName,
                   getAccessor: getAccessorOrNull,
                   setAccessor: setAccessorOrNull,
                   customAttributes: customAttributes);
    }

    internal Parser<EventDeclarationSyntax> ParseEventDeclaration()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from property in Parse.String(".event").Token()
               from returnType in ParseType().Token()
               from methodName in ParseSymbolName().Token()
               from openCloseParenthesis in Parse.String("()").Token()
               from openCurlyBrace in Parse.Char('{').Token()
               from customAttributes in ParseCustomAttribute().Token().Many().Token()
               from accessor1 in ParseEventAccessor().Token()
               from accessor2 in ParseEventAccessor().Token().Optional() // There should be at least one accessor, but there can be only 2 accessors maximum
               from closeCurlyBrace in Parse.Char('}')
               let addAccessorOrNull =
                    accessor1 is AddOnAccessorSyntax syntax ? syntax
                    : accessor2.IsDefined
                      ? (accessor2.Get() is AddOnAccessorSyntax getAccessor ? getAccessor : null)
                      : null
               let removeAccessorOrNull =
                    accessor1 is RemoveOnAccessorSyntax syntax ? syntax
                    : accessor2.IsDefined
                      ? (accessor2.Get() is RemoveOnAccessorSyntax setAccessor ? setAccessor : null)
                      : null
               select new EventDeclarationSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   returnType: returnType,
                   name: methodName,
                   addOnAccessor: addAccessorOrNull,
                   removeOnAccessor: removeAccessorOrNull,
                   customAttributes: customAttributes);
    }

    internal static Parser<PackDirectiveSyntax> ParsePackDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from pack in Parse.String(".pack").Token()
               from packBy in ParseHexNumber()
               select new PackDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   pack: packBy);
    }

    internal static Parser<IEnumerable<string>> ParseClassFlags()
    {
        return (
                from flag in Parse.String("sequential").Token()
                        .Or(Parse.String("enum").Token())
                        .Or(Parse.String("interface").Token())
                        .Or(Parse.String("auto").Token())
                        .Or(Parse.String("ansi").Token())
                        .Or(Parse.String("beforefieldinit").Token())
                        .Or(Parse.String("public").Token())
                        .Or(Parse.String("private").Token())
                        .Or(Parse.String("assembly").Token())
                        .Or(Parse.String("family").Token())
                        .Or(Parse.String("famorassem").Token())
                        .Or(Parse.String("famandassem").Token())
                        .Or(Parse.String("sealed").Token())
                        .Or(Parse.String("abstract").Token())
                        .Or(Parse.String("nested").Token())
                        .Or(Parse.String("serializable").Token())
                        .Or(Parse.String("static").Token())
                        .Text()
                select flag
            ).Many();
    }

    internal Parser<TypeSyntax> ParseExtends()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from extends in Parse.String("extends").Token()
               from type in ParseType()
               select type;
    }

    internal Parser<IEnumerable<TypeSyntax>> ParseImplements()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from implements in Parse.String("implements").Token()
               from inheritingTypes in (
                    from type in ParseType().Token()
                    from comma in Parse.Char(',').Token().Optional()
                    select type
               ).Many()
               select inheritingTypes;
    }

    internal static Parser<SizeDirectiveSyntax> ParseSizeDirective()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from size in Parse.String(".size").Token()
               from number in ParseHexNumber()
               select new SizeDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   size: number);
    }

    internal Parser<SyntaxNode> ParseNodePartOfClassBody()
    {
        return ParseFieldDeclaration()
            .Or<SyntaxNode>(ParseCustomAttribute())
            .Or(ParseClassDeclaration()) /*Nested classes in IL are OK*/
            .Or(ParseMethodDeclaration())
            .Or(ParsePackDirective())
            .Or(ParseEventDeclaration())
            .Or(ParsePropertyDeclaration())
            .Or(ParseLineDirective())
            .Or(ParseComment())
            .Or(ParseSizeDirective());
    }

    internal Parser<ClassDeclarationSyntax> ParseClassDeclaration()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from classDecl in Parse.String(".class").Token()
               from flags in ParseClassFlags().Token()
               from name in ParseSymbolName().Token()
               from extends in ParseExtends().Optional().Token()
               from implements in ParseImplements().Optional().Token()
               from curlyBrace in Parse.Char('{').Token()
               from descendants in ParseNodePartOfClassBody().Token().Until(Parse.Char('}').Token()).Token()
               select new ClassDeclarationSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   flags: flags,
                   name: name,
                   classExtends: extends.IsDefined ? extends.Get() : null,
                   classImplements: implements.IsDefined ? implements.Get() : null,
                   descendantNodes: descendants);
    }

    internal Parser<SyntaxNode> ParseNodePartOfAssemblyBody()
    {
        return ParseCustomAttribute().Token()
            .Or<SyntaxNode>(ParsePermissionSet().Token())
            .Or(ParseHashAlgorithm().Token())
            .Or(ParseVerDirective().Token())
            .Or(ParseLineDirective().Token())
            .Or(ParseComment().Token());
    }

    internal Parser<AssemblyDeclarationSyntax> ParseAssemblyDeclaration()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from assembly in Parse.String(".assembly").Token()
               from @extern in Parse.String("extern").Token().Optional()
               from asmName in ParseSymbolName().Token()
               from openCurlyBrace in Parse.Char('{').Token()
               from body in ParseNodePartOfAssemblyBody().Token().Until(Parse.Char('}').Token()).Token()
               select new AssemblyDeclarationSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   assemblyName: asmName,
                   isExtern: @extern.IsDefined,
                   descendantNodes: body);
    }

    internal static Parser<ImageBaseDirectiveSyntax> ParseImageBase()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from imageBase in Parse.String(".imagebase").Token()
               from value in ParseHexNumber()
               select new ImageBaseDirectiveSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   imageBase: value);
    }

    internal Parser<SyntaxNode> ParseNodePartOfRoot()
    {
        return ParseAssemblyDeclaration().Token()
            .Or<SyntaxNode>(ParseImageBase().Token())
            .Or(ParseFileAlignmentDirective().Token())
            .Or(ParseFileDirective().Token())
            .Or(ParseModuleDirective().Token())
            .Or(ParseLineDirective().Token())
            .Or(ParseClassDeclaration().Token())
            .Or(ParseMethodDeclaration().Token()) // Yeah, methods can in fact be a top-level node
            .Or(ParseComment().Token());

    }

    internal Parser<TryBlockSyntax> ParseTryBlock()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from _try in Parse.String(".try").Token()
               from openCurlyBrace in Parse.Char('{').Token()
               from body in ParseNodePartOfMethodBody().Token().Until(Parse.Char('}').Token())
               select new TryBlockSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   descendantNodes: body);
    }

    internal Parser<FinallyBlockSyntax> ParseFinallyBlock()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from @finally in Parse.String("finally").Token()
               from openCurlyBrace in Parse.Char('{').Token()
               from body in ParseNodePartOfMethodBody().Token().Until(Parse.Char('}').Token())
               select new FinallyBlockSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   descendantNodes: body);
    }

    internal Parser<TryFinallyBlockSyntax> ParseTryFinallyBlock()
    {
        return from whitespace in ParseWhiteSpaceTrivia()
               from @try in ParseTryBlock().Token()
               from @finally in ParseFinallyBlock()
               select new TryFinallyBlockSyntax(
                   leadingTrivia: [whitespace],
                   trailingTrivia: [],
                   tryBlock: @try,
                   finallyBlock: @finally);
    }

    internal Parser<ILRootNode> Root()
    {
        return from nodes in ParseNodePartOfRoot().Many()
               select new ILRootNode(descendantNodes: nodes);
    }
}
