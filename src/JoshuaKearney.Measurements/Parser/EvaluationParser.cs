using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser.Lexing;

namespace JoshuaKearney.Measurements.Parser {
    internal class EvaluationParser {
        private IEnumerable<Token> Tokens;
        private IEnumerable<Operator> AllOperators;
        private Dictionary<string, object> Units;
        private int pos = 0;
        private object lockObj = new object();

        private Token CurrentToken => this.Tokens.ElementAtOrDefault(this.pos) ?? Token.EOF;

        private IEnumerable<TokenType> AtomStartTypes { get; } = new[] { TokenType.OpenParen, TokenType.Number, TokenType.Id };

        private bool Advance(TokenType type, out ParseException failure) {
            Validate.NonNull(type, nameof(type));

            if (this.CurrentToken.Type != type) {
                failure = ParseException.UnexpectedCharactersError(this.CurrentToken.ToString());
                return false;
            }
            else {
                this.pos++;
                failure = null;
                return true;
            }
        }

        public bool TryParse(
            IEnumerable<Token> tokens,
            IEnumerable<Operator> ops,
            Dictionary<string, object> units,
            out object success, 
            out ParseException failure) {

            Validate.NonNull(tokens, nameof(tokens));
            Validate.NonNull(ops, nameof(ops));
            Validate.NonNull(units, nameof(units));

            lock (this.lockObj) {
                this.pos = 0;
                this.Tokens = tokens;
                this.AllOperators = ops;
                this.Units = units;

                return this.AddExpression(out success, out failure);
            }
        }

        private bool AddExpression(out object success, out ParseException failure) {
            if (!this.MultExpression(out success, out failure)) {
                return false;
            }
            else {
                while (this.CurrentToken.Type == TokenType.Plus || this.CurrentToken.Type == TokenType.Minus) {
                    Token prev = this.CurrentToken;

                    if (!this.Advance(this.CurrentToken.Type, out failure)) {
                        success = null;
                        return false;
                    }
                    else if (!this.MultExpression(out object next, out failure)) {
                        success = null;
                        return false;
                    }
                    else if (prev.Type == TokenType.Plus) {

                        if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Addition, success, next, out object temp, out failure)) {
                            if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Addition, next, success, out temp, out failure)) {
                                return false;
                            }
                        }

                        success = temp;
                    }
                    else {
                        if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Subtraction, success, next, out success, out failure)) {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        private bool MultExpression(out object success, out ParseException failure) {
            if (!this.PowExpression(out success, out failure)) {
                return false;
            }
            else {
                while (this.AtomStartTypes.Any(x => this.CurrentToken.Type == x) || this.CurrentToken.Type == TokenType.ForwardSlash || this.CurrentToken.Type == TokenType.Asterisk) {
                    Token prev = this.CurrentToken;
                    object first = success;

                    if (prev.Type == TokenType.ForwardSlash || prev.Type == TokenType.Asterisk) {
                        if (!this.Advance(this.CurrentToken.Type, out failure)) {
                            success = null;
                            return false;
                        }
                    }

                    if (!this.PowExpression(out object next, out failure)) {
                        success = null;
                        return false;
                    }
                    else {
                        if (prev.Type == TokenType.ForwardSlash) {
                            if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Division, first, next, out success, out failure)) {
                                return false;
                            }
                        }
                        else {

                            if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Multiplication, first, next, out object temp, out failure)) {
                                if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Multiplication, next, first, out temp, out failure)) {
                                    return false;
                                }
                            }

                            success = temp;
                        }
                    }
                }

                return true;
            }
        }

        private bool PowExpression(out object success, out ParseException failure) {
            if (!this.Atom(out success, out failure)) {
                return false;
            }
            else {
                while (this.CurrentToken.Type == TokenType.Caret) {
                    Token prev = this.CurrentToken;

                    if (!this.Advance(TokenType.Caret, out failure)) {
                        success = null;
                        return false;
                    }
                    else if (!this.Atom(out object next, out failure)) {
                        success = null;
                        return false;
                    }
                    else {
                        if (!Evaluation.ApplyBinaryOperator(this.AllOperators, BinaryOperatorType.Exponation, success, next, out success, out failure)) {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        private bool Atom(out object success, out ParseException failure) {
            if (this.CurrentToken.Type == TokenType.Number) {
                NumberToken tok = this.CurrentToken as NumberToken;

                if (!this.Advance(TokenType.Number, out failure)) {
                    success = null;
                    return false;
                }
                else {
                    success = new DoubleMeasurement(tok.Value);
                    failure = null;
                    return true;
                }
            }
            else if (this.CurrentToken.Type == TokenType.Id) {
                IdToken tok = this.CurrentToken as IdToken;

                if (!this.Advance(TokenType.Id, out failure)) {
                    success = null;
                    return false;
                }
                else {
                    if (this.Units.ContainsKey(tok.StringValue)) {
                        success = this.Units[tok.StringValue];
                        return true;
                    }
                    else {
                        success = null;
                        failure = ParseException.UndefinedUnitDiscovered(tok.StringValue);
                        return false;
                    }
                }
            }
            else if (this.CurrentToken.Type == TokenType.OpenParen) {
                if (!this.Advance(TokenType.OpenParen, out failure)) {
                    success = null;
                    return false;
                }
                else if (!this.AddExpression(out success, out failure)) {
                    return false;
                }
                else if (!this.Advance(TokenType.CloseParen, out failure)) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (this.CurrentToken.Type == TokenType.OpenBracket) {
                if (!this.Advance(TokenType.OpenBracket, out failure)) {
                    success = null;
                    return false;
                }
                else if (!this.AddExpression(out success, out failure)) {
                    return false;
                }
                else if (!this.Advance(TokenType.CloseBracket, out failure)) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (this.CurrentToken.Type == TokenType.Plus || this.CurrentToken.Type == TokenType.Minus) {
                if (!this.Advance(this.CurrentToken.Type, out failure)) {
                    success = null;
                    return false;
                }
                else if (!this.AddExpression(out success, out failure)) {
                    return false;
                }
                else {
                    return Evaluation.ApplyUrnaryOperator(
                        this.AllOperators,
                        this.CurrentToken.Type == TokenType.Plus ? UrnaryOperatorType.Positation : UrnaryOperatorType.Negation,
                        success,
                        out success,
                        out failure
                    );
                }
            }
            else {
                success = null;
                failure = ParseException.UnexpectedCharactersError(this.CurrentToken.StringValue);
                return false;
            }
        }
    }
}
