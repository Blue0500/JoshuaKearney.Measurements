using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser.Syntax;

namespace JoshuaKearney.Measurements.Parser.Evaluating {
    class Evaluation {
        private bool ApplyBinaryOperators(IEnumerable<BinaryOperator> ops, IMeasurement first, IMeasurement second, out IMeasurement result, out ParseException failure) {
            result = null;

            foreach (var op in ops) {
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

        private bool ApplyUrnaryOperators(
            IEnumerable<UrnaryOperator> ops, 
            IMeasurement first, 
            out IMeasurement success, 
            out ParseException failure) {

            success = null;

            foreach (var op in ops) {
                if (success != null) {
                    break;
                }
                else {
                    success = op.Evaluate(first);
                }
            }

            if (success != null) {
                failure = null;
                return true;
            }

            failure = ParseException.UrnaryOperatorEvaluationFailed()
            return false;
        }

        private IEnumerable<BinaryOperator> FindBinaryOperators(IEnumerable<Operator> allOps, BinaryOperatorType type, Type t1, Type t2) {
            var ops = allOps
                .Where(x => x is BinaryOperator)
                .Select(x => (BinaryOperator)x)
                .Where(x => x.Type == type && x.InputType1 == t1 && x.InputType2 == t2);

            return ops;
        }

        private IEnumerable<UrnaryOperator> FindUrnaryOperators(IEnumerable<Operator> operators, UrnaryOperatorType type, Type t1) {
            var ops = operators
                .Where(x => x is UrnaryOperator)
                .Select(x => (UrnaryOperator)x)
                .Where(x => x.Type == type && x.InputType == t1);

            return ops;
        }
    }
}
