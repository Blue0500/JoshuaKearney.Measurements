namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        /// <summary>
        /// Casts the specified unit to a different type of measurement. This can result in unchecked, bad conversions. Only use this if you know what you're doing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<TResult> Cast<T, TResult>(this Unit<T> unit)
                where T : Measurement<T>
                where TResult : Measurement<TResult> {
            return new Unit<TResult>(
                name: unit.Name,
                symbol: unit.Symbol,
                unitsPerDefault: unit.UnitsPerDefault
            );
        }

        /// <summary>
        /// Cubes this unit.
        /// </summary>
        /// <typeparam name="TSelf">The type of this unit.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Divides this unit by the specified unit
        /// </summary>
        /// <typeparam name="TSelf">The type of this unit.</typeparam>
        /// <typeparam name="TThat">The type of the that.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public static Unit<TResult> Divide<TSelf, TThat, TResult>(this Unit<TSelf> unit, Unit<TThat> that)
                where TSelf : Measurement<TSelf>, IDividableMeasurement<TThat, TResult>
                where TThat : Measurement<TThat>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<TSelf, TThat, TResult>(that);
        }

        /// <summary>
        /// Divides this unit by another unit, making a unit of a ratio.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TThat">The type of the that.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public static Unit<Ratio<T, TThat>> DivideToRatio<T, TThat>(this Unit<T> unit, Unit<TThat> that)
                where T : Measurement<T>
                where TThat : Measurement<TThat> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<T, TThat, Ratio<T, TThat>>(that);
        }

        /// <summary>
        /// Multiplies this unit with another unit.
        /// </summary>
        /// <typeparam name="TSelf">The type of the self.</typeparam>
        /// <typeparam name="TThat">The type of the that.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public static Unit<TResult> Multiply<TSelf, TThat, TResult>(this Unit<TSelf> unit, Unit<TThat> that)
                where TSelf : Measurement<TSelf>, IMultipliableMeasurement<TThat, TResult>
                where TThat : Measurement<TThat>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<TSelf, TThat, TResult>(that);
        }

        /// <summary>
        /// Multiplies this unit with another unit, making a unit of a term
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TThat">The type of the that.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public static Unit<Term<T, TThat>> MultiplyToTerm<T, TThat>(this Unit<T> unit, Unit<TThat> that)
                where T : Measurement<T>
                where TThat : Measurement<TThat> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<T, TThat, Term<T, TThat>>(that);
        }

        /// <summary>
        /// Squares this unit.
        /// </summary>
        /// <typeparam name="TSelf">The type of the self.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
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
    }

    /// <summary>
    /// Represents a unit that can be prefixed with the <see cref="Prefix"/> class. Most types of measurements require a unit to access them, which can
    /// be found in dedicated unit classes. Ex: <see cref="Distance.Units"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="JoshuaKearney.Measurements.Unit{T}" />
    public class PrefixableUnit<T> : Unit<T> where T : Measurement<T> {

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixableUnit{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the unit. Ex: foot</param>
        /// <param name="symbol">The symbol of the unit. Ex: ft</param>
        /// <param name="unitsPerDefault">The units per default unit for this type of measurement. Ex: 3.2808399 ft/m (meter is the default unit for length)</param>
        public PrefixableUnit(string name, string symbol, double unitsPerDefault) : base(name, symbol, unitsPerDefault) {
        }
    }

    /// <summary>
    /// Represents a unit that can be prefixed with the <see cref="Prefix"/> class. Most types of measurements require a unit to access them, which can
    /// be found in dedicated unit classes. Ex: <see cref="Distance.Units"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Unit<T> where T : Measurement<T> {

        /// <summary>
        /// Gets the name of this unit.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the symbol of this unit.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Gets the units per default unit for this measurement. Ex: 3.2808399 ft/m (meter is the default unit for length)
        /// </summary>
        public double UnitsPerDefault { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit{T}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="unitsPerDefault">The units per default unit for this measurement.</param>
        public Unit(string name, string symbol, double unitsPerDefault) {
            this.Name = name;
            this.Symbol = symbol;
            this.UnitsPerDefault = unitsPerDefault;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.Symbol;
        }
    }
}