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

        public Union<object, Exception> TryInterpret(AbstractSyntaxTree tree, IEnumerable<ParsingOperator> ops, IReadOnlyDictionary<string, object> units) {
            lock (lockObj) {
                this.Operators = ops;
                this.Units = units;

                return this.Visit(tree);
            }
        }

        public Union<object, Exception> Visit(AbstractSyntaxTree tree) {
            if (tree is BinaryOperatorTree) {
                return this.VisitBinaryOperator(tree as BinaryOperatorTree);
            }
            else if (tree is IdLeaf) {
                return this.VisitIdLeaf(tree as IdLeaf);
            }
            else if (tree is NumberLeaf) {
                return this.VisitNumberLeaf(tree as NumberLeaf);
            }
            else {
                return new Exception();
            }
        }

        public Union<object, Exception> VisitBinaryOperator(BinaryOperatorTree tree) {
            return this.Visit(tree.LeftOperand).Select(
                first => this.Visit(tree.RightOperand).Select(
                    second => {
                        IEnumerable<ParsingOperator> ops;
                        if (tree.Type == BinaryOperatorType.Multiplication) {
                            if (this.FindOp(BinaryOperatorType.Multiplication, first.GetType(), second.GetType(), out ops)) {
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
                        else if (tree.Type == BinaryOperatorType.Exponation || tree.Type == BinaryOperatorType.Division) {
                            if (this.FindOp(tree.Type, first.GetType(), second.GetType(), out ops)) {
                                object measurement;
                                if (this.ApplyOperators(ops, first, second, out measurement)) {
                                    return measurement;
                                }
                            }
                        }

                        return new Exception($"Evaluation of operator '{tree.Token}' failed on operands '{first}' and '{second}'");
                    },
                    exception => exception    
                ),
                exception => exception    
            );            
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