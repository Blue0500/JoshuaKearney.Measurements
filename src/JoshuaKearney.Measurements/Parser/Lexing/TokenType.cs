using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser.Lexing {
    internal enum TokenType {
        EOF,
        Number,
        Plus,
        Minus,
        Asterisk,
        ForwardSlash,
        Caret,
        OpenParen,
        CloseParen,
        Id,
        OpenBracket,
        CloseBracket
    }
}
