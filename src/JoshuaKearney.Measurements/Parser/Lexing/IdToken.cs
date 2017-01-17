using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser.Lexing {
    internal class IdToken : Token {
        public string Value { get; }

        public IdToken(string value) : base(TokenType.Id, value) {
            Validate.NonNull(value, nameof(value));

            this.Value = value;
        }
    }
}
