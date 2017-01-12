using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.NewParser.Lexer;

namespace JoshuaKearney.Measurements.NewParser.Syntax {
    public abstract class AbstractSyntaxTree {
        public Token Token { get; }

        public AbstractSyntaxTree(Token tok) {
            this.Token = tok;
        }
    }

    public class NumberLeaf : AbstractSyntaxTree {
        public double Value { get; }

        public NumberLeaf(NumberToken tok) : base(tok) {
            this.Value = tok.Value;
        }
    }

    public class IdLeaf : AbstractSyntaxTree {
        public string Value { get; }

        public IdLeaf(IdToken tok) : base(tok) {
            this.Value = tok.Value;
        }
    }

    public enum BinaryOperatorType {
        Multiplication, Division, Addition, Subtraction, Exponation
    }

    public class BinaryOperatorTree : AbstractSyntaxTree {
        public AbstractSyntaxTree LeftOperand { get; }

        public AbstractSyntaxTree RightOperand { get; }

        public BinaryOperatorType Type { get; }

        public BinaryOperatorTree(AbstractSyntaxTree left, AbstractSyntaxTree right, BinaryOperatorType type, Token tok) : base(tok) {
            this.LeftOperand = left;
            this.RightOperand = right;
            this.Type = type;
        }
    }

    public enum UrnaryOperatorType {
        Negation, Positation    
    }

    public class UrnaryOperatorTree : AbstractSyntaxTree {
        public AbstractSyntaxTree Operand { get; }

        public UrnaryOperatorType Type { get; }

        public UrnaryOperatorTree(AbstractSyntaxTree operand, UrnaryOperatorType type, Token tok) : base(tok) {
            this.Operand = operand;
            this.Type = type;
        }
    }
}
