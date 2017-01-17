//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using JoshuaKearney.Measurements.Parser.Lexing;
//using JoshuaKearney.Measurements.Parser.Syntax;

//namespace JoshuaKearney.Measurements.Parser.Syntax {
//    internal class TokenParser {
//        private IEnumerable<Token> Tokens;
//        private int pos = 0;
//        private object lockObj = new object();

//        private Token CurrentToken => Tokens.ElementAtOrDefault(pos) ?? Token.EOF;

//        private IEnumerable<TokenType> AtomStartTypes { get; } = new[] { TokenType.OpenParen, TokenType.Number, TokenType.Id };

//        private bool Advance(TokenType type, out ParseException failure) {
//            if (CurrentToken.Type != type) {
//                failure = ParseException.UnexpectedCharactersError(this.CurrentToken.ToString());
//                return false;
//            }
//            else {
//                pos++;
//                failure = null;
//                return true;
//            }
//        }

//        public bool TryParse(IEnumerable<Token> tokens, out AbstractSyntaxTree success, out ParseException failure) {
//            lock (lockObj) {
//                this.pos = 0;
//                this.Tokens = tokens;

//                return this.AddExpression(out success, out failure);
//            }
//        }

//        private bool AddExpression(out AbstractSyntaxTree success, out ParseException failure) {
//            if (!this.MultExpression(out success, out failure)) {
//                return false;
//            }
//            else {
//                while (this.CurrentToken.Type == TokenType.Plus || this.CurrentToken.Type == TokenType.Minus) {
//                    Token prev = this.CurrentToken;
//                    AbstractSyntaxTree next;

//                    if (!this.Advance(this.CurrentToken.Type, out failure)) {
//                        success = null;
//                        return false;
//                    }
//                    else if (!this.MultExpression(out next, out failure)) {
//                        success = null;
//                        return false;
//                    }
//                    else if (prev.Type == TokenType.Plus) {
//                        success = new BinaryOperatorTree(success, next, BinaryOperatorType.Addition, prev);
//                    }
//                    else {
//                        success = new BinaryOperatorTree(success, next, BinaryOperatorType.Subtraction, prev);
//                    }                  
//                }

//                return true;
//            }
//        }

//        private bool MultExpression(out AbstractSyntaxTree success, out ParseException failure) {
//            if (!this.PowExpression(out success, out failure)) {
//                return false;
//            }
//            else {
//                while (this.AtomStartTypes.Any(x => this.CurrentToken.Type == x) || this.CurrentToken.Type == TokenType.ForwardSlash || this.CurrentToken.Type == TokenType.Asterisk) {
//                    Token prev = this.CurrentToken;
//                    AbstractSyntaxTree next;

//                    if (prev.Type == TokenType.ForwardSlash || prev.Type == TokenType.Asterisk) {
//                        if (!this.Advance(this.CurrentToken.Type, out failure)) {
//                            success = null;
//                            return false;
//                        }
//                    }

//                    if (!this.PowExpression(out next, out failure)) {
//                        success = null;
//                        return false;
//                    }
//                    else {
//                        if (prev.Type == TokenType.ForwardSlash) {
//                            success = new BinaryOperatorTree(success, next, BinaryOperatorType.Division, prev);
//                        }
//                        else {
//                            success = new BinaryOperatorTree(success, next, BinaryOperatorType.Multiplication, prev);
//                        }
//                    }
//                }

//                return true;
//            }
//        }

//        private bool PowExpression(out AbstractSyntaxTree success, out ParseException failure) {
//            if (!this.Atom(out success, out failure)) {
//                return false;
//            }
//            else {
//                while (this.CurrentToken.Type == TokenType.Caret) {
//                    Token prev = this.CurrentToken;
//                    AbstractSyntaxTree next;

//                    if (!this.Advance(TokenType.Caret, out failure)) {
//                        success = null;
//                        return false;
//                    }
//                    else if (!this.Atom(out next, out failure)) {
//                        success = null;
//                        return false;
//                    }
//                    else {
//                        success = new BinaryOperatorTree(success, next, BinaryOperatorType.Exponation, prev);
//                    }
//                }

//                return true;
//            }
//        }

//        private bool Atom(out AbstractSyntaxTree success, out ParseException failure) {
//            if (this.CurrentToken.Type == TokenType.Number) {
//                NumberToken tok = this.CurrentToken as NumberToken;

//                if (!this.Advance(TokenType.Number, out failure)) {
//                    success = null;
//                    return false;
//                }
//                else {
//                    success = new NumberLeaf(tok);
//                    failure = null;
//                    return true;
//                }
//            }
//            else if (this.CurrentToken.Type == TokenType.Id) {
//                IdToken tok = this.CurrentToken as IdToken;

//                if (!this.Advance(TokenType.Id, out failure)) {
//                    success = null;
//                    return false;
//                }
//                else {
//                    success = new IdLeaf(tok);
//                    failure = null;
//                    return true;
//                }
//            }
//            else if (this.CurrentToken.Type == TokenType.OpenParen) {
//                if (!this.Advance(TokenType.OpenParen, out failure)) {
//                    success = null;
//                    return false;
//                }
//                else if (!this.AddExpression(out success, out failure)) {
//                    return false;
//                }
//                else if (!this.Advance(TokenType.CloseParen, out failure)) {
//                    return false;
//                }
//                else {
//                    return true;
//                }
//            }
//            else if (this.CurrentToken.Type == TokenType.Plus || this.CurrentToken.Type == TokenType.Minus) {
//                if (!this.Advance(this.CurrentToken.Type, out failure)) {
//                    success = null;
//                    return false;
//                }
//                else if (!this.AddExpression(out success, out failure)) {
//                    return false;
//                }
//                else {
//                    success = new UrnaryOperatorTree(
//                        success, 
//                        this.CurrentToken.Type == TokenType.Plus ? UrnaryOperatorType.Positation : UrnaryOperatorType.Negation, 
//                        Token.Plus
//                    );

//                    return true;
//                }
//            }
//            else {
//                success = null;
//                failure = ParseException.UnexpectedCharactersError(this.CurrentToken.StringValue);
//                return false;
//            }
//        }
//    }
//}
