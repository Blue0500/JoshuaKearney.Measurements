using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements.Parser.Lexing {

    internal class Lexer {
        private static NumberParser numParser { get; } = new NumberParser();

        private char[] text;
        private int position = 0;
        private object lockObj = new object();

        private char CurrentChar => this.text.ElementAtOrDefault(this.position);

        private bool Advance() {
            this.position++;
            return this.CurrentChar != '\0';
        }

        private bool Retract() {
            this.position--;
            return this.CurrentChar != '\0';
        }

        public bool TryGetTokens(string text, out IEnumerable<Token> success, out ParseException failure) {
            Validate.NonNull(text, nameof(text));

            lock (this.lockObj) {
                this.text = text.ToCharArray();
                this.position = 0;

                List<Token> allToks = new List<Token>();

                do {
                    if (char.IsDigit(this.CurrentChar) || this.CurrentChar == '.') {
                        char start = this.CurrentChar;

                        if (numParser.Parse(this.text, this.position, out double ret, out this.position)) {
                            allToks.Add(new NumberToken(ret));
                            this.Retract();
                        }
                        else {
                            success = null;
                            failure = ParseException.NumberParseFailed(start);
                            return false;
                        }
                    }
                    else if (char.IsLetter(this.CurrentChar)) {
                        string numStr = this.CurrentChar.ToString();

                        while (Advance() && char.IsLetter(this.CurrentChar)) {
                            numStr = numStr + this.CurrentChar;
                        }

                        // Found the first non letter char, back step to process it on the next go
                        Retract();

                        allToks.Add(new IdToken(numStr));
                    }
                    else if (char.IsWhiteSpace(this.CurrentChar)) {
                        continue;
                    }
                    else {
                        switch (this.CurrentChar) {
                            case '+': allToks.Add(Token.Plus); break;
                            case '-': allToks.Add(Token.Minus); break;
                            case '*': {
                                this.Advance();

                                if (this.CurrentChar == '*') {
                                    allToks.Add(Token.Caret);
                                }
                                else {
                                    allToks.Add(Token.Askerisk);
                                    this.Retract();
                                }

                                break;
                            }
                            case '/': allToks.Add(Token.ForwardSlash); break;
                            case '^': allToks.Add(Token.Caret); break;
                            case '(': allToks.Add(Token.OpenParen); break;
                            case ')': allToks.Add(Token.CloseParen); break;
                            case '[': allToks.Add(Token.OpenBracket); break;
                            case ']': allToks.Add(Token.CloseBracket); break;
                            default: {
                                    success = null;
                                    failure = ParseException.UnexpectedCharactersError(this.CurrentChar.ToString());
                                    return false;
                                }
                        }
                    }
                } while (this.Advance());

                success = allToks;
                failure = null;
                return true;
            }
        }
    }
}