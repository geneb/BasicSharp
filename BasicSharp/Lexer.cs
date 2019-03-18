using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace OpenSBP {
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
            "INPUT", // Page 15 - This keyword needs to be able to handle the two different modes
                     // of input that OpenSBP has - one is a dialog box-oriented version that can return up
                     // to ten variables, the second is used to read data from a file via, "INPUT #"
                     // if we're doing the dialog box input, then the token should be "InputUser".  If
                     // we're parsing the file input version, the token should be "InputFile".
            "LET",
            "GOSUB",
            "RETURN",
            "REM",
            "END",
                // New keywords for OpenSBP
        // TODO There are a number of mathematical operations that OpenSBP support.  See the "Operations" list.
              
        "CLOSE",       // Page 12
        "END ALL",      // Page 12
        "EXIT SHOPBOT", // Page 13
        "MSGBOX",      // Page 15-17
        "ON INPUT",     // Page 17-18
        "OPEN",    // Page 18
        "PAUSE",       // Page 19-20
        "PLAY",        // Page 20-21
        "SHELL",       // Page 23-24
        "WARNING OFF",  // Page 25
        "WRITE",   // Pages 25-26
        
        //  These are special registry commands that allow reading/writing to the ShopBot area of the Windows registry.
        //  This should probably be configured to read/write data from an ini file of some sort, as Linux has no "registry".
        "SETUSRVAL",        // Page 26
        "SETUSRVALCLRD",    // Page 26
        "SETSPINDLESTATUS", // Page 26
        "GETUSRVAL",        // Page 26
        "GETUSRVALCLRD",    // Page 26
        "GETUSRPATH",       // Page 26
        "GETAPPPATH",       // Page 27
        "GETPARTFILENAME",  // Page 27
        "GETSPINDLESTATUS" }; // Page 28
        

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
            // we store our source position right after we've processed the
            // jump token + identifier (target label).
            returnStack.Push(sourceMarker);
        }
        public void GoSub(Marker marker) {
            GoTo(marker);
        }

        public Marker Return() {
            lastChar = '\r';
            return returnStack.Pop();
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
            // TODO Need a more elegant way of determining if the character we have
            //      is valid for an identifier or keyword.

            char inChar = GetChar();
            string systemVar = "";
            int bufMax = 6; // TODO no magic numbers!
            int bufIdx = 0;
            if (char.IsLetterOrDigit(inChar) || inChar == ' ' || inChar == '(') {
                //if (inChar == ' ' || inChar == '(' || inChar == '_') {
                // let's see if what we've got so far is an actual keyword...
                if (keywordList.Contains(Identifier.ToUpper().Trim())) {
                    // we might have a full keyword here.  However, we may only have a partial...
                    // we need to peek ahead in the source code buffer to see if we've got a 
                    // two word keyword here.  (ex. end if, on input, etc...)
                    string validKeyword = Identifier + " ";
                    bufIdx = 1;
                    if (sourceMarker.Pointer + bufMax > source.Length) {
                        // don't let us look past the end of the source file!
                        bufMax = sourceMarker.Pointer - source.Length;
                    }
                    if (sourceMarker.Pointer + bufIdx > source.Length) {
                        // yeah, we're out of source to look through...
                        return false;
                    }

                    while (((source[sourceMarker.Pointer + bufIdx] != ' ') &&
                    (source[sourceMarker.Pointer + bufIdx] != '\r') &&
                    (source[sourceMarker.Pointer + bufIdx] != '\n')) && (bufIdx <= bufMax)) {
                        if (sourceMarker.Pointer + bufIdx <= source.Length) {
                            validKeyword += source[sourceMarker.Pointer + bufIdx];
                            bufIdx++;
                        } else {
                            break;
                        }

                    }
                    if (keywordList.Contains(validKeyword.ToUpper())) {
                        // make the Identifier the valid keyword we just built...
                        Identifier = validKeyword;
                        // We need to update the source pointer & column now...
                        sourceMarker.Pointer += (bufIdx - 1);
                        sourceMarker.Column += (bufIdx - 1);  // will this break something?
                        return false;
                    } else {
                        return false;
                    }
                } else {
                    // let's look to see if we have a system variable here...
                    if (Identifier.StartsWith("%")) {
                        // Yep, it's a system variable.
                        if (inChar == '(') {
                            // we need to police up the rest of it...
                            systemVar = Identifier;
                            bufIdx = 0;
                            while (((source[sourceMarker.Pointer + bufIdx] != ' ') &&
                            (source[sourceMarker.Pointer + bufIdx] != '\r') &&
                            (source[sourceMarker.Pointer + bufIdx] != '\n')) && (bufIdx <= bufMax)) {
                                if (sourceMarker.Pointer + bufIdx <= source.Length) {
                                    if (source[sourceMarker.Pointer + bufIdx] == ')') {
                                        // ensures we get the closing paren.
                                        systemVar += source[sourceMarker.Pointer + bufIdx];
                                        break;
                                    }
                                    systemVar += source[sourceMarker.Pointer + bufIdx];
                                    bufIdx++;
                                } else {
                                    break;
                                }

                            }
                        }
                        // Now we see if what we have is usable...
                        if (systemVar.StartsWith("%(") && systemVar.EndsWith(")")) {
                            // make the Identifier the valid keyword we just built...
                            Identifier = systemVar;
                            // We need to update the source pointer & column now...
                            sourceMarker.Pointer += bufIdx + 1;
                            sourceMarker.Column += bufIdx + 1;  // will this break something?
                            lastChar = source[sourceMarker.Pointer]; // hide the fact that the last actual "real" character was "("
                            return false;
                        } else {
                            return false;
                        }
                    }
                    return true;
                }
                //} else {
                //    return true;
                //}
            } else if (inChar == '_') {
                // underscores are permitted.
                return true;
            } else
                return false;

        }

        public Token GetToken() {
            bool skipIdentParse = false;

            while (lastChar == ' ' || lastChar == '\t' || lastChar == '\r')
                GetChar();

            TokenMarker = sourceMarker;

            if (char.IsLetter(lastChar) || lastChar == '&' || lastChar == '%') {
                // User variables in OpenSBP should start with a &.  System variables start with "%"
                // The problem is that string concatenation ALSO uses the "&" symbol...
                // TODO We need to disallow variables that don't start with "&" and "%", per the OpenSBP spec.

                // if lastChar is &, we need to peek ahead and see if the next character is a space.
                // if it is, we can assume this is a concatenation op...
                if (lastChar == '&') {
                    if (source[sourceMarker.Pointer + 1] == ' ') {
                        skipIdentParse = true;
                    }
                }
                if (!skipIdentParse) {
                    Identifier = lastChar.ToString();
                    while (GetValidChar())
                        Identifier += lastChar;

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
                            if (Identifier.StartsWith("%(")) {
                                return Token.SystemVar;
                            } else {
                                return Token.Identifer;
                            }
                    }
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
                case '&': tok = Token.Ampersand; break;
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

