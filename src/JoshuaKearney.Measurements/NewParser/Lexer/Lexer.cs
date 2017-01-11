using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshuaKearney.Measurements.NewParser.Lexer {
    public class Lexer {
       // public int Position { get; private set; } = 0;
       // public IEnumerable<char> Text { get; }

     //   public char CurrentChar => this.Text.ElementAtOrDefault(this.Position);

      //  public char NextChar => this.Text.ElementAtOrDefault(this.Position + 1);

        public bool Advance() {
            this.Position++;

            if (this.Position >= this.Text.Count()) {
                return false;
            }
            else {
                return true;
            }
        }

        public void Retract() {
            this.Position--;
        }

        public Lexer(string text) {
            this.Text = text.ToCharArray();
        }

        private IEnumerable<Token> GetTokensCore(string text) {
            char[] arr = text.ToCharArray();
            int position = 0;
            char currentchar = arr.ElementAtOrDefault(position);
            List<Token> allToks = new List<Token>();

            do {
                if (char.IsDigit(currentChar)) {
                    string numStr = this.CurrentChar.ToString();
                    double num = double.Parse(numStr);

                    while (this.Advance() && (char.IsDigit(this.CurrentChar) || this.CurrentChar == '.')) {
                        string temp = numStr + this.CurrentChar;

                        double res;
                        if (!double.TryParse(temp, out res)) {
                            break;
                        }
                        else {
                            numStr = temp;
                            num = res;
                        }
                    }

                    // Found the first non number char, back step to process it on the next go
                    this.Retract();

                    yield return new NumberToken(num);
                }
                else if (this.CurrentChar == '+') {
                    yield return Token.Plus;
                }
                else if (this.CurrentChar == '-') {
                    yield return Token.Minus;
                }
                else if (this.CurrentChar == '*') {
                    yield return Token.Askerisk;
                }
                else if (this.CurrentChar == '/') {
                    yield return Token.ForwardSlash;
                }
                else if (this.CurrentChar == '(') {
                    yield return Token.OpenParen;
                }
                else if (this.CurrentChar == ')') {
                    yield return Token.CloseParen;
                }
                else if (char.IsWhiteSpace(this.CurrentChar)) {
                    continue;
                }
                else {
                    break;
                }
            } while (this.Advance());

            yield return Token.EOF;
        }
    }
}