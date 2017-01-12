using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.NewParser.Lexer;
using JoshuaKearney.Measurements.NewParser.Syntax;

namespace JoshuaKearney.Measurements.NewParser {
    public class TokenParser {
        private IEnumerable<Token> Tokens;
        private int pos = 0;
        private object lockObj = new object();

        private Token CurrentToken => Tokens.ElementAtOrDefault(pos) ?? Token.EOF;

        private IEnumerable<TokenType> AtomStartTypes { get; } = new[] { TokenType.OpenParen, TokenType.Number, TokenType.Id };

        private void Advance(TokenType type) {
            if (CurrentToken.Type != type) {
                throw new Exception();
            }

            pos++;
        }

        public AbstractSyntaxTree Parse(IEnumerable<Token> tokens) {
            lock (lockObj) {
                this.pos = 0;
                this.Tokens = tokens;
                return this.AddExpression();
            }
        }

        public AbstractSyntaxTree AddExpression() {
            AbstractSyntaxTree first = this.MultExpression();

            while (this.CurrentToken.Type == TokenType.Plus || this.CurrentToken.Type == TokenType.Minus) {
                Token prev = this.CurrentToken;
                this.Advance(this.CurrentToken.Type);

                if (this.CurrentToken.Type == TokenType.Plus) {
                    first = new BinaryOperatorTree(first, this.MultExpression(), BinaryOperatorType.Addition, prev);
                }
                else {
                    first = new BinaryOperatorTree(first, this.MultExpression(), BinaryOperatorType.Subtraction, prev);
                }
            }

            return first;
        }

        public AbstractSyntaxTree MultExpression() {
            AbstractSyntaxTree first = this.PowExpression();

            while (this.AtomStartTypes.Any(x => this.CurrentToken.Type == x) || this.CurrentToken.Type == TokenType.ForwardSlash || this.CurrentToken.Type == TokenType.Asterisk) {
                Token prev = this.CurrentToken;

                if (prev.Type == TokenType.ForwardSlash || prev.Type == TokenType.Asterisk) {
                    this.Advance(this.CurrentToken.Type);
                }

                if (prev.Type == TokenType.ForwardSlash) {
                    first = new BinaryOperatorTree(first, this.PowExpression(), BinaryOperatorType.Division, prev);
                }
                else {
                    first = new BinaryOperatorTree(first, this.PowExpression(), BinaryOperatorType.Multiplication, prev);
                }
            }

            return first;
        }

        public AbstractSyntaxTree PowExpression() {
            AbstractSyntaxTree first = this.Atom();

            while (this.CurrentToken.Type == TokenType.Caret) {
                Token prev = this.CurrentToken;
                this.Advance(TokenType.Caret);
                first = new BinaryOperatorTree(first, this.Atom(), BinaryOperatorType.Exponation, prev);
            }

            return first;
        }

        public AbstractSyntaxTree Atom() {
            if (this.CurrentToken.Type == TokenType.Number) {
                NumberToken tok = this.CurrentToken as NumberToken;
                this.Advance(TokenType.Number);

                return new NumberLeaf(tok);
            }
            else if (this.CurrentToken.Type == TokenType.Id) {
                IdToken tok = this.CurrentToken as IdToken;
                this.Advance(TokenType.Id);

                return new IdLeaf(tok);
            }
            else if (this.CurrentToken.Type == TokenType.OpenParen) {
                this.Advance(TokenType.OpenParen);
                var ret = this.AddExpression();
                this.Advance(TokenType.CloseParen);

                return ret;
            }
            else if (this.CurrentToken.Type == TokenType.Plus) {
                this.Advance(TokenType.Plus);
                return new UrnaryOperatorTree(this.AddExpression(), UrnaryOperatorType.Positation, Token.Plus);
            }
            else if (this.CurrentToken.Type == TokenType.Minus) {
                this.Advance(TokenType.Minus);
                return new UrnaryOperatorTree(this.AddExpression(), UrnaryOperatorType.Negation, Token.Minus);
            }
            else {
                throw new Exception();
            }
        }
    }
}
