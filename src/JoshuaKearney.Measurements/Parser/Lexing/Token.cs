using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser.Lexing {
    internal class Token {
        public static Token Plus { get; } = new Token(TokenType.Plus, "+");
        public static Token Minus { get; } = new Token(TokenType.Minus, "-");
        public static Token Askerisk { get; } = new Token(TokenType.Asterisk, "*");
        public static Token ForwardSlash { get; } = new Token(TokenType.ForwardSlash, "/");
        public static Token Caret { get; } = new Token(TokenType.Caret, "^");
        public static Token OpenParen { get; } = new Token(TokenType.OpenParen, "(");
        public static Token CloseParen { get; } = new Token(TokenType.CloseParen, ")");
        public static Token EOF { get; } = new Token(TokenType.EOF, "");

        public TokenType Type { get; }

        public string StringValue { get; }

        public Token(TokenType type, string str) {
            Validate.NonNull(type, nameof(type));
            Validate.NonNull(str, nameof(str));

            this.Type = type;
            this.StringValue = str;
        }

        public override string ToString() {
            return this.StringValue;
        }
    }
}
