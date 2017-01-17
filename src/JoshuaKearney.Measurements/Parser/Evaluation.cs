using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    internal static class Evaluation {
        public static bool ApplyBinaryOperator(IEnumerable<Operator> ops, BinaryOperatorType opType, IMeasurement first, IMeasurement second, out IMeasurement success, out ParseException failure) {
            Validate.NonNull(ops, nameof(ops));
            Validate.NonNull(opType, nameof(opType));
            Validate.NonNull(first, nameof(first));
            Validate.NonNull(second, nameof(second));

            bool worked = ApplyBinaryOperators(
                FindBinaryOperators(
                    ops,
                    opType,
                    first.GetType(),
                    second.GetType()
                ),
                first,
                second,
                out success
            );

            if (!worked) {
                failure = ParseException.BinaryOperatorEvaluationFailed(opType.GetSymbolString(), first.ToString(), second.ToString());
                return false;
            }
            else {
                failure = null;
                return true;
            }
        }

        public static bool ApplyUrnaryOperator(IEnumerable<Operator> ops, UrnaryOperatorType opType, IMeasurement first,  out IMeasurement success, out ParseException failure) {
            Validate.NonNull(ops, nameof(ops));
            Validate.NonNull(opType, nameof(opType));
            Validate.NonNull(first, nameof(first));

            bool worked = ApplyUrnaryOperators(
                FindUrnaryOperators(
                    ops,
                    opType,
                    first.GetType()
                ),
                first,
                out success
            );

            if (!worked) {
                failure = ParseException.UrnaryOperatorEvaluationFailed(opType.GetSymbolString(), first.ToString());
                return false;
            }
            else {
                failure = null;
                return true;
            }
        }

        private static bool ApplyBinaryOperators(IEnumerable<BinaryOperator> ops, IMeasurement first, IMeasurement second, out IMeasurement result) {
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

        private static bool ApplyUrnaryOperators(IEnumerable<UrnaryOperator> ops, IMeasurement first, out IMeasurement success) {
            Validate.NonNull(ops, nameof(ops));
            Validate.NonNull(first, nameof(first));

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
                return true;
            }

            return false;
        }

        private static IEnumerable<BinaryOperator> FindBinaryOperators(IEnumerable<Operator> operators, BinaryOperatorType type, Type t1, Type t2) {
            Validate.NonNull(operators, nameof(operators));
            Validate.NonNull(type, nameof(type));
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            var ops = operators
                .Where(x => x is BinaryOperator)
                .Select(x => (BinaryOperator)x)
                .Where(x => x.Type == type && x.InputType1 == t1 && x.InputType2 == t2);

            return ops;
        }

        private static IEnumerable<UrnaryOperator> FindUrnaryOperators(IEnumerable<Operator> operators, UrnaryOperatorType type, Type t1) {
            Validate.NonNull(operators, nameof(operators));
            Validate.NonNull(type, nameof(type));
            Validate.NonNull(t1, nameof(t1));

            var ops = operators
                .Where(x => x is UrnaryOperator)
                .Select(x => (UrnaryOperator)x)
                .Where(x => x.Type == type && x.InputType == t1);

            return ops;
        }
    }
}
