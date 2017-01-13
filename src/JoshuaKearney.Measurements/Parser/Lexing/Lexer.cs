using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshuaKearney.Measurements.Parser.Lexing {
    internal class Lexer {
        private char[] text;
        private int position = 0;
        private object lockObj = new object();

        private char CurrentChar => text.ElementAtOrDefault(this.position);

        private bool Advance() {
            position++;
            return this.CurrentChar != '\0';
        }

        private bool Retract() {
            position--;
            return this.CurrentChar != '\0';
        }

        public bool TryGetTokens(string text, out IEnumerable<Token> success, out ParseException failure) {
            lock (this.lockObj) {
                this.text = text.ToCharArray();
                this.position = 0;

                List<Token> allToks = new List<Token>();

                do {
                    if (char.IsDigit(CurrentChar)) {
                        string numStr = CurrentChar.ToString();
                        double num = double.Parse(numStr);

                        while (this.Advance() && (char.IsDigit(CurrentChar) || CurrentChar == '.')) {
                            string temp = numStr + CurrentChar;

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
                        Retract();

                        allToks.Add(new NumberToken(num));
                    }
                    else if (char.IsLetter(CurrentChar)) {
                        string numStr = CurrentChar.ToString();

                        while (Advance() && char.IsLetter(CurrentChar)) {
                            numStr = numStr + CurrentChar;
                        }

                        // Found the first non letter char, back step to process it on the next go
                        Retract();

                        allToks.Add(new IdToken(numStr));
                    }
                    else if (CurrentChar == '+') {
                        allToks.Add(Token.Plus);
                    }
                    else if (CurrentChar == '-') {
                        allToks.Add(Token.Minus);
                    }
                    else if (CurrentChar == '*') {
                        allToks.Add(Token.Askerisk);
                    }
                    else if (CurrentChar == '/') {
                        allToks.Add(Token.ForwardSlash);
                    }
                    else if (CurrentChar == '^') {
                        allToks.Add(Token.Caret);
                    }
                    else if (CurrentChar == '(') {
                        allToks.Add(Token.OpenParen);
                    }
                    else if (CurrentChar == ')') {
                        allToks.Add(Token.CloseParen);
                    }
                    else if (char.IsWhiteSpace(CurrentChar)) {
                        continue;
                    }
                    else {
                        success = null;
                        failure = ParseException.UnexpectedCharactersError(this.CurrentChar.ToString());
                    }
                } while (Advance());

                success = allToks;
                failure = null;
                return true;
            }
        }
    }
}