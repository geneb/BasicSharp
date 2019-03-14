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

        Print,
        If,
        EndIf, // also End If
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
