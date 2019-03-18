namespace OpenSBP
{
    public enum Token
    {
        Unknown,

        Identifer,
        Value,
        SystemVar,

        // Keyword list.  Any changes to this list must also be made to the
        // keywordList list in the Lexer class!

        Print,  // TODO OpenSBP has special behaviors for the PRINT command.
                // See Pages 21-22
        If,     // TODO OpenSBP has some weird restrictions with the IF keyword.  See 
                // Page 13 for more info.
        EndIf,  // also End If
        Then,
        Else,
        For,
        To,
        Next,
        Goto,
        Input,
        Let,
        Gosub,
        Return,
        Rem,
        End,

        // New keywords for OpenSBP
        // TODO There are a number of mathematical operations that OpenSBP support.  See the "Operations" list.
              
        Close,       // Page 12
        EndAll,      // Page 12
        ExitShopBot, // Page 13
        InputFile,   // Page 15 - This is the Input keyword used to read data from files.
        InputUser,   // Page 14 - This is the Input keyword used to display a message box and return up to 
                     // ten variables.
        MsgBox,      // Page 15-17
        OnInput,     // Page 17-18
        OpenFile,    // Page 18
        Pause,       // Page 19-20
        Play,        // Page 20-21
        Shell,       // Page 23-24
        WarningOff,  // Page 25
        WriteFile,   // Pages 25-26
        
        //  These are special registry commands that allow reading/writing to the ShopBot area of the Windows registry.
        //  This should probably be configured to read/write data from an ini file of some sort, as Linux has no "registry".
        SetUsrVal,   // Page 26
        SetUsrValClrd, // Page 26
        SetSpindlestatus, // Page 26
        GetUsrVal,   // Page 26
        GetUsrValClrd, // Page 26
        GetUsrPath,  // Page 26
        GetAppPath,  // Page 27
        GetPartFileName, // Page 27
        GetSpindleStatus, // Page 28


        NewLine,
        Colon,
        Semicolon,
        Comma,

        Ampersand,  // OpenSBP uses "&" for string concatenation.
        Plus,
        Minus,
        Slash,
        Asterisk,
        Caret,
        Equal,
        Less,
        More,
        NotEqual,
        LessEqual,
        MoreEqual,
        Or,
        And,
        Not,

        LParen,
        RParen,

        EOF = -1   //End Of File
    }       
}
