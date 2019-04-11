using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace OpenSBP {
    public class Interpreter {
        // TODO Convert any string concatenation operations to use StringBuilder.

        public const int SYSVARMAX = 170;  // Current max # of system variables for OpenSBP
        public const int MAXFILES = 9;     // OpenSBP max # of open files.

        private Lexer lex;
        private Token prevToken;
        private Token lastToken;

        private Dictionary<string, Value> vars;
        private Dictionary<string, Value> sysVars;  // OpenSBP System Variables
        private Dictionary<string, Value> sysConsts;  // OpenSBP constants (used by msgbox, etc)
        private Dictionary<string, Marker> labels;
        private Dictionary<string, Marker> loops;

        private Dictionary<int, FileInfo> userFiles;
        private Dictionary<int, OnInputInfo> inputEvents;
        private Dictionary<string, BasicFunction> funcs;

        public delegate Value BasicFunction(Interpreter interpreter, List<Value> args);

        private int ifcounter;

        private Marker lineMarker;

        private bool exit;

        public bool StandAlone { get; set; }

        public System.Windows.Forms.TextBox OutputWindow;

        public Interpreter(string input) {
            this.lex = new Lexer(input);
            this.vars = new Dictionary<string, Value>();
            this.sysVars = new Dictionary<string, Value>();
            this.sysConsts = new Dictionary<string, Value>();
            this.labels = new Dictionary<string, Marker>();
            this.loops = new Dictionary<string, Marker>();
            this.funcs = new Dictionary<string, BasicFunction>();
            this.userFiles = new Dictionary<int, FileInfo>();
            this.inputEvents = new Dictionary<int, OnInputInfo>();

            this.ifcounter = 0;
            BuiltIns.InstallAll(this);
            InitializeSystemVariables();
            InitializeOnInputDict();

        }

        private void InitializeSystemVariables() {
            // this builds out all the system variables that OpenSBP currently knows about.
            string varName = "";
            //Value newVal = new Value();

            for (int x = 1; x <= SYSVARMAX; x++) {
                varName = "%(" + x.ToString() + ")";
                SetSysVar(varName, Value.Zero);  // makes them exist, but assigns no actual value.
            }

            // TODO The System Variables need their own class.
            // test entry.
            SetSysVar("%(1)", new Value(4.1250));

            // Now we add system constants...

            // These are used to dicatate what buttons we apply
            // to dialog boxes created by MsgBox().
            SetSysConst("OKONLY", new Value(0));
            SetSysConst("OKCANCEL", new Value(1));
            SetSysConst("ABORTRETRYIGNORE", new Value(2));
            SetSysConst("YESNOCANCEL", new Value(3));
            SetSysConst("YESNO", new Value(4));
            SetSysConst("RETRYCANCEL", new Value(5));

            // These are used to dictate what icon is applied
            // to dialog boxes created by MsgBox();

            SetSysConst("CRITICAL", new Value(16));
            SetSysConst("QUESTION", new Value(32));
            SetSysConst("EXCLAMATION", new Value(48));
            SetSysConst("INFORMATION", new Value(64));
        }

        private void InitializeOnInputDict() {
            // set up the input slots as empty.
            OnInputInfo inInfo;
            inInfo.type = EventType.Empty;
            inInfo.jumpTarget = "";
            inInfo.assignmentName = "";
            inInfo.assignmentValue = new Value();
            inInfo.commandStr = "";

            // TODO No magic numbers!  Max # of inputs needs to come from a config file maybe?

            for (int x = 0; x < 8; x++)
                inputEvents.Add(x, inInfo);

        }
        private void ProcessFileException(Exception ex) {
            if (ex is ArgumentNullException) {
                Error("Path is null: " + ex.Message.ToString());
            } else if (ex is ArgumentException) {
                Error("Path is empty or contains invalid characters: " + ex.Message.ToString());
            } else if (ex is NotFiniteNumberException) {
                Error("Path refers to a non-file device: " + ex.Message.ToString());
            } else if (ex is System.Security.SecurityException) {
                Error("You don't have the required permissions to access a file in this path: " + ex.Message.ToString());
            } else if (ex is DirectoryNotFoundException) {
                Error("The specified path is invalid: " + ex.Message.ToString());
            } else if (ex is UnauthorizedAccessException) {
                Error("Access requested s not permitted for the specified path: " + ex.Message.ToString());
            } else if (ex is PathTooLongException) {
                Error("The specified path is too long: " + ex.Message.ToString());
            } else if (ex is IOException) {
                Error("An I/O Exception ocurred.: " + ex.Message.ToString());
            } else
                Error("Exception: " + ex.Message.ToString() + ": " + ex.StackTrace);
        }


        public Value GetVar(string name) {
            if (name.StartsWith("%(")) {
                if (!sysVars.ContainsKey(name))
                    throw new Exception("System Variable " + name + " does not exist.");
                return sysVars[name];
            } else {
                if (!vars.ContainsKey(name))
                    throw new Exception("Variable with name \"" + name + "\" does not exist.");
                return vars[name];
            }
        }

        public void SetVar(string name, Value val) {
            if (!lex.StrictMode && lex.KeyWordList.Contains(name.ToUpper())) {
                Error("When Strict Mode is off, " + name + " is a reserved word");
            }
            if (!vars.ContainsKey(name))
                vars.Add(name, val);
            else
                vars[name] = val;
        }

        public void SetSysVar(string name, Value val) {
            if (!sysVars.ContainsKey(name))
                sysVars.Add(name, val);
            else
                sysVars[name] = val;
        }

        public void SetSysConst(string name, Value val) {
            if (!sysConsts.ContainsKey(name))
                sysConsts.Add(name, val);
            else
                sysConsts[name] = val;
        }

        public void CloseAllUserFiles() {
            FileInfo workStream;
            foreach (KeyValuePair<int, FileInfo> entry in userFiles) {
                workStream = entry.Value; //userFiles[entry.Key];
                try {
                    switch (workStream.Mode) {
                        case "O":
                        case "A": {
                                // output & append are using Writer...
                                workStream.Writer.Flush();
                                workStream.Writer.Close();
                                workStream.Writer.Dispose();

                            }
                            break;
                        case "I": {
                                // input uses Reader
                                workStream.Reader.Close();
                                workStream.Reader.Dispose();
                            }
                            break;
                    }
                    if (workStream.fs != null)
                        workStream.fs.Dispose();

                } catch (Exception ex) {
                    ProcessFileException(ex);
                }
            }
            userFiles.Clear();
        }
        public void CloseUserFile(int fileNum) {
            if (!userFiles.ContainsKey(fileNum))
                Error("File # specified is not open");


            FileInfo workStream = userFiles[fileNum];
            try {
                switch (workStream.Mode) {
                    case "O":
                    case "A": {
                            // output & append are using Writer...
                            workStream.Writer.Flush();
                            workStream.Writer.Close();
                            workStream.Writer.Dispose();
                        }
                        break;
                    case "I": {
                            // input uses Reader
                            workStream.Reader.Close();
                            workStream.Reader.Dispose();
                        }
                        break;
                }
                if (workStream.fs != null)
                    workStream.fs.Dispose();

            } catch (Exception ex) {
                ProcessFileException(ex);
            }
            userFiles.Remove(fileNum);
        }

        public void OpenUserFile(int fileNum, string fileName, string mode) {
            FileInfo workStream = new FileInfo();

            fileName = @"" + fileName;

            // TODO need to check to see if the fileNum slot is open, if it is
            //      throw an error indicating the file # is in use.
            //      Don't calculate the next file slot, use fileNum as passed.
            if (userFiles.ContainsKey(fileNum))
                Error("File number in use");

            if (userFiles.Count < MAXFILES) {
                workStream.Mode = mode;
                switch (mode.ToUpper()) {
                    // TODO refactor the exception handling to call a routine called 
                    // ProcessOpenException()
                    case "O":  // output - Tested good as of 20Mar19 -gwb
                        try {
                            workStream.fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                            workStream.Writer = new StreamWriter(workStream.fs);
                            userFiles.Add(fileNum, workStream);
                        } catch (Exception ex) {
                            ProcessFileException(ex);
                        }
                        break;
                    case "I":  // input
                        try {
                            workStream.fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                            workStream.Reader = new StreamReader(workStream.fs);
                            userFiles.Add(fileNum, workStream);
                        } catch (Exception ex) {
                            ProcessFileException(ex);
                        }
                        break;
                    case "A":  // append - Tested good as of 20Mar19 -gwb
                        try {
                            workStream.fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                            workStream.Writer = new StreamWriter(workStream.fs);
                            userFiles.Add(fileNum, workStream);
                        } catch (Exception ex) {
                            ProcessFileException(ex);
                        }
                        break;
                    default:
                        Error("Invalid file open mode specified");
                        break;
                }
            } else {
                Error("Max # of file handles (" + MAXFILES.ToString() + ") have been opened");
            }

        }

        public void Pause() {
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            if (lastToken == Token.NewLine) { // Basic Pause dialog.
                string dialogText = "Continue ?";
                if (lex.CommentLine != "") {
                    dialogText = lex.CommentLine;
                }
                MessageBox.Show(null, dialogText, "Pause in File", buttons);
            }
            if (lastToken == Token.Value) {
                // this will be either a text prompt or a delay value.
                if (lex.Value.Type == ValueType.String) {
                    string dialogText = lex.Value.ToString();
                    GetNextToken();
                    if (lastToken == Token.NewLine) {
                        MessageBox.Show(null, dialogText, "Pause in File", buttons);
                        //GetNextToken();
                    } else if (lastToken == Token.Value) {
                        int delay = (int)lex.Value.Real;  // anything after the pause prompt must be numeric.
                        GetNextToken();
                        if (lastToken == Token.NewLine) {
                            MessageBox.Show(null, dialogText + " delay is " + delay.ToString() + " seconds.",
                        "Pause in File", buttons);
                            PauseDialogDelay(dialogText, delay);
                        }
                    } else if (lastToken == Token.Until) {
                        GetNextToken(); // grab the input #
                        if (lastToken == Token.Value) {
                            if (lex.Value.Type != ValueType.Real) {
                                Error("Switch # in PAUSE UNTIL must be numeric!");
                            }
                            int switchNum = (int)lex.Value.Real;
                            GetNextToken();
                            if (lastToken == Token.Comma) {
                                GetNextToken();
                                if (lastToken == Token.Value) {
                                    if (lex.Value.Type != ValueType.Real) {
                                        Error("Input state must be numeric!");
                                    }
                                    int switchState = (int)lex.Value.Real;
                                    //MessageBox.Show(null, String.Format("Switch {0}, State {1}.\n{2}", switchNum, switchState, dialogText), "Pause in File", buttons);
                                    PauseUntil(dialogText, switchNum, switchState);
                                    GetNextToken();
                                }
                            }
                        } else {
                            Error("Switch # is required in PAUSE UNTIL.");
                        }
                    }
                } else if (lex.Value.Type == ValueType.Real) {
                    // we've got a delay here and then the dialog will auto-dimiss.
                    int delay = (int)lex.Value.Real;
                    //MessageBox.Show(null, "We're pretending this will dismiss after " + delay.ToString() + " seconds.",
                    //    "Pause in File", buttons);
                    PauseDialogDelay(String.Format("Pausing for {0} seconds.", delay), delay);
                    GetNextToken();
                }
            } else if (lastToken == Token.Until) {
                // we've got a PAUSE UNTIL command here...
                // because lastToken is the Until keyword, we know there's no text prompt for this and we bring up a generic 
                // prompt...
                GetNextToken(); // grab the input #
                if (lastToken == Token.Value) {
                    if (lex.Value.Type != ValueType.Real) {
                        Error("Switch # in PAUSE UNTIL must be numeric!");
                    }
                    int switchNum = (int)lex.Value.Real;
                    GetNextToken();
                    if (lastToken == Token.Comma) {
                        GetNextToken();
                        if (lastToken == Token.Value) {
                            if (lex.Value.Type != ValueType.Real) {
                                Error("Input state must be numeric!");
                            }
                            int switchState = (int)lex.Value.Real;
                            Debug.Print("Switch {0}, State {1}.", switchNum, switchState);
                            MessageBox.Show(null, String.Format("Pausing for switch #{0} to be {1}.", switchNum, (switchState == 1) ? "On" : "Off"), "Pause in File", buttons);
                            GetNextToken();
                            PauseUntil(String.Format("Pausing for switch #{0} to be {1}.", switchNum, (switchState == 1) ? "On" : "Off"),
                                switchNum, switchState);
                        }
                    }
                } else {
                    Error("Switch # is required in PAUSE UNTIL.");
                }
            }
        }

        void PauseDialogDelay(string textPrompt, int delaySecs) {
            // TODO This needs to throw a dialog box up with the message of
            //      textPrompt and self-dismiss after delaySecs has elapsed.
            //
        }

        void PauseUntil(string textPrompt, int switchNum, int switchState) {
            // TODO This needs to do a number of things...
            // 1. Throw up a dialog box that shows the textPrompt value as well as
            //    an OK button and a Quit button.  The Quit button needs to terminate the whole program.
            // 2. It needs to check the controller to see if switchNum is already in switchState.
            //    if it is, then it needs to throw a dialog at the user about this error, or 
            //    crash the program as if it were a syntax error.
            // 3. If the switchState is not already indicated, then it needs to continually peek at the
            //    incoming data stream from the controller to see if switchNum has changed to inputState.
            //    if it has, then it needs to safely resume program operation (see Page 20 of the SBP Handbook),
            //    unless the SW [Set Warning] duration is zero.

        }

        public void AddFunction(string name, BasicFunction function) {
            if (!funcs.ContainsKey(name)) funcs.Add(name, function);
            else funcs[name] = function;
        }

        void Error(string text) {
            throw new Exception(text + " at line # " + lineMarker.Line + ".");
        }

        void Match(Token tok) {
            if (lastToken != tok)
                Error("Expected \"" + tok.ToString() + "\" got \"" + lastToken.ToString() + "\"");
        }

        public void Exec() {
            exit = false;
            GetNextToken();
            while (!exit)
                Line();
        }

        public Token GetNextToken() {
            prevToken = lastToken;
            lastToken = lex.GetToken();

            if (lastToken == Token.EOF && prevToken == Token.EOF)
                Error("Unexpected end of file");

            return lastToken;
        }

        public void Line() {
            while (lastToken == Token.NewLine) GetNextToken();

            if (lastToken == Token.EOF) {
                exit = true;
                return;
            }

            lineMarker = lex.TokenMarker;
            Statement();

            if (lastToken != Token.NewLine && lastToken != Token.EOF)
                Error("Expected new line got \"" + lastToken.ToString() + "\"");
        }

        void Statement() {
            Token keyword = lastToken;
            GetNextToken();
            switch (keyword) {
                case Token.Print: Print(); break;
                case Token.Input: Input(); break;
                case Token.Goto: Goto(); break;
                case Token.Gosub: GoSub(); break;
                case Token.Return: Return(); break;
                case Token.If: If(); break;
                case Token.Else: Else(); break;
                case Token.EndIf: break;
                case Token.For: For(); break;
                case Token.Next: Next(); break;
                case Token.Let: Let(); break;
                case Token.End: End(); break;
                case Token.OpenFile: OpenUserFile(); break;
                case Token.Close: CloseUserFile(); break;
                case Token.WriteFile: WriteToFile(); break;
                case Token.OnInput: OnInput(); break;
                case Token.Pause: Pause(); break;

                case Token.Identifer:
                    if (lastToken == Token.Equal) {
                        if (lex.StrictMode && (!lex.Identifier.StartsWith("&"))) {
                            Error("Strict OpenSBP enforcement enabled - variables must begin with '&'");
                        } else
                            Let();
                    } else if (lastToken == Token.Colon) Label();
                    else Expr();
                    break;
                case Token.SystemVar:
                    if (lastToken == Token.Equal) Let();
                    else if (lastToken == Token.Colon) Label();
                    else Expr();
                    break;

                case Token.EOF:
                    exit = true;
                    break;
                case Token.StrictOn:
                    if (!lex.StrictMode) {
                        lex.StrictMode = true;
                        //GetNextToken();
                        //Statement();
                    }
                    break;
                default:
                    Error("Expected keyword got \"" + keyword.ToString() + "\".");
                    break;
            }
            if (lastToken == Token.Colon) {
                GetNextToken();
                Statement();
            }
        }

        void CloseUserFile() {
            if (lastToken == Token.NewLine) {
                CloseAllUserFiles();
            } else {
                if (lastToken != Token.FileNumber)
                    Error("Syntax error = file # expected");

                if (lex.FileNumber == 0)
                    Error("Invalid file # specified");
                CloseUserFile(lex.FileNumber);
                GetNextToken();
            }
        }

        OnInputInfo ClearEvent() {
            OnInputInfo eventInfo;
            eventInfo.type = EventType.Empty;
            eventInfo.jumpTarget = "";
            eventInfo.assignmentName = "";
            eventInfo.assignmentValue = new Value();
            eventInfo.commandStr = "";
            return eventInfo;
        }
        OnInputInfo ConfigEvent(EventType eType, string paramStr) {
            OnInputInfo eventInfo;
            switch (eType) {
                case EventType.JumpTarget:
                    eventInfo.type = eType;
                    eventInfo.jumpTarget = paramStr;
                    eventInfo.assignmentName = "";
                    eventInfo.assignmentValue = new Value();
                    eventInfo.commandStr = "";
                    break;
                case EventType.Command:
                    eventInfo.type = eType;
                    eventInfo.jumpTarget = "";
                    eventInfo.assignmentName = "";
                    eventInfo.assignmentValue = new Value();
                    eventInfo.commandStr = paramStr;
                    break;
                default:
                    eventInfo.type = eType;
                    eventInfo.jumpTarget = "";
                    eventInfo.assignmentName = "";
                    eventInfo.assignmentValue = new Value();
                    eventInfo.commandStr = "";
                    Error("Invalid event type specified!");
                    break;
            }
            return (eventInfo);
        }

        OnInputInfo ConfigEvent(EventType eType, string varName, Value value) {
            OnInputInfo eventInfo;
            eventInfo.type = eType;
            eventInfo.jumpTarget = "";
            eventInfo.assignmentName = varName;
            eventInfo.assignmentValue = value;
            eventInfo.commandStr = "";

            return (eventInfo);
        }


        void OnInput() {
            // This one is special...
            // the first part of the command is ON INPUT(sw,ss)
            // where sw is a switch input an ss is a switch state.
            // sw can be from 0 to 7.  The max # of inputs can be increased by the firmware, but the ShopBot max is 8.
            // ss can be from 0 to 3.  The states are:
            // 0 - Input off
            // 1 - Input on
            // 2 - Input off, perform a ramped stop before returning control to the host.
            // 3 - Input on, perform a ramped stop before returning control to the host.
            //OnInputInfo eventInfo;
            int inputNum;
            int inputState;

            // string name = lex.Identifier;
            List<Value> args = new List<Value>();
            Match(Token.LParen);

            start:
            if (GetNextToken() != Token.RParen) {
                args.Add(Expr());
                if (lastToken == Token.Comma)
                    goto start;
            }
            inputNum = (int)args[0].Real;
            inputState = (int)args[1].Real;
            // at this point, the token should be RParen.

            GetNextToken();

            if (lastToken == Token.NewLine) {
                // if there's nothing after the right paren, we need to clear the specified event.
                inputEvents[inputNum] = ClearEvent();

                // now we tell the controller to clear that input event.
                if (!Comms.SendCommand("CLRINT " + inputNum.ToString())) {
                    Error("Unable to send command to controller!");
                }

            } else {
                if (lastToken == Token.Goto) {
                    GetNextToken();  // this should give us the jump target.
                }
                if (lastToken == Token.Identifer) {
                    //name = lex.Identifier;
                    GetNextToken();
                    if (lastToken == Token.NewLine) {
                        // we just record the name of the jump target here.  If or when then 
                        // input state is met, we'll process it as a "goto" at that point, passing in
                        // the name of the target to the goto code as if it was a command.

                        inputEvents[inputNum] = ConfigEvent(EventType.JumpTarget, lex.Identifier);
                        if (!Comms.SendCommand("SETINT " + inputNum.ToString() + " " + inputState.ToString())) {
                            Error("Unable to send command to controller!");
                        }
                        lastToken = Token.NewLine;

                    } else if (lastToken == Token.Equal) {
                        // we're processing a variable assignment.
                        GetNextToken();
                        inputEvents[inputNum] = ConfigEvent(EventType.Assignment, lex.Identifier, lex.Value);
                        GetNextToken();
                    } else if (lastToken == Token.Comma) {
                        if (lex.sbpList.Contains(lex.Identifier)) {
                            // we're processing a ShopBot command.    
                            StringBuilder cmdStr = new StringBuilder();
                            cmdStr.Append(lex.Identifier); // this will be the movement command.
                            cmdStr.Append(",");
                            // now we police up the values.  We need to handle both explicit values and variables...
                            while (GetNextToken() != Token.NewLine) {
                                if (lastToken == Token.Comma)
                                    cmdStr.Append(",");
                                if (lastToken == Token.Identifer)
                                    cmdStr.Append(vars[lex.Identifier].ToString());
                                else if (lastToken == Token.Value)
                                    cmdStr.Append(lex.Value.ToString());
                            }
                            inputEvents[inputNum] = ConfigEvent(EventType.Command, cmdStr.ToString());
                        }
                    }
                }

            }
        }
        void OpenUserFile() {
            // TODO a filename like "d:\test.txt" will throw an invalid character exception because
            //      of the single "\" character.  This needs to be handled!
            string fileName = Expr().ToString();
            string mode;
            if (lastToken != Token.For) {
                Error("Missing 'FOR' in OPEN statement.");
            }
            GetNextToken();
            // at this point lastToken /should/ be Token.Output, Token.Input, or Token.Append...
            switch (lastToken) {
                case Token.Output: mode = "O"; break;
                case Token.Input: mode = "I"; break;
                case Token.Append: mode = "A"; break;
                default:
                    mode = "";
                    Error("Invalid file access mode (" + lastToken.ToString() + ") specified");
                    break;
            }
            GetNextToken();
            if (lastToken != Token.As)
                Error("Syntax error - 'AS' expected");
            GetNextToken();
            if (lastToken != Token.FileNumber)
                Error("Syntax error = file # expected");

            if (lex.FileNumber == 0)
                Error("Invalid file # specified");

            OpenUserFile(lex.FileNumber, fileName, mode);
            GetNextToken(); // this will make lastToken = Token.NewLine...
        }

        void Print() {
            //Console.WriteLine(Expr().ToString());
            StringBuilder outputStr = new StringBuilder();

            while (lastToken != Token.NewLine) {
                switch (lastToken) {
                    // if the token is a comma, we write out 5 spaces between chunks that we get.
                    // if the token is a semicolon, we treat it the same as a "+" (or "&")
                    // if the last token is a newline and the prevtoken is a semicolon,
                    // we need to not write a cr/lf at the end of the line as we finish.
                    case Token.Comma: {
                            if (lastToken == Token.Comma)
                                outputStr.Append("     ");

                            GetNextToken();
                            if (lastToken == Token.Value || lastToken == Token.Identifer) {
                                outputStr.Append(Expr().ToString());

                                if (lastToken == Token.Comma)
                                    outputStr.Append("     ");

                                if (lastToken != Token.NewLine)
                                    GetNextToken();
                            }
                            break;
                        }
                    case Token.Identifer:
                    case Token.Value: // might work?
                        outputStr.Append(Expr().ToString());
                        break;
                    case Token.Semicolon:

                        GetNextToken();
                        break;
                    default:
                        Error("I don't know what to do with Token." + lastToken.ToString());
                        break;
                }

            }
            if (OutputWindow == null) {
            }

            if (prevToken == Token.Identifer) {
                Console.WriteLine(outputStr.ToString());
            } else if (prevToken == Token.Semicolon) {
                Console.Write(outputStr.ToString());
            } else if (prevToken == Token.Value)
                Console.WriteLine(outputStr.ToString());
        }

        void ReadFromFile() {
            if (lex.FileNumber == 0)
                Error("Invalid file # specified");

            GetNextToken();
            if (lastToken != Token.Comma)
                Error("Syntax error - missing ','");

            if (!userFiles.ContainsKey(lex.FileNumber))
                Error("Invalid File # specified - file not open");

            FileInfo workStream = userFiles[lex.FileNumber];
            if (workStream.Reader == null) {
                Error("File #" + lex.FileNumber + " is not open for reading!");

            }
            GetNextToken();
            //inputStr = Expr(inputMode: true).ToString();
            StringBuilder inputStr = new StringBuilder();
            string resultStr = "";
            double valDouble;
            bool quoteStart = false;
            bool commaSep = false;
            char workCh;

            while (true) {
                Match(Token.Identifer);

                if (!vars.ContainsKey(lex.Identifier))
                    vars.Add(lex.Identifier, new Value());

                // if a line in a file we're reading (after any whitespace) begins with a comma,
                // we need to assign the entire line contents to a single variable.
                // If however, the line is not "quoted", we need to assign up to the occurance of a comma
                // to the variable in the input statement, and subsequent variables as shown on page # 15
                // of the 2015 ShopBot Programming Handbook.
                inputStr.Clear();
                valDouble = 0.0;
                quoteStart = false;


                //string input = workStream.Reader.ReadLine();
                while (true) {
                    // TODO - Need to encapsulate disk reads with try..catch.
                    workCh = (char)workStream.Reader.Read();
                    if (workCh == '\n')
                        break;
                    if (workCh == '"') {// we read until we hit a newline, and we don't include the quotes in the data.
                        quoteStart = true;
                        // we need to strip any leading whitespace if we've accumulated any - this makes the result conform
                        // to how the ShopBot interpreter works.
                        string holdStr = inputStr.ToString().TrimStart();
                        inputStr.Clear();
                        inputStr.Append(holdStr);
                    }
                    if (workCh != '"') {
                        if (quoteStart) // we add all characters except newline or a quote since we're grabbing it all.
                            inputStr.Append(workCh);
                        else {
                            if (workCh == ',') { // we don't add the comma to the read value.
                                commaSep = true;
                                break;
                            } else
                                inputStr.Append(workCh);
                        }
                    }
                }

                if (commaSep) {
                    resultStr = inputStr.ToString().Trim();
                } else {
                    resultStr = inputStr.ToString();
                }

                if (double.TryParse(resultStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out valDouble))
                    vars[lex.Identifier] = new Value(valDouble);
                else
                    vars[lex.Identifier] = new Value(resultStr);

                GetNextToken();
                if (lastToken != Token.Comma) break;
                GetNextToken();
            }
        }

        void WriteToFile() {
            StringBuilder outputStr = new StringBuilder();

            if (lastToken != Token.FileNumber)
                Error("Syntax error = file # expected");

            if (lex.FileNumber == 0)
                Error("Invalid file # specified");

            GetNextToken();
            if (lastToken != Token.Comma && lastToken != Token.Semicolon)
                Error("Syntax error - missing ',' or ';'");

            if (!userFiles.ContainsKey(lex.FileNumber))
                Error("Invalid File # specified - file not open");

            FileInfo workStream = userFiles[lex.FileNumber];

            GetNextToken();
            outputStr.Append(Expr().ToString());

            while (lastToken != Token.NewLine) {
                switch (lastToken) {
                    // if the token is a comma, we write out ", " between chunks that we get.
                    // if the token is a semicolon, we treat it the same as a "+" (or "&")
                    // if the last token is a newline and the prevtoken is a semicolon,
                    // we need to not write a cr/lf at the end of the line as we finish.
                    case Token.Comma: {
                            if (lastToken == Token.Comma)
                                outputStr.Append(", ");

                            GetNextToken();
                            if (lastToken == Token.Value || lastToken == Token.Identifer) {
                                outputStr.Append(Expr().ToString());

                                if (lastToken == Token.Comma)
                                    outputStr.Append(", ");

                                if (lastToken != Token.NewLine)
                                    GetNextToken();
                            }
                            break;
                        }
                    case Token.Identifer:
                    case Token.Value: // might work?
                        outputStr.Append(Expr().ToString());
                        break;
                    case Token.Semicolon:
                        GetNextToken();
                        break;
                    default:
                        Error("I don't know what to do with Token." + lastToken.ToString());
                        break;
                }

            }
            if (prevToken == Token.Identifer) {
                workStream.Writer.WriteLine(outputStr.ToString());
            } else if (prevToken == Token.Semicolon) {
                workStream.Writer.Write(outputStr.ToString());
            } else if (prevToken == Token.Value)
                workStream.Writer.WriteLine(outputStr.ToString());
        }

        void Input() {
            if (lastToken == Token.FileNumber) {
                ReadFromFile();
            } else {
                while (true) {
                    Match(Token.Identifer);

                    if (!vars.ContainsKey(lex.Identifier)) vars.Add(lex.Identifier, new Value());

                    string input = Console.ReadLine();
                    double d;
                    if (double.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d))
                        vars[lex.Identifier] = new Value(d);
                    else
                        vars[lex.Identifier] = new Value(input);

                    GetNextToken();
                    if (lastToken != Token.Comma) break;
                    GetNextToken();
                }
            }
        }

        void Goto() {
            Match(Token.Identifer);
            string name = lex.Identifier;

            if (!labels.ContainsKey(name)) {
                while (true) {
                    if (GetNextToken() == Token.Colon && prevToken == Token.Identifer) {
                        if (!labels.ContainsKey(lex.Identifier))
                            labels.Add(lex.Identifier, lex.TokenMarker);
                        if (lex.Identifier == name)
                            break;
                    }
                    if (lastToken == Token.EOF) {
                        Error("Cannot find label named \"" + name + "\".");
                    }
                }
            }
            lex.GoTo(labels[name]);
            lastToken = Token.NewLine;
        }

        void GoSub() {
            // store our current location...
            lex.GoSubPush();
            Goto(); // Leverage the existing jump code.
        }

        void Return() {
            Marker returnMarker = lex.Return();
            lex.GoTo(new Marker(returnMarker.Pointer - 1, returnMarker.Line, returnMarker.Column - 1));
            lastToken = Token.NewLine;

        }

        void If() {
            bool result = (Expr().BinOp(new Value(0), Token.Equal).Real == 1);

            Match(Token.Then);
            GetNextToken();

            if (result) {
                int i = ifcounter;
                while (true) {
                    if (lastToken == Token.If) {
                        i++;
                    } else if (lastToken == Token.Else) {
                        if (i == ifcounter) {
                            GetNextToken();
                            return;
                        }
                    } else if (lastToken == Token.EndIf) {
                        if (i == ifcounter) {
                            GetNextToken();
                            return;
                        }
                        i--;
                    }
                    GetNextToken();
                }
            }
        }

        void Else() {
            int i = ifcounter;
            while (true) {
                if (lastToken == Token.If) {
                    i++;
                } else if (lastToken == Token.EndIf) {
                    if (i == ifcounter) {
                        GetNextToken();
                        return;
                    }
                    i--;
                }
                GetNextToken();
            }
        }

        void Label() {
            string name = lex.Identifier;
            if (!labels.ContainsKey(name)) labels.Add(name, lex.TokenMarker);

            GetNextToken();
            Match(Token.NewLine);
        }

        void End() {
            CloseAllUserFiles(); //ensures any open files get closed and their handles are disposed properly.
            exit = true;
        }

        void Let() {
            if (lastToken != Token.Equal) {
                Match(Token.Identifer);
                GetNextToken();
                Match(Token.Equal);
            }

            string id = lex.Identifier;

            GetNextToken();

            SetVar(id, Expr());
        }

        void For() {
            Match(Token.Identifer);
            string userVar = lex.Identifier;

            GetNextToken();
            Match(Token.Equal);

            GetNextToken();
            Value v = Expr();

            if (loops.ContainsKey(userVar)) {
                loops[userVar] = lineMarker;
            } else {
                SetVar(userVar, v);
                loops.Add(userVar, lineMarker);
            }

            Match(Token.To);

            GetNextToken();
            v = Expr();

            if (vars[userVar].BinOp(v, Token.More).Real == 1) {
                while (true) {
                    while (!(GetNextToken() == Token.Identifer && prevToken == Token.Next)) ;
                    if (lex.Identifier == userVar) {
                        loops.Remove(userVar);
                        GetNextToken();
                        Match(Token.NewLine);
                        break;
                    }
                }
            }

        }

        void Next() {
            Match(Token.Identifer);
            string userVar = lex.Identifier;
            vars[userVar] = vars[userVar].BinOp(new Value(1), Token.Plus);
            lex.GoTo(new Marker(loops[userVar].Pointer - 1, loops[userVar].Line, loops[userVar].Column - 1));
            lastToken = Token.NewLine;
        }

        Value Expr(int min = 0) {
            Dictionary<Token, int> precedence = new Dictionary<Token, int>()
            {
                { Token.Or, 0 }, { Token.And, 0 },
                { Token.Equal, 1 }, { Token.NotEqual, 1 },
                { Token.Less, 1 }, { Token.More, 1 },
                { Token.LessEqual, 1 },  { Token.MoreEqual, 1 },
                { Token.Plus, 2 }, { Token.Minus, 2 }, {Token.Ampersand, 2}, /* { Token.Comma, 2 }, */
                { Token.Asterisk, 3 }, {Token.Slash, 3 },
                { Token.Caret, 4 }
            };

            Value lhs = Primary();

            while (true) {
                // When the string concat operator & is being used, we need to treat it as much as the + operator as we can.

                if ((lastToken < Token.Plus && lastToken < Token.Ampersand) ||
                    (lastToken < Token.Ampersand) || lastToken > Token.And || precedence[lastToken] < min)
                    break;

                Token op = lastToken;
                int prec = precedence[lastToken];

                int assoc = 0; // 0 left, 1 right
                int nextmin = assoc == 0 ? prec : prec + 1;

                GetNextToken();
                Value rhs = Expr(nextmin);
                lhs = lhs.BinOp(rhs, op);
            }

            return lhs;
        }

        Value Primary() {
            Value prim = Value.Zero;

            if (lastToken == Token.Value) {
                prim = lex.Value;
                GetNextToken();
            } else if ((lastToken == Token.Identifer)) {
                if (vars.ContainsKey(lex.Identifier)) {
                    prim = vars[lex.Identifier];
                } else if (sysVars.ContainsKey(lex.Identifier)) {
                    prim = sysVars[lex.Identifier];
                } else if (sysConsts.ContainsKey(lex.Identifier.ToUpper())) {
                    prim = sysConsts[lex.Identifier.ToUpper()];
                } else if (funcs.ContainsKey(lex.Identifier.ToLower())) {
                    string name = lex.Identifier.ToLower();
                    List<Value> args = new List<Value>();
                    GetNextToken();
                    Match(Token.LParen);

                    start:
                    if (GetNextToken() != Token.RParen) {
                        args.Add(Expr());
                        if (lastToken == Token.Comma)
                            goto start;
                    }

                    prim = funcs[name](null, args);
                } else {
                    if (lex.StrictMode && (!lex.Identifier.StartsWith("&"))) {
                        Error("Strict OpenSBP enforcement enabled - variables must begin with '&'.");
                    } else
                        Error("Undeclared variable \"" + lex.Identifier + "\"");
                }
                GetNextToken();
            } else if (lastToken == Token.SystemVar) {
                // Process the OpenSBP System Variable....
                if (sysVars.ContainsKey(lex.Identifier)) {
                    prim = sysVars[lex.Identifier];
                } else if (funcs.ContainsKey(lex.Identifier)) {
                    string name = lex.Identifier;
                    List<Value> args = new List<Value>();
                    GetNextToken();
                    Match(Token.LParen);

                    start:
                    if (GetNextToken() != Token.RParen) {
                        args.Add(Expr());
                        if (lastToken == Token.Comma)
                            goto start;
                    }

                    prim = funcs[name](null, args);
                } else {
                    Error("Unknown System Variable \"" + lex.Identifier + "\"");
                }
                GetNextToken();
            } else if (lastToken == Token.LParen) {
                GetNextToken();
                prim = Expr();
                Match(Token.RParen);
                GetNextToken();
            } else if (lastToken == Token.Plus || lastToken == Token.Ampersand || lastToken == Token.Minus) {
                Token op = lastToken;
                GetNextToken();
                prim = Value.Zero.BinOp(Primary(), op); // we dont realy have a unary operators
            } else {
                Error("Unexpected token in primary!");
            }

            return prim;
        }
    }
}
