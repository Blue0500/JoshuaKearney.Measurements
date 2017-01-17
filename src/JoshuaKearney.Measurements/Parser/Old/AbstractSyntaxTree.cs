//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using JoshuaKearney.Measurements.Parser.Lexing;

//namespace JoshuaKearney.Measurements.Parser.Syntax {
//    internal abstract class AbstractSyntaxTree {
//        public Token Token { get; }

//        public AbstractSyntaxTree(Token tok) {
//            this.Token = tok;
//        }
//    }

//    internal class NumberLeaf : AbstractSyntaxTree {
//        public double Value { get; }

//        public NumberLeaf(NumberToken tok) : base(tok) {
//            this.Value = tok.Value;
//        }
//    }

//    internal class IdLeaf : AbstractSyntaxTree {
//        public string Value { get; }

//        public IdLeaf(IdToken tok) : base(tok) {
//            this.Value = tok.Value;
//        }
//    }

//    

//    

//    internal class BinaryOperatorTree : AbstractSyntaxTree {
//        public AbstractSyntaxTree LeftOperand { get; }

//        public AbstractSyntaxTree RightOperand { get; }

//        public BinaryOperatorType Type { get; }

//        public BinaryOperatorTree(AbstractSyntaxTree left, AbstractSyntaxTree right, BinaryOperatorType type, Token tok) : base(tok) {
//            this.LeftOperand = left;
//            this.RightOperand = right;
//            this.Type = type;
//        }
//    }

//    

//    internal class UrnaryOperatorTree : AbstractSyntaxTree {
//        public AbstractSyntaxTree Operand { get; }

//        public UrnaryOperatorType Type { get; }

//        public UrnaryOperatorTree(AbstractSyntaxTree operand, UrnaryOperatorType type, Token tok) : base(tok) {
//            this.Operand = operand;
//            this.Type = type;
//        }
//    }
//}
