using System;
using System.Linq;

namespace JoshuaKearney.Measurements {

    /// <summary>
    /// The base class to represent all measurements within JoshuaKearney.Measurements
    /// </summary>
    /// <typeparam name="TSelf">The type of the derived class</typeparam>
    /// <seealso cref="System.IEquatable{TSelf}" />
    /// <seealso cref="System.IComparable{TSelf}" />
    /// <seealso cref="System.IComparable" />
    public abstract class Measurement<TSelf> : IEquatable<TSelf>, IComparable<TSelf>, IComparable,
        IMultipliableMeasurement<DoubleMeasurement, TSelf>,
        IDividableMeasurement<DoubleMeasurement, TSelf>
        where TSelf : Measurement<TSelf> {

        // For unit
        internal Measurement(double amount) {
            this.Value = amount;
        }

        protected Measurement(double amount, Unit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            this.Value = amount * unit.Value;
        }

        private static bool hasUnit = false;

        protected Measurement() {
            this.Value = 0;
        }

        public abstract MeasurementProvider<TSelf> MeasurementProvider { get; }

        protected double Value { get; }

        public static implicit operator Ratio<TSelf, DoubleMeasurement>(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement.ToRatio();
        }

        public Unit<TSelf> ToUnit(string symbol) {
            return new Unit<TSelf>(symbol, this.Value, this.MeasurementProvider);
        }
        public PrefixableUnit<TSelf> ToPrefixableUnit(string symbol) {
            Validate.NonNull(symbol, nameof(symbol));
            return new PrefixableUnit<TSelf>(symbol, this.Value, this.MeasurementProvider);
        }

        public static implicit operator TSelf(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement.ToMeasurement();
        }

        public TSelf ToMeasurement() {
            return this as TSelf ?? this.MeasurementProvider.CreateMeasurement(
                this.Value, 
                this.MeasurementProvider.DefaultUnit
            );
        }

        public static bool IsInfinity(Measurement<TSelf> measurement) {
            return double.IsInfinity(measurement.Value);
        }

        public static bool IsNan(Measurement<TSelf> measurement) {
            return double.IsNaN(measurement.Value);
        }

        public static bool IsNegativeInfinity(Measurement<TSelf> measurement) {
            return double.IsNegativeInfinity(measurement.Value);
        }

        public static bool IsPositiveInfinity(Measurement<TSelf> measurement) {
            return double.IsPositiveInfinity(measurement.Value);
        }

        /// <summary>
        /// Returns the maximum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static TSelf Max(Measurement<TSelf> t1, Measurement<TSelf> t2) {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            if (t1 >= t2) {
                return t1.ToMeasurement();
            }
            else {
                return t2.ToMeasurement();
            }
        }

        /// <summary>
        /// Returns the maximum of this instance and the specified measurement.
        /// </summary>
        /// <param name="measurements">The other measurements.</param>
        /// <returns></returns>
        public static TSelf Max(params Measurement<TSelf>[] measurements) {
            Validate.NonNull(measurements, nameof(measurements));
            Validate.NonEmpty(measurements, nameof(measurements));

            return measurements.Aggregate((x, y) => Max(x, y)).ToMeasurement();
        }

        /// <summary>
        /// Returns the minimum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static TSelf Min(Measurement<TSelf> t1, Measurement<TSelf> t2) {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            if (t1 <= t2) {
                return t1;
            }
            else {
                return t2;
            }
        }

        /// <summary>
        /// Returns the minimum of this instance and the specified measurement.
        /// </summary>
        /// <param name="measurments">The other measurments.</param>
        /// <returns></returns>
        public static TSelf Min(params Measurement<TSelf>[] measurments) {
            Validate.NonNull(measurments, nameof(measurments));
            Validate.NonEmpty(measurments, nameof(measurments));

            return measurments.Aggregate((x, y) => Min(x, y));
        }

        public static TSelf operator -(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement.Negate();
        }

        public static TSelf operator -(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (measurement == null || measurement2 == null) {
                return null;
            }

            return measurement.Subtract(measurement2);
        }

        public static bool operator !=(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (object.ReferenceEquals(measurement, null)) {
                if (object.ReferenceEquals(measurement2, null)) {
                    return false;
                }
                else {
                    return true;
                }
            }

            return !measurement.Equals(measurement2);
        }

        public static TSelf operator *(Measurement<TSelf> measurement, double factor) {
            if (measurement == null) {
                return null;
            }

            return measurement.Multiply(factor);
        }

        public static TSelf operator *(double factor, Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement.Multiply(factor);
        }

        public static TSelf operator /(Measurement<TSelf> measurement, double factor) {
            if (measurement == null) {
                return null;
            }

            return measurement.Divide(factor);
        }

        public static TSelf operator +(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (measurement == null || measurement2 == null) {
                return null;
            }

            return measurement.Add(measurement2);
        }

        public static TSelf operator +(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement as TSelf;
        }

        public static bool operator <(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (measurement == null) {
                if (measurement2 == null) {
                    return false;
                }
                else {
                    return true;
                }
            }

            return measurement.CompareTo(measurement2) < 0;
        }

        public static bool operator <=(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (measurement == null) {
                return true;
            }

            return measurement.CompareTo(measurement2) <= 0;
        }

        public static bool operator ==(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (object.ReferenceEquals(measurement, null)) {
                if (object.ReferenceEquals(measurement2, null)) {
                    return true;
                }
                else {
                    return false;
                }
            }

            return measurement.Equals(measurement2);
        }

        public static bool operator >(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (measurement == null) {
                return false;
            }

            return measurement.CompareTo(measurement2) > 0;
        }

        public static bool operator >=(Measurement<TSelf> measurement, Measurement<TSelf> measurement2) {
            if (measurement == null) {
                if (measurement2 == null) {
                    return true;
                }
                else {
                    return false;
                }
            }

            return measurement.CompareTo(measurement2) >= 0;
        }

        /// <summary>
        /// Returns the absolute value of this instance
        /// </summary>
        /// <returns></returns>

        public TSelf Abs() => this.MeasurementProvider.CreateMeasurement(Math.Abs(this.Value), this.MeasurementProvider.DefaultUnit);

        /// <summary>
        /// Adds this instance to the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public TSelf Add(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));
            return this.MeasurementProvider.CreateMeasurement(
                this.Value + that.Value, 
                this.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(TSelf that) {
            if (that == null) {
                return 1;
            }

            return this.Value.CompareTo(that.Value);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj) {
            TSelf measurement = obj as TSelf;

            if (measurement == null) {
                return 1;
            }
            else {
                return this.CompareTo(measurement);
            }
        }

        ///// <summary>
        ///// Creates a unit with the given name and symbol from this measurement's value
        ///// </summary>
        ///// <param name="name">The name.</param>
        ///// <param name="symbol">The symbol.</param>
        ///// <returns></returns>
        //public Unit<TSelf> CreateUnit(string name, string symbol) {
        //    return new Unit<TSelf>(name, symbol, 1 / this.DefaultUnits);
        //}

        /// <summary>
        /// Divides this instance by the specified ratio.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="ratio">The ratio.</param>
        /// <returns></returns>
        public E Divide<E, F>(Ratio<F, TSelf, E> ratio)
                where F : Ratio<F, TSelf, E>
                where E : Measurement<E> {
            return ratio.Reciprocal().Multiply(this);
        }

        /// <summary>
        /// Divides this instance by the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public DoubleMeasurement Divide(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));
            return this.Value / that.Value;
        }

        /// <summary>
        /// Divides this instance by the specified double.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns></returns>
        public TSelf Divide(double factor) {
            return this.MeasurementProvider.CreateMeasurement(
                this.Value / factor, 
                this.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Divides this instance by another type of measurement to create a ratio.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public Ratio<TSelf, E> DivideToRatio<E>(Measurement<E> that)
                where E : Measurement<E> {
            Validate.NonNull(that, nameof(that));

            return new Ratio<TSelf, E>(this, that);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="that">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object that) {
            TSelf cast = that as TSelf;

            if (cast != null) {
                return this.Equals(cast);
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified measurement, is equal to this instance.
        /// </summary>
        /// <param name="that">The measurement to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified measurement> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TSelf that) {
            if (object.ReferenceEquals(that, null)) {
                return false;
            }
            else {
                return this.Value.Equals(that.Value);
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => this.Value.GetHashCode();

        TSelf IDividableMeasurement<DoubleMeasurement, TSelf>.Divide(DoubleMeasurement measurement2) {
            return this.Divide(measurement2.ToDouble());
        }

        TSelf IMultipliableMeasurement<DoubleMeasurement, TSelf>.Multiply(DoubleMeasurement measurement2) {
            return this.Multiply(measurement2.ToDouble());
        }

        /// <summary>
        /// Multiplies this instance by the specified double.
        /// </summary>
        /// <param name="factor">The double to multiply by.</param>
        /// <returns></returns>
        public TSelf Multiply(double factor) {
            return this.MeasurementProvider.CreateMeasurement(
                this.Value * factor, 
                this.MeasurementProvider.DefaultUnit
            );
        }

        public T Multiply<T, E>(Ratio<E, T, TSelf> ratio)
            where E : Ratio<E, T, TSelf>
            where T : Measurement<T> {
            return ratio.Multiply(this);
        }

        /// <summary>
        /// Multiplies this instance by another type of measurement, creating a term
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public Term<TSelf, E> MultiplyToTerm<E>(Measurement<E> that) where E : Measurement<E> {
            Validate.NonNull(that, nameof(that));

            return new Term<TSelf, E>(this, that);
        }

        /// <summary>
        /// Negates this instance.
        /// </summary>
        /// <returns></returns>
        public TSelf Negate() => this.MeasurementProvider.CreateMeasurement(
            -this.Value,
            this.MeasurementProvider.DefaultUnit
        );

        public Ratio<DoubleMeasurement, TSelf> Reciprocal() {
            return new Ratio<DoubleMeasurement, TSelf>(1, this);
        }

        /// <summary>
        /// Subtracts the this instance by another measurement
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public TSelf Subtract(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));

            return this.MeasurementProvider.CreateMeasurement(
                this.Value - that.Value,
                this.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Returns a <see cref="System.Double" /> that represents this instance.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public double ToDouble(Unit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.Value / unit.Value;
        }

        public Ratio<TSelf, DoubleMeasurement> ToRatio() {
            return new Ratio<TSelf, DoubleMeasurement>(this, 1);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.ToString(this.MeasurementProvider.ParsableUnits.First());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(Unit<TSelf> unit, string format) {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(format, nameof(format));

            string unitStr = IsInfinity(this) || IsNan(this)
                ? ""
                : " " + unit.ToString();

            return (this.ToDouble(unit).ToString(format) + unitStr).Trim();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(Unit<TSelf> unit1, params Unit<TSelf>[] units) {
            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(units, nameof(units));

            units = units.Concat(new[] { unit1 }).OrderBy(x => this.ToDouble(x)).ToArray();
            var unit = units.FirstOrDefault(x => this.ToDouble(x) >= 1) ?? units.FirstOrDefault() ?? this.MeasurementProvider.ParsableUnits.FirstOrDefault();

            return this.ToString(unit, "0.##");
        }        
    }
}