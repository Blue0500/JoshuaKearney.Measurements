using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace JoshuaKearney.Measurements.Parser {
    public class NumberParser {
        private IEnumerable<char> text;
        private int pos;
        private readonly char[] hexNums = "0123456789abcdefABCDEF".ToCharArray();
        private readonly char[] octNums = "01234567".ToCharArray();
        private readonly char[] binNums = "01".ToCharArray();

        private char CurrentChar => this.text.ElementAtOrDefault(this.pos);

        private void Advance() {
            this.pos++;
        }

        public NumberParser() { }

        public bool Parse(IEnumerable<char> str, int start, out double success, out int newPos) {
            this.text = str;
            this.pos = start;

            bool worked = this.Literal(out double ret);

            success = ret;
            newPos = this.pos;

            return worked;
        }

        private bool Literal(out double success) {
            //if (this.CurrentChar == '0') {
            //    this.Advance();

            //    if(this.CurrentChar == 'x') {
            //        return this.NonDecimalLiteral(hexNums, 'x', out success);
            //    }
            //    else if (this.CurrentChar == 'c') {
            //        return this.NonDecimalLiteral(octNums, 'c', out success);
            //    }
            //    else if (this.CurrentChar == 'b') {
            //        return this.NonDecimalLiteral(binNums, 'b', out success);
            //    }
            //}

            return this.DecimalLiteral(out success);
        }

        private bool NonDecimalLiteral(char[] charset, char prefix, out double success) {
            string num = "0" + prefix;

            if (this.CurrentChar != prefix) {
                success = 0;
                return false;
            }

            this.Advance();

            while (charset.Contains(this.CurrentChar)) {
                num += this.CurrentChar;
                this.Advance();
            }

            bool worked = long.TryParse(
    num,
    NumberStyles.AllowHexSpecifier,
    CultureInfo.InvariantCulture,
    out long ret
);

            success = ret;
            return worked;
        }

        private bool DecimalLiteral(out double success) {
            string num = "";

            while (char.IsDigit(this.CurrentChar)) {
                num += this.CurrentChar;
                this.Advance();
            }

            if (this.CurrentChar == '.') {
                this.Advance();
                num += '.';

                while (char.IsDigit(this.CurrentChar)) {
                    num += this.CurrentChar;
                    this.Advance();
                }
            }

            if (this.CurrentChar == '.') {
                success = 0;
                return false;
            }

            return double.TryParse(num, out success);
        }
    }
}