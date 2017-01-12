using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.NewParser.Lexer {
    public class NumberToken : Token {
        public double Value { get; }

        public NumberToken(double value) : base(TokenType.Number, value.ToString()) {
            this.Value = value;
        }
    }
}
