namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static Unit<TResult> Cast<T, TResult>(this Unit<T> unit)
                where T : Measurement<T>
                where TResult : Measurement<TResult> {
            return new Unit<TResult>(
                name: unit.Name,
                symbol: unit.Symbol,
                unitsPerDefault: unit.UnitsPerDefault
            );
        }

        public static Unit<TResult> Cube<TSelf, TResult>(this Unit<TSelf> unit)
                where TSelf : Measurement<TSelf>, ICubableMeasurement<TResult>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));

            return new Unit<TResult>(
                name: unit.Name + " cubed",
                symbol: unit.Symbol + "³",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        public static Unit<TResult> Divide<TSelf, TThat, TResult>(this Unit<TSelf> unit, Unit<TThat> that)
                where TSelf : Measurement<TSelf>, IDividableMeasurement<TThat, TResult>
                where TThat : Measurement<TThat>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<TSelf, TThat, TResult>(that);
        }

        public static Unit<Ratio<T, TThat>> DivideToRatio<T, TThat>(this Unit<T> unit, Unit<TThat> that)
                where T : Measurement<T>
                where TThat : Measurement<TThat> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<T, TThat, Ratio<T, TThat>>(that);
        }

        public static Unit<TResult> Multiply<TSelf, TThat, TResult>(this Unit<TSelf> unit, Unit<TThat> that)
                where TSelf : Measurement<TSelf>, IMultipliableMeasurement<TThat, TResult>
                where TThat : Measurement<TThat>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<TSelf, TThat, TResult>(that);
        }

        public static Unit<Term<T, TThat>> MultiplyToTerm<T, TThat>(this Unit<T> unit, Unit<TThat> that)
                where T : Measurement<T>
                where TThat : Measurement<TThat> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<T, TThat, Term<T, TThat>>(that);
        }

        public static Unit<TResult> Square<TSelf, TResult>(this Unit<TSelf> unit)
                where TSelf : Measurement<TSelf>, ISquareableMeasurement<TResult>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));

            return new Unit<TResult>(
                name: unit.Name + " squared",
                symbol: unit.Symbol + "²",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        private static Unit<TResult> CubeTo<TIn, TResult>(this Unit<TIn> unit)
                where TIn : Measurement<TIn>
                where TResult : Measurement<TResult> {
            return new Unit<TResult>(
                name: unit.Name + " cubed",
                symbol: $"({unit.Symbol}^3)",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        private static Unit<TResult> DivideTo<TIn, TIn2, TResult>(this Unit<TIn> unit, Unit<TIn2> that)
                where TIn : Measurement<TIn>
                where TIn2 : Measurement<TIn2>
                where TResult : Measurement<TResult> {
            return new Unit<TResult>(
                name: $"{unit.Name} per {that.Name}",
                symbol: $"({unit.Symbol}/({that.Symbol}))",
                unitsPerDefault: unit.UnitsPerDefault / that.UnitsPerDefault
            );
        }

        private static Unit<TResult> MultiplyTo<TIn, TIn2, TResult>(this Unit<TIn> unit, Unit<TIn2> that)
                where TIn : Measurement<TIn>
                where TIn2 : Measurement<TIn2>
                where TResult : Measurement<TResult> {
            if (object.ReferenceEquals(unit, that)) {
                return unit.SquareTo<TIn, TResult>();
            }
            else {
                return new Unit<TResult>(
                    name: $"{unit.Name} {that.Name}",
                    symbol: $"({unit.Symbol}*{that.Symbol})",
                    unitsPerDefault: unit.UnitsPerDefault * that.UnitsPerDefault
                );
            }
        }

        private static Unit<TResult> SquareTo<TIn, TResult>(this Unit<TIn> unit)
                where TIn : Measurement<TIn>
                where TResult : Measurement<TResult> {
            return new Unit<TResult>(
                name: unit.Name + " squared",
                symbol: $"({unit.Symbol}^2)",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        public static T Multiply<T>(this Unit<T> unit, double amount) where T : Measurement<T>, new() {
            return new T().MeasurementProvider.CreateMeasurement(amount, unit);
        }

        public static T Divide<T>(this Unit<T> unit, double amount) where T : Measurement<T>, new() {
            return new T().MeasurementProvider.CreateMeasurement(1 / amount, unit);
        }
    }

    public class PrefixableUnit<T> : Unit<T> where T : Measurement<T> {

        public PrefixableUnit(string name, string symbol, double unitsPerDefault) : base(name, symbol, unitsPerDefault) {
        }
    }

    public class Unit<T> where T : Measurement<T> {
        public string Name { get; }

        public string Symbol { get; }

        public double UnitsPerDefault { get; }

        public Unit(string name, string symbol, double unitsPerDefault) {
            this.Name = name;
            this.Symbol = symbol;
            this.UnitsPerDefault = unitsPerDefault;
        }

        public override string ToString() {
            return this.Symbol;
        }
    }
}