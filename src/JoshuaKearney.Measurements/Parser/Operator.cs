using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser.Syntax;

namespace JoshuaKearney.Measurements.Parser {
    public abstract class Operator {
        protected internal Operator() { }

        public static Operator CreateMultiplication<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Multiplication, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static Operator CreateDivision<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Division, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static Operator CreateExponation<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Exponation, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static Operator CreateAddition<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Addition, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static Operator CreateSubtraction<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Subtraction, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static Operator CreateUrnaryNegative<TIn, TResult>(Func<TIn, TResult> eval)
            where TIn : Measurement<TIn>
            where TResult : Measurement<TResult> {

            return new UrnaryOperator(typeof(TIn), UrnaryOperatorType.Negation, (x) => eval(x as TIn));
        }

        public static Operator CreateUrnaryPositive<TIn, TResult>(Func<TIn, TResult> eval)
           where TIn : Measurement<TIn>
           where TResult : Measurement<TResult> {

            return new UrnaryOperator(typeof(TIn), UrnaryOperatorType.Positation, (x) => eval(x as TIn));
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
    }

}
