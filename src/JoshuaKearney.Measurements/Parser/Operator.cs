using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {

    internal class Operator : Token {
        public static Operator CloseParen { get; } = new Operator(")", 100);

        public static Operator OpenParen { get; } = new Operator("(", 100);

        public static BinaryOperator Multiply { get; } = new BinaryOperator("*", 5, (x, y) => ApplyBinaryOp(typeof(IMultipliableMeasurement<,>), typeof(Term<,>), x, y));

        public static BinaryOperator Divide { get; } = new BinaryOperator("/", 5, (x, y) => ApplyBinaryOp(typeof(IDividableMeasurement<,>), typeof(Ratio<,>), x, y));

        public static UrnaryOperator Square { get; } = new UrnaryOperator("²", 5, x => ApplyUrnaryOp(typeof(ISquareableMeasurement<>), x));

        public static UrnaryOperator Cube { get; } = new UrnaryOperator("³", 5, x => ApplyUrnaryOp(typeof(ICubableMeasurement<>), x));

        public Operator(string value, int priority) : base(value) {
            this.Priority = priority;
        }

        // Higher is higher priortiy
        public int Priority { get; }

        private static MeasurementToken ApplyBinaryOp(Type tInterface, Type tBackup, MeasurementToken x, MeasurementToken y) {
            Type tFirst = x.MeasurementValue.GetType();
            Type tSecond = y.MeasurementValue.GetType();

            var tGenericInterface = tFirst
                .GetTypeInfo()
                .ImplementedInterfaces
                .Where(z =>
                    z.IsConstructedGenericType &&
                    z.GetGenericTypeDefinition() == tInterface &&
                    z.GenericTypeArguments.Count() == 2 &&
                    z.GenericTypeArguments[0] == tSecond)
                .FirstOrDefault();

            if (tGenericInterface != null) {
                var mathMethod = tGenericInterface
                    .GetTypeInfo()
                    .DeclaredMethods
                    .First();

                return new MeasurementToken(mathMethod.Invoke(x.MeasurementValue, new[] { y.MeasurementValue }));
            }
            else {
                return null;
            }
        }

        private static MeasurementToken ApplyUrnaryOp(Type tInterface, MeasurementToken x) {
            Type tFirst = x.MeasurementValue.GetType();

            var tGenericInterface = tFirst
                .GetTypeInfo()
                .ImplementedInterfaces
                .Where(z =>
                    z.IsConstructedGenericType &&
                    z.IsConstructedGenericType &&
                    z.GetGenericTypeDefinition() == tInterface &&
                    z.GenericTypeArguments.Count() == 1)
                .FirstOrDefault();

            if (tGenericInterface != null) {
                var mathMethod = tGenericInterface
                    .GetTypeInfo()
                    .DeclaredMethods
                    .First();

                return new MeasurementToken(mathMethod.Invoke(x.MeasurementValue, new object[] { }));
            }
            else {
                return null;
            }
        }
    }

    internal class UrnaryOperator : Operator {

        public UrnaryOperator(string value, int priority, Func<MeasurementToken, MeasurementToken> eval) : base(value, priority) {
            this.Evaluate = eval;
        }

        public Func<MeasurementToken, MeasurementToken> Evaluate { get; }
    }

    internal class BinaryOperator : Operator {

        public BinaryOperator(string value, int priority, Func<MeasurementToken, MeasurementToken, MeasurementToken> eval) : base(value, priority) {
            this.Evaluate = eval;
        }

        public Func<MeasurementToken, MeasurementToken, MeasurementToken> Evaluate { get; }
    }
}