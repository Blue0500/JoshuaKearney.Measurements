using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.NewParser.Lexer {
    public class IdToken : Token {
        public string Value { get; }

        public IdToken(string value) : base(TokenType.Id, value) {
            this.Value = value;
        }
    }
}
