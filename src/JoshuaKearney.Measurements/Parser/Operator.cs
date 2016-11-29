using System;
using System.Linq;
using System.Reflection;

namespace JoshuaKearney.Measurements.Parser {

    internal class Operator : Token {
        public static Operator CloseParen { get; } = new Operator(")", 100);

        public static Operator OpenParen { get; } = new Operator("(", 100);

        public static BinaryOperator Multiply { get; } = new BinaryOperator("*", 5, (x, y) => {
            var result = ApplyBinaryOp(typeof(IMultipliableMeasurement<,>), x, y) ?? ApplyBinaryOp(typeof(IMultipliableMeasurement<,>), y, x);
            return result;
        });

        public static BinaryOperator Divide { get; } = new BinaryOperator("/", 5, (x, y) => ApplyBinaryOp(typeof(IDividableMeasurement<,>), x, y) ?? DivideToRatio(x, y));

        public static UrnaryOperator Square { get; } = new UrnaryOperator("²", 10, x => ApplyUrnaryOp(typeof(ISquareableMeasurement<>), x));

        public static UrnaryOperator Cube { get; } = new UrnaryOperator("³", 10, x => ApplyUrnaryOp(typeof(ICubableMeasurement<>), x));

        public Operator(string value, int priority) : base(value) {
            this.Priority = priority;
        }

        // Higher is higher priortiy
        public int Priority { get; }

        private static MeasurementToken DivideToRatio(MeasurementToken x, MeasurementToken y) {
            var divideToRatio = x.MeasurementValue.GetType().GetRuntimeMethods().Where(z => z.Name == "DivideToRatio").First().MakeGenericMethod(y.MeasurementValue.GetType());

            return new MeasurementToken(divideToRatio.Invoke(x.MeasurementValue, new[] { y.MeasurementValue }));
        }

        private static MethodInfo GetArithmeticInterfaceMethod(Type tInterface, MeasurementToken x, MeasurementToken y) {
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

            return tGenericInterface
                ?.GetTypeInfo()
                ?.DeclaredMethods
                ?.First();
        }

        private static MeasurementToken ApplyBinaryOp(Type tInterface, MeasurementToken x, MeasurementToken y) {
            var mathMethod = GetArithmeticInterfaceMethod(tInterface, x, y);

            if (mathMethod != null) {
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