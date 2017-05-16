using System;

namespace JoshuaKearney.Measurements {
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