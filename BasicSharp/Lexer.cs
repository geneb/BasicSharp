using System;
using System.Diagnostics;

namespace BasicSharp
{
    public class Lexer
    {
        private readonly string source;
        private Marker sourceMarker;
        private char lastChar;

        public Marker TokenMarker { get; set; }

        public string Identifier { get; set; }
        public Value Value { get; set; }

        public Lexer(string input)
        {
            source = input;
            sourceMarker = new Marker(0, 1, 1);
            lastChar = source[0];
        }

        public void GoTo(Marker marker)
        {
            sourceMarker = marker;
        }

        char GetChar()
        {
            sourceMarker.Column++;
            sourceMarker.Pointer++;

            if (sourceMarker.Pointer >= source.Length)
                return lastChar = (char)0;

            if ((lastChar = source[sourceMarker.Pointer]) == '\n')
            {
                sourceMarker.Column = 1;
                sourceMarker.Line++;
            }
            return lastChar;
        }

        public bool GetValidChar() {
            char inChar = GetChar();
            if (char.IsLetterOrDigit(inChar) || inChar == ' ') {
                return true;
            } else {
                return false;
            }
        }
        public Token GetToken()
        {
            while (lastChar == ' ' || lastChar == '\t' || lastChar == '\r')
                GetChar();

            TokenMarker = sourceMarker;

            if (char.IsLetter(lastChar))
            {
                Identifier = lastChar.ToString();
                //while (char.IsLetterOrDigit(GetChar()))
                while (GetValidChar())
                    Identifier += lastChar;
                Debug.Print(Identifier);
                switch (Identifier.ToUpper())
                {
                    case "PRINT": return Token.Print;
                    case "IF": return Token.If;
                    case "ENDIF": return Token.EndIf;
                    case "THEN": return Token.Then;
                    case "ELSE": return Token.Else;
                    case "FOR": return Token.For;
                    case "TO": return Token.To;
                    case "NEXT": return Token.Next;
                    case "GOTO": return Token.Goto;
                    case "INPUT": return Token.Input;
                    case "LET": return Token.Let;
                    case "GOSUB": return Token.Gosub;
                    case "RETURN": return Token.Return;
                    case "END": return Token.End;
                    case "OR": return Token.Or;
                    case "AND": return Token.And;
                    case "REM":
                        while (lastChar != '\n') GetChar();
                        GetChar();
                        return GetToken();
                    default:
                        return Token.Identifer;
                }
            }

            if (char.IsDigit(lastChar))
            {
                string num = "";
                do { num += lastChar; } while (char.IsDigit(GetChar()) || lastChar == '.');

                double real;
                if (!double.TryParse(num, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out real))
                    throw new Exception("ERROR while parsing number");
                Value = new Value(real);
                return Token.Value;
            }

            Token tok = Token.Unknown;
            switch (lastChar)
            {
                case '\n': tok = Token.NewLine; break;
                case ':': tok = Token.Colon; break;
                case ';': tok = Token.Semicolon; break;
                case ',': tok = Token.Comma; break;
                case '=': tok = Token.Equal; break;
                case '+': tok = Token.Plus; break;
                case '-': tok = Token.Minus; break;
                case '/': tok = Token.Slash; break;
                case '*': tok = Token.Asterisk; break;
                case '^': tok = Token.Caret; break;
                case '(': tok = Token.LParen; break;
                case ')': tok = Token.RParen; break;
                case '\'':
                    while (lastChar != '\n') GetChar();
                    GetChar();
                    return GetToken();
                case '<':
                    GetChar();
                    if (lastChar == '>') tok = Token.NotEqual;
                    else if (lastChar == '=') tok = Token.LessEqual;
                    else return Token.Less;
                    break;
                case '>':
                    GetChar();
                    if (lastChar == '=') tok = Token.MoreEqual;
                    else return Token.More;
                    break;
                case '"':
                    string str = "";
                    while (GetChar() != '"')
                    {
                        if (lastChar == '\\')
                        {
                            switch (char.ToLower(GetChar()))
                            {
                                case 'n': str += '\n'; break;
                                case 't': str += '\t'; break;
                                case '\\': str += '\\'; break;
                                case '"': str += '"'; break;
                            }
                        }
                        else
                        {
                            str += lastChar;
                        }
                    }
                    Value = new Value(str);
                    tok = Token.Value;
                    break;
                case (char)0:
                    return Token.EOF;
            }

            GetChar();
            return tok;
        }
    }
}

