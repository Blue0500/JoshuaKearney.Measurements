using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.NewParser.Syntax;

namespace JoshuaKearney.Measurements.NewParser {
    public class ParsingOperator {
        internal BinaryOperatorType Type { get; }
        internal Func<object, object, object> Evaluate { get; }
        internal Type InputType1 { get; }
        internal Type InputType2 { get; }

        public ParsingOperator(Type input1, Type input2, BinaryOperatorType type, Func<object, object, object> eval) {
            this.Evaluate = eval;
            this.Type = type;
            this.InputType1 = input1;
            this.InputType2 = input2;
        }

        public static ParsingOperator CreateMultiplication<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new ParsingOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Multiplication, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static ParsingOperator CreateDivision<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new ParsingOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Division, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static ParsingOperator CreateExponation<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new ParsingOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Exponation, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static ParsingOperator CreateAddition<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new ParsingOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Addition, (x, y) => eval(x as TIn1, y as TIn2));
        }

        public static ParsingOperator CreateSubtraction<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> eval)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new ParsingOperator(typeof(TIn1), typeof(TIn2), BinaryOperatorType.Subtraction, (x, y) => eval(x as TIn1, y as TIn2));
        }
    }
}
