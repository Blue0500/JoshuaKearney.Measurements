using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser.Syntax;


namespace JoshuaKearney.Measurements.Parser.Evaluating {
    internal class EvaluationVisitor {
        private IEnumerable<Operator> Operators;
        private IReadOnlyDictionary<string, IMeasurement> Units;
        private object lockObj = new object();

        public bool TryInterpret(
            AbstractSyntaxTree tree, 
            IEnumerable<Operator> ops, 
            IReadOnlyDictionary<string, IMeasurement> units,
            out IMeasurement success,
            out ParseException failure) {

            lock (lockObj) {
                this.Operators = ops;
                this.Units = units;

                return this.Visit(tree, out success, out failure);
            }
        }

        private bool Visit(AbstractSyntaxTree tree, out IMeasurement success, out ParseException failure) {
            if (tree is BinaryOperatorTree) {
                return this.VisitBinaryOperator(tree as BinaryOperatorTree, out success, out failure);
            }
            else if (tree is IdLeaf) {
                return this.VisitIdLeaf(tree as IdLeaf, out success, out failure);
            }
            else if (tree is NumberLeaf) {
                return this.VisitNumberLeaf(tree as NumberLeaf, out success, out failure);
            }
            else {
                success = null;
                failure = ParseException.UnrecognizedSyntaxConstructDiscovered(tree.GetType());
                return false;
            }
        }

        private bool VisitUrnaryOperator(UrnaryOperatorTree tree, out IMeasurement success, out ParseException failure) {
            if (this.Visit(tree.Operand, out success, out failure)) {
                IEnumerable<UrnaryOperator> ops;

                if (this.FindUrnaryOperators(tree.Type, success.GetType(), out ops)) {
                    if (this.ApplyUrnaryOperators(ops, success, out success)) {
                        return true;
                    }
                }

                failure = ParseException.UrnaryOperatorEvaluationFailed(tree.Operand.Token.StringValue, success.ToString());
                return false;
            }

            return false;
        }

        private bool VisitBinaryOperator(BinaryOperatorTree tree, out IMeasurement success, out ParseException failure) {
            IMeasurement first, second;

            if (this.Visit(tree.LeftOperand,  out first, out failure)) {
                if (this.Visit(tree.RightOperand, out second, out failure)) {
                    IEnumerable<BinaryOperator> ops;

                    if (tree.Type == BinaryOperatorType.Multiplication || tree.Type == BinaryOperatorType.Addition) {
                        if (this.FindBinaryOperators(tree.Type, first.GetType(), second.GetType(), out ops)) {
                            if (this.ApplyBinaryOperators(ops, first, second, out success)) {
                                return true;
                            }
                        }
                        else if (this.FindBinaryOperators(BinaryOperatorType.Multiplication, second.GetType(), first.GetType(), out ops)) {
                            if (this.ApplyBinaryOperators(ops, second, first, out success)) {
                                return true;
                            }
                        }
                    }
                    else {
                        if (this.FindBinaryOperators(tree.Type, first.GetType(), second.GetType(), out ops)) {
                            if (this.ApplyBinaryOperators(ops, first, second, out success)) {
                                failure = null;
                                return true;
                            }
                        }
                    }

                    success = null;
                    failure = ParseException.BinaryOperatorEvaluationFailed(tree.Token.StringValue, first.ToString(), second.ToString());
                    return false;
                }
            }

            success = null;
            return false;
        }

        private bool VisitIdLeaf(IdLeaf id, out IMeasurement success, out ParseException failure) {
            if (!this.Units.ContainsKey(id.Value)) {
                success = null;
                failure = ParseException.UndefinedUnitDiscovered(id.Value);
                return false;
            }
            else {
                success = this.Units[id.Value];
                failure = null;
                return true;
            }
        }

        private bool VisitNumberLeaf(NumberLeaf leaf, out IMeasurement success, out ParseException failure) {
            success = new DoubleMeasurement(leaf.Value);
            failure = null;
            return true;
        }

        private bool ApplyBinaryOperators(IEnumerable<BinaryOperator> ops, IMeasurement first, IMeasurement second, out IMeasurement result) {
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

        private bool FindBinaryOperators(BinaryOperatorType type, Type t1, Type t2, out IEnumerable<BinaryOperator> operators) {
            var ops = this.Operators
                .Where(x => x is BinaryOperator)
                .Select(x => (BinaryOperator)x)
                .Where(x => x.Type == type && x.InputType1 == t1 && x.InputType2 == t2);

            operators = ops;
            return operators.Count() > 0;            
        }

        private bool ApplyUrnaryOperators(IEnumerable<UrnaryOperator> ops, IMeasurement first, out IMeasurement result) {
            result = null;

            foreach (var op in ops) {
                if (result != null) {
                    break;
                }
                else {
                    result = op.Evaluate(first);
                }
            }

            if (result != null) {
                return true;
            }

            return false;
        }

        private bool FindUrnaryOperators(UrnaryOperatorType type, Type t1, out IEnumerable<UrnaryOperator> operators) {
            var ops = this.Operators
                .Where(x => x is UrnaryOperator)
                .Select(x => (UrnaryOperator)x)
                .Where(x => x.Type == type && x.InputType == t1);

            operators = ops;
            return operators.Count() > 0;
        }
    }
}