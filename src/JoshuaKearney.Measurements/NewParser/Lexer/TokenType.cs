using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.NewParser.Lexer {
    public enum TokenType {
        EOF,
        Number,
        Plus,
        Minus,
        Asterisk,
        ForwardSlash,
        Caret,
        OpenParen,
        CloseParen,
        Id
    }
}
