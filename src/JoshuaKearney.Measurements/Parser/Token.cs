using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {

    internal class Token {
        public string Value { get; }

        public Token(string value) {
            this.Value = value;
        }

        public override string ToString() {
            return this.Value;
        }
    }
}