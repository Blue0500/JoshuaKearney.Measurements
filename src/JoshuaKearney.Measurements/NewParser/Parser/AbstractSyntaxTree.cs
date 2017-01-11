using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.NewParser.Parser {
    public class AbstractSyntaxTree { }

    public class NumberLeaf : AbstractSyntaxTree {
        public double Value { get; }

        public NumberLeaf(double value) {
            this.Value = value;
        }
    }

    public class IdLeaf : AbstractSyntaxTree {
        public string Value { get; }

        public IdLeaf(string value) {
            this.Value = value;
        }
    }

    public enum BinaryOperatorType {
        Multiplication, Division, Addition, Subtraction, Exponentation
    }

    public class BinaryOperatorTree : AbstractSyntaxTree {
        public AbstractSyntaxTree LeftOperand { get; }

        public AbstractSyntaxTree RightOperand { get; }

        public BinaryOperatorType Type { get; }

        public BinaryOperatorTree(AbstractSyntaxTree left, AbstractSyntaxTree right, BinaryOperatorType type) {
            this.LeftOperand = left;
            this.RightOperand = right;
            this.Type = type;
        }
    }

    public enum UrnaryOperatorType {
        Negation, Positation    
    }

    public class UrnaryOperatorTree {
        public AbstractSyntaxTree Operand { get; }

        public UrnaryOperatorType Type { get; }

        public UrnaryOperatorTree(AbstractSyntaxTree operand, UrnaryOperatorType type) {
            this.Operand = operand;
            this.Type = type;
        }
    }
}
