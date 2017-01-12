using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.NewParser.Syntax;


namespace JoshuaKearney.Measurements.NewParser.Interpreter {
    internal class EvaulateVisitor {
        private IEnumerable<ParsingOperator> Operators;
        private IReadOnlyDictionary<string, object> Units;
        private object lockObj = new object();

        public bool TryInterpret(
            AbstractSyntaxTree tree, 
            IEnumerable<ParsingOperator> ops, 
            IReadOnlyDictionary<string, object> units,
            out object success,
            out Exception failure) {

            lock (lockObj) {
                this.Operators = ops;
                this.Units = units;

                return this.Visit(tree, out success, out failure);
            }
        }

        public bool Visit(AbstractSyntaxTree tree, out object result, out Exception result2) {
            if (tree is BinaryOperatorTree) {
                return this.VisitBinaryOperator(tree as BinaryOperatorTree, out result, out result2);
            }
            else if (tree is IdLeaf) {
                return this.VisitIdLeaf(tree as IdLeaf, out result, out result2);
            }
            else if (tree is NumberLeaf) {
                return this.VisitNumberLeaf(tree as NumberLeaf, out result, out result2);
            }
            else {
                result = null;
                result2 = new Exception();
                return false;
            }
        }

        public bool VisitBinaryOperator(BinaryOperatorTree tree, out object success, out Exception failure) {
            object first, second;
            Exception exp;

            if (this.Visit(tree.LeftOperand,  out first, out exp)) {
                if (this.Visit(tree.RightOperand, out second, out exp)) {

                }
                else {
                    success = null;
                    failure = exp;
                    return false;
                }
            }
            else {
                success = null;
                failure = exp;
                return false;
            }

           /// return this.Visit(tree.LeftOperand).Select(
              //  first => this.Visit(tree.RightOperand).Select(
                //    second => {
                        IEnumerable<ParsingOperator> ops;
                        if (tree.Type == BinaryOperatorType.Multiplication || tree.Type == BinaryOperatorType.Addition) {
                            if (this.FindOp(tree.Type, first.GetType(), second.GetType(), out ops)) {
                                object measurement;
                                if (this.ApplyOperators(ops, first, second, out measurement)) {
                                    return measurement;
                                }                                
                            }
                            else if (this.FindOp(BinaryOperatorType.Multiplication, second.GetType(), first.GetType(), out ops)) {
                                object measurement;
                                if (this.ApplyOperators(ops, second, first, out measurement)) {
                                    return measurement;
                                }                                
                            }
                        }
                        else {
                            if (this.FindOp(tree.Type, first.GetType(), second.GetType(), out ops)) {
                                object measurement;
                                if (this.ApplyOperators(ops, first, second, out measurement)) {
                                    return measurement;
                                }
                            }
                        }

                        return new Exception($"Evaluation of operator '{tree.Token}' failed on operands '{first}' and '{second}'");
                //    },
              //      exception => exception    
               // ),
                //exception => exception    
            //);            
        }

        public Union<object, Exception> VisitIdLeaf(IdLeaf id) {
            if (!this.Units.ContainsKey(id.Value)) {
                return new Exception($"The unit '{id.Value}' is undefined");
            }
            else {
                return this.Units[id.Value];
            }
        }

        public Union<object, Exception> VisitNumberLeaf(NumberLeaf leaf) {
            return new DoubleMeasurement(leaf.Value);
        }

        public bool ApplyOperators(IEnumerable<ParsingOperator> ops, object first, object second, out object result) {
            result = null;

            foreach(var op in ops) {
                if (result != null) {
                    break;
                }
                else {
                    result = op.Evaluate(first, second);
                }
            }

            if (result != null) {
                return true;
            }

            return false;
        }

        public bool FindOp(BinaryOperatorType type, Type t1, Type t2, out IEnumerable<ParsingOperator> operators) {
            var ops = this.Operators.Where(x => x.Type == type && x.InputType1 == t1 && x.InputType2 == t2);
            operators = ops;
            return operators.Count() > 0;            
        }
    }
}