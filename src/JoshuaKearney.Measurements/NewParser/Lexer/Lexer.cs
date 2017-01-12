using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshuaKearney.Measurements.NewParser.Lexer {
    public class Lexer {
        public IEnumerable<Token> GetTokens(string text) {
            char[] arr = text.ToCharArray();
            int position = 0;
            char currentChar = arr.ElementAtOrDefault(position);
            List<Token> allToks = new List<Token>();

            Func<bool> Advance = () => {
                position++;
                currentChar = arr.ElementAtOrDefault(position);

                return currentChar != '\0';
            };

            Action Retract = () => {
                position--;
                currentChar = arr.ElementAtOrDefault(position);
            };

            do {
                if (char.IsDigit(currentChar)) {
                    string numStr = currentChar.ToString();
                    double num = double.Parse(numStr);

                    while (Advance() && (char.IsDigit(currentChar) || currentChar == '.')) {
                        string temp = numStr + currentChar;

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
                else if (char.IsLetter(currentChar)) {
                    string numStr = currentChar.ToString();

                    while (Advance() && char.IsLetter(currentChar)) {
                        numStr = numStr + currentChar;   
                    }

                    // Found the first non letter char, back step to process it on the next go
                    Retract();

                    allToks.Add(new IdToken(numStr));
                }
                else if (currentChar == '+') {
                    allToks.Add(Token.Plus);
                }
                else if (currentChar == '-') {
                    allToks.Add(Token.Minus);
                }
                else if (currentChar == '*') {
                    allToks.Add(Token.Askerisk);
                }
                else if (currentChar == '/') {
                    allToks.Add(Token.ForwardSlash);
                }
                else if (currentChar == '^') {
                    allToks.Add(Token.Caret);
                }
                else if (currentChar == '(') {
                    allToks.Add(Token.OpenParen);
                }
                else if (currentChar == ')') {
                    allToks.Add(Token.CloseParen);
                }
                else if (char.IsWhiteSpace(currentChar)) {
                    continue;
                }
                else {
                    break;
                }
            } while (Advance());

            return allToks;
        }
    }
}