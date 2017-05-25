using System;

namespace JoshuaKearney.Measurements {

    public static partial class Measurement {
        // Term unit extensions
        public static Unit<Term<T1, T2>> MultiplyToTermUnit<T1, T2>(this Unit<T1> unit1, Unit<T2> unit2)
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2> {

            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(unit2, nameof(unit2));

            return new Unit<Term<T1, T2>>(
                $"{unit1.Symbol}*{unit2.Symbol}",
                unit1.DefaultsPerUnit * unit2.DefaultsPerUnit,
                Term<T1, T2>.GetProvider(unit1.MeasurementProvider, unit2.MeasurementProvider)
            );
        }

        public static Unit<TSelf> ToTermUnit<TSelf, T1, T2>(this Unit<Term<T1, T2>> unit, MeasurementProvider<TSelf> provider)
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2>
            where TSelf : Term<TSelf, T1, T2> {

            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(provider, nameof(provider));

            return new Unit<TSelf>(unit.Symbol, unit.DefaultsPerUnit, provider);
        }

        public static Unit<Term<T1, T2>> ToTermUnit<TSelf, T1, T2>(this Unit<TSelf> unit)
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2>
            where TSelf : Term<TSelf, T1, T2> {

            Validate.NonNull(unit, nameof(unit));

            TSelf self = unit.ToMeasurement();
            return new Unit<Term<T1, T2>>(
                unit.Symbol,
                unit.DefaultsPerUnit,
                Term<T1, T2>.GetProvider(self.Item1Provider, self.Item2Provider)
            );
        }

        // Ratio unit extensions
        public static Unit<Ratio<TNumerator, TDenominator>> DivideToRatioUnit<TNumerator, TDenominator>(this Unit<TNumerator> unit1, Unit<TDenominator> unit2)
            where TNumerator : IMeasurement<TNumerator>
            where TDenominator : IMeasurement<TDenominator> {

            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(unit2, nameof(unit2));

            return new Unit<Ratio<TNumerator, TDenominator>>(
                $"{unit1.Symbol}/{unit2.Symbol}",
                unit1.DefaultsPerUnit / unit2.DefaultsPerUnit,
                Ratio<TNumerator, TDenominator>.GetProvider(unit1.MeasurementProvider, unit2.MeasurementProvider)
            );
        }

        public static Unit<TSelf> ToRatioUnit<TSelf, TNumerator, TDenominator>(this Unit<Ratio<TNumerator, TDenominator>> unit, MeasurementProvider<TSelf> provider)
            where TNumerator : IMeasurement<TNumerator>
            where TDenominator : IMeasurement<TDenominator>
            where TSelf : Ratio<TSelf, TNumerator, TDenominator> {

            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(provider, nameof(provider));

            return new Unit<TSelf>(unit.Symbol, unit.DefaultsPerUnit, provider);
        }

        public static Unit<Ratio<TNumerator, TDenominator>> ToRatioUnit<TSelf, TNumerator, TDenominator>(this Unit<TSelf> unit)
            where TNumerator : IMeasurement<TNumerator>
            where TDenominator : IMeasurement<TDenominator>
            where TSelf : Ratio<TSelf, TNumerator, TDenominator> {

            Validate.NonNull(unit, nameof(unit));

            TSelf self = unit.ToMeasurement();
            return new Unit<Ratio<TNumerator, TDenominator>>(
                unit.Symbol,
                unit.DefaultsPerUnit,
                Ratio<TNumerator, TDenominator>.GetProvider(self.NumeratorProvider, self.DenominatorProvider)
            );
        }
    }

    /// <summary>
    /// Represents a unit that can be prefixed with the <see cref="Prefix"/> class. Most types of measurements require a unit to access them, which can
    /// be found in dedicated unit classes. Ex: <see cref="Distance.Units"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="JoshuaKearney.Measurements.Unit{T}" />
    public class PrefixableUnit<T> : Unit<T> where T : IMeasurement<T> {

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixableUnit{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the unit. Ex: foot</param>
        /// <param name="symbol">The symbol of the unit. Ex: ft</param>
        /// <param name="unitsPerDefault">The units per default unit for this type of measurement. Ex: 3.2808399 ft/m (meter is the default unit for length)</param>
        internal PrefixableUnit(string symbol, double defaultsPerUnit, MeasurementProvider<T> provider) : base(symbol, defaultsPerUnit, provider) { }

        public PrefixableUnit(string symbol, MeasurementProvider<T> provider) : base(symbol, provider) { }
    }

    /// <summary>
    /// Represents a unit that can be prefixed with the <see cref="Prefix"/> class. Most types of measurements require a unit to access them, which can
    /// be found in dedicated unit classes. Ex: <see cref="Distance.Units"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Unit<T> : Measurement<T>, IEquatable<Unit<T>> where T : IMeasurement<T> {
        /// <summary>
        /// Gets the symbol of this unit.
        /// </summary>
        public string Symbol { get; }

        public double DefaultsPerUnit => this.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit{T}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="unitsPerDefault">The units per default unit for this measurement.</param>
        internal Unit(string symbol, double defaultsPerUnit, MeasurementProvider<T> provider) : base(defaultsPerUnit) {
            Validate.NonNull(symbol, nameof(symbol));
            Validate.NonNull(provider, nameof(provider));

            this.Symbol = $"({symbol})";
            this.MeasurementProvider = provider;
        }

        public Unit(string symbol, MeasurementProvider<T> provider) : this(symbol, 1, provider) { }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>

        // The symbol will always have parenthensis around it
        public override string ToString() => this.Symbol.Substring(1, this.Symbol.Length - 2);

        public bool Equals(Unit<T> other) {
            if (other == null) {
                return false;
            }

            return this.Value == other.Value && this.Symbol == other.Symbol;
        }

        public override bool Equals(object that) {
            if (that is Unit<T> unit) {
                return this.Equals(unit);
            }
            else {
                return base.Equals(that);
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override MeasurementProvider<T> MeasurementProvider { get; }
    }
}