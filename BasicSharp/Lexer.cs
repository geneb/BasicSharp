using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace BasicSharp {
    public class Lexer {
        private readonly string source;
        private Marker sourceMarker;
        private char lastChar;

        private List<string> keywordList = new List<string>() {
            // If any changes are made to this list of keywords, the 
            // Token enum in Token.cs must be updated as well!
            "PRINT",
            "IF",
            "ENDIF",
            "END IF",  // treated as the same token as ENDIF           
            "THEN",
            "ELSE",
            "FOR",
            "TO",
            "NEXT",
            "GOTO",
            "INPUT",
            "LET",
            "GOSUB",
            "RETURN",
            "REM",
            "END"};

        private Stack<Marker> returnStack = new Stack<Marker>();

        public Marker TokenMarker { get; set; }

        public string Identifier { get; set; }
        public Value Value { get; set; }

        public Lexer(string input) {
            source = input;
            sourceMarker = new Marker(0, 1, 1);
            lastChar = source[0];
        }

        public void GoTo(Marker marker) {
            sourceMarker = marker;
        }

        public void GoSubPush() {
            // This bumps the source line marker by one and
            // pushes it on to the return stack.  When this
            // item is popped from the return stack, execution
            // should resume on the line AFTER the GoSub.
            Debug.Print("Current Column: " + sourceMarker.Column.ToString());
            Debug.Print("Current Line: " + sourceMarker.Line.ToString());
            Debug.Print("Current Pointer: " + sourceMarker.Pointer.ToString());

            Marker workMarker = sourceMarker;
            workMarker.Column = 1;
            workMarker.Line++; // moves us to the next source line.
            workMarker.Pointer++; // should technically be the first character of the next line.
            returnStack.Push(workMarker);
        }
        public void GoSub(Marker marker) {
            //returnStack.Push(marker);
            sourceMarker = marker;
        }

        public void Return() {
            sourceMarker = returnStack.Pop();
            Debug.Print("Current Column: " + sourceMarker.Column.ToString());
            Debug.Print("Current Line: " + sourceMarker.Line.ToString());
            Debug.Print("Current Pointer: " + sourceMarker.Pointer.ToString());
            lastChar = '\r';
        }
        char GetChar() {
            sourceMarker.Column++;
            sourceMarker.Pointer++;

            // Checks to see if the source code position pointer has met or exceeded the length
            // of the source file.
            if (sourceMarker.Pointer >= source.Length)
                return lastChar = (char)0;

            // If we've reached the end of the line, reset the column position and increment the 
            // line counter.
            if ((lastChar = source[sourceMarker.Pointer]) == '\n') {
                sourceMarker.Column = 1;
                sourceMarker.Line++;
            }
            return lastChar;
        }

        public bool GetValidChar() {
            char inChar = GetChar();
            if (char.IsLetterOrDigit(inChar) || inChar == ' ') {
                if (inChar == ' ') {
                    // let's see if what we've got so far is an actual keyword...
                    if (keywordList.Contains(Identifier.ToUpper().Trim())) {
                        // we might have a full keyword here.  However, we may only have a partial...
                        // we need to peek ahead in the source code buffer to the next space...
                        string validKeyword = Identifier + " ";
                        int bufIdx = 1;
                        while(((source[sourceMarker.Pointer + bufIdx] != ' ') &&
                            (source[sourceMarker.Pointer + bufIdx] != '\r') &&
                            (source[sourceMarker.Pointer + bufIdx] != '\n' )) || (bufIdx > 10)) {  // TODO no magic numbers!
                            // we go until we find the next space or bufIdx goes way beyond a reasonable point.
                            validKeyword += source[sourceMarker.Pointer + bufIdx];
                            bufIdx++;
                        }
                        if (keywordList.Contains(validKeyword.ToUpper())) {
                            // make the Identifier the valid keyword we just built...
                            Identifier = validKeyword;
                            // We need to update the source pointer now...
                            sourceMarker.Pointer += (bufIdx - 1);
                            return false;
                        } else {
                            return false;
                        }
                    } else {
                        return true;
                    }
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }
        public Token GetToken() {
            while (lastChar == ' ' || lastChar == '\t' || lastChar == '\r')
                GetChar();

            TokenMarker = sourceMarker;

            if (char.IsLetter(lastChar)) {
                Identifier = lastChar.ToString();
                //while (char.IsLetterOrDigit(GetChar()))
                while (GetValidChar())
                    Identifier += lastChar;
          
                //Debug.Print(Identifier);
                Identifier = Identifier.Trim();
                switch (Identifier.ToUpper()) {
                    case "PRINT": return Token.Print;
                    case "IF": return Token.If;
                    case "ENDIF":
                    case "END IF": return Token.EndIf;
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

            if (char.IsDigit(lastChar)) {
                string num = "";
                do { num += lastChar; } while (char.IsDigit(GetChar()) || lastChar == '.');

                double real;
                if (!double.TryParse(num, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out real))
                    throw new Exception("ERROR while parsing number");
                Value = new Value(real);
                return Token.Value;
            }

            Token tok = Token.Unknown;
            switch (lastChar) {
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
                    while (GetChar() != '"') {
                        if (lastChar == '\\') {
                            switch (char.ToLower(GetChar())) {
                                case 'n': str += '\n'; break;
                                case 't': str += '\t'; break;
                                case '\\': str += '\\'; break;
                                case '"': str += '"'; break;
                            }
                        } else {
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

