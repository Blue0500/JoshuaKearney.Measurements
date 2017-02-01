using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    public delegate bool BinaryOperatorTryCallback<TIn1, TIn2, TResult>(TIn1 measurement1, TIn2 measurement2, out TResult result) where TIn1 : IMeasurement<TIn1> where TIn2 : IMeasurement<TIn2> where TResult : IMeasurement<TResult>;
    public delegate bool UrnaryOperatorTryCallback<TIn1, TResult>(TIn1 measurement1, out TResult result) where TIn1 : IMeasurement<TIn1> where TResult : IMeasurement<TResult>;
    public delegate TResult BinaryOperatorCallback<TIn1, TIn2, TResult>(TIn1 measurement1, TIn2 measurement2) where TIn1 : IMeasurement<TIn1> where TIn2 : IMeasurement<TIn2> where TResult : IMeasurement<TResult>;
    public delegate TResult UrnaryOperatorCallback<TIn1, TResult>(TIn1 measurement1) where TIn1 : IMeasurement<TIn1> where TResult : IMeasurement<TResult>;

    public abstract class Operator {
        protected internal Operator() { }

        private static IMeasurement InvokeBinaryOperator<TIn1, TIn2, TResult>(IMeasurement x, IMeasurement y, BinaryOperatorTryCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1> where TIn2 : IMeasurement<TIn2> where TResult : IMeasurement<TResult> {

            if (x is TIn1 && y is TIn2) {
                TResult res;
                if (eval((TIn1)x, (TIn2)y, out res)) {
                    return res;
                }
            }

            return null;
        }

        private static IMeasurement InvokeUrnaryOperator<TIn1, TResult>(IMeasurement x, UrnaryOperatorTryCallback<TIn1, TResult> eval)
           where TIn1 : IMeasurement<TIn1> where TResult : IMeasurement<TResult> {

            if (x is TIn1) {
                TResult res;
                if (eval((TIn1)x, out res)) {
                    return res;
                }
            }

            return null;
        }

        public static Operator CreateMultiplication<TIn1, TIn2, TResult>(BinaryOperatorTryCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Multiplication, (x, y) => InvokeBinaryOperator(x, y, eval));
        }

        public static Operator CreateDivision<TIn1, TIn2, TResult>(BinaryOperatorTryCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Division, (x, y) => InvokeBinaryOperator(x, y, eval));
        }

        public static Operator CreateExponation<TIn1, TIn2, TResult>(BinaryOperatorTryCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Exponation, (x, y) => InvokeBinaryOperator(x, y, eval));
        }

        public static Operator CreateAddition<TIn1, TIn2, TResult>(BinaryOperatorTryCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Addition, (x, y) => InvokeBinaryOperator(x, y, eval));
        }

        public static Operator CreateSubtraction<TIn1, TIn2, TResult>(BinaryOperatorTryCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Subtraction, (x, y) => InvokeBinaryOperator(x, y, eval));
        }

        public static Operator CreateUrnaryNegative<TIn, TResult>(UrnaryOperatorTryCallback<TIn, TResult> eval)
            where TIn : IMeasurement<TIn>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new UrnaryOperator(typeof(TIn), UrnaryOperatorType.Negation, (x) => InvokeUrnaryOperator(x, eval));
        }

        public static Operator CreateUrnaryPositive<TIn, TResult>(UrnaryOperatorTryCallback<TIn, TResult> eval)
            where TIn : IMeasurement<TIn>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return new UrnaryOperator(typeof(TIn), UrnaryOperatorType.Positation, (x) => InvokeUrnaryOperator(x, eval));
        }


        public static Operator CreateMultiplication<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateMultiplication((TIn1 x, TIn2 y, out TResult result) => { result = eval(x, y); return true; });
        }

        public static Operator CreateDivision<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateDivision((TIn1 x, TIn2 y, out TResult result) => { result = eval(x, y); return true; });
        }

        public static Operator CreateExponation<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateExponation((TIn1 x, TIn2 y, out TResult result) => { result = eval(x, y); return true; });
        }

        public static Operator CreateAddition<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateAddition((TIn1 x, TIn2 y, out TResult result) => { result = eval(x, y); return true; });
        }

        public static Operator CreateSubtraction<TIn1, TIn2, TResult>(BinaryOperatorCallback<TIn1, TIn2, TResult> eval)
            where TIn1 : IMeasurement<TIn1>
            where TIn2 : IMeasurement<TIn2>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateSubtraction((TIn1 x, TIn2 y, out TResult result) => { result = eval(x, y); return true; });
        }

        public static Operator CreateUrnaryNegative<TIn, TResult>(UrnaryOperatorCallback<TIn, TResult> eval)
            where TIn : IMeasurement<TIn>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateUrnaryNegative((TIn x, out TResult result) => { result = eval(x); return true; });
        }

        public static Operator CreateUrnaryPositive<TIn, TResult>(UrnaryOperatorCallback<TIn, TResult> eval)
            where TIn : IMeasurement<TIn>
            where TResult : IMeasurement<TResult> {

            Validate.NonNull(eval, nameof(eval));

            return CreateUrnaryPositive((TIn x, out TResult result) => { result = eval(x); return true; });
        }
    }

    internal class BinaryOperator : Operator {
        public BinaryOperatorType Type { get; }
        public Func<IMeasurement, IMeasurement, IMeasurement> Evaluate { get; }
        public Type InputType1 { get; }
        public Type InputType2 { get; }

        public BinaryOperator(Type input1, Type input2, BinaryOperatorType type, Func<IMeasurement, IMeasurement, IMeasurement> eval) {
            this.Evaluate = eval;
            this.Type = type;
            this.InputType1 = input1;
            this.InputType2 = input2;
        }

        public override string ToString() {
            char c = '&';
            switch (this.Type) {
                case BinaryOperatorType.Multiplication: c = '*'; break;
                case BinaryOperatorType.Division:       c = '/'; break;
                case BinaryOperatorType.Addition:       c = '+'; break;
                case BinaryOperatorType.Subtraction:    c = '-'; break;
                case BinaryOperatorType.Exponation:     c = '^'; break;
            }

            return $"Operator[{InputType1.Name} {c.ToString()} {InputType2.Name} -> IMeasurement]";
        }
    }

    internal class UrnaryOperator : Operator {
        public UrnaryOperatorType Type { get; }
        public Func<IMeasurement, IMeasurement> Evaluate { get; }
        public Type InputType { get; }

        public UrnaryOperator(Type input, UrnaryOperatorType type, Func<IMeasurement, IMeasurement> eval) {
            this.Evaluate = eval;
            this.Type = type;
            this.InputType = input;
        }

        public override string ToString() {
            char c = '&';
            switch (this.Type) {
                case UrnaryOperatorType.Negation: c = '-'; break;
                case UrnaryOperatorType.Positation: c = '/'; break;
            }

            return $"Operator[{InputType.Name} {c.ToString()} -> IMeasurement]";
        }
    }
}