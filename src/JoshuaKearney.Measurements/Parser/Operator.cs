using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    public delegate TResult BinaryOperatorCallback<TIn1, TIn2, TResult>(TIn1 measurement1, TIn2 measurement2) where TIn1 : IMeasurement<TIn1> where TIn2 : IMeasurement<TIn2> where TResult : IMeasurement<TResult>;
    public delegate TResult UrnaryOperatorCallback<TIn1, TResult>(TIn1 measurement1) where TIn1 : IMeasurement<TIn1> where TResult : IMeasurement<TResult>;

    public abstract class Operator {
        protected internal Operator() { }

        private static object InvokeBinaryOperator<TIn1, TIn2, TResult>(object x, object y, BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1> where TIn2 : IMeasurement<TIn2> where TResult : IMeasurement<TResult> {

            if (x is TIn1 in1 && y is TIn2 in2) {
                return eval(in1, in2);
            }

            return null;
        }

        private static object InvokeUrnaryOperator<TIn1, TResult>(object x, UrnaryOperatorCallback<TIn1, TResult> eval)
           where TIn1 : IMeasurement<TIn1> where TResult : IMeasurement<TResult> {

            if (x is TIn1 in1) {
                return eval(in1);
            }

            return null;
        }

        public static Operator CreateMultiplication<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(
                typeof(TIn1), 
                typeof(TIn2), 
                BinaryOperatorType.Multiplication, 
                (x, y) => InvokeBinaryOperator(x, y, eval)
            );
        }

        public static Operator CreateDivision<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(
                typeof(TIn1), 
                typeof(TIn2), 
                BinaryOperatorType.Division, 
                (x, y) => InvokeBinaryOperator(x, y, eval)
            );
        }

        public static Operator CreateExponation<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(
                typeof(TIn1), 
                typeof(TIn2), 
                BinaryOperatorType.Exponation, 
                (x, y) => InvokeBinaryOperator(x, y, eval)
            );
        }

        public static Operator CreateAddition<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(
                typeof(TIn1), 
                typeof(TIn2), 
                BinaryOperatorType.Addition, 
                (x, y) => InvokeBinaryOperator(x, y, eval)
            );
        }

        public static Operator CreateSubtraction<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(
                typeof(TIn1), 
                typeof(TIn2), 
                BinaryOperatorType.Subtraction, 
                (x, y) => InvokeBinaryOperator(x, y, eval)
            );
        }

        public static Operator CreateUrnaryNegative<TIn, TResult>(UrnaryOperatorCallback<TIn, TResult> eval)
            where TIn : IMeasurement<TIn>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new UrnaryOperator(
                typeof(TIn), 
                UrnaryOperatorType.Negation, 
                (x) => InvokeUrnaryOperator(x, eval)
            );
        }

        public static Operator CreateUrnaryPositive<TIn, TResult>(UrnaryOperatorCallback<TIn, TResult> eval)
            where TIn : IMeasurement<TIn>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new UrnaryOperator(
                typeof(TIn), 
                UrnaryOperatorType.Positation, 
                (x) => InvokeUrnaryOperator(x, eval)
            );
        }
    }

    internal class BinaryOperator : Operator {
        public BinaryOperatorType Type { get; }
        public Func<object, object, object> Evaluate { get; }
        public Type InputType1 { get; }
        public Type InputType2 { get; }

        public BinaryOperator(Type input1, Type input2, BinaryOperatorType type, Func<object, object, object> eval) {
            this.Evaluate = eval;
            this.Type = type;
            this.InputType1 = input1;
            this.InputType2 = input2;
        }
    }

    internal class UrnaryOperator : Operator {
        public UrnaryOperatorType Type { get; }
        public Func<object, object> Evaluate { get; }
        public Type InputType { get; }

        public UrnaryOperator(Type input, UrnaryOperatorType type, Func<object, object> eval) {
            this.Evaluate = eval;
            this.Type = type;
            this.InputType = input;
        }
    }

}
