using System;
using System.Linq;

namespace JoshuaKearney.Measurements {
    public interface IMeasurement { }

    public interface IMeasurement<T> : IComparable<IMeasurement<T>>, IEquatable<IMeasurement<T>>, IMeasurement where T : IMeasurement<T> {
        MeasurementProvider<T> MeasurementProvider { get; }
        double ToDouble(Unit<T> unit);
        bool Equals(object other);
        string ToString();
    }

    /// <summary>
    /// The base class to represent all measurements within JoshuaKearney.Measurements
    /// </summary>
    /// <typeparam name="TSelf">The type of the derived class</typeparam>
    /// <seealso cref="System.IEquatable{TSelf}" />
    /// <seealso cref="System.IComparable{TSelf}" />
    /// <seealso cref="System.IComparable" />
    public abstract class Measurement<TSelf> : IEquatable<IMeasurement<TSelf>>, IComparable<IMeasurement<TSelf>>, IComparable, IMeasurement, IMeasurement<TSelf>
        where TSelf : IMeasurement<TSelf> {

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

        public static implicit operator TSelf(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.ToMeasurement();
        }

        public static bool IsInfinity(IMeasurement<TSelf> measurement) {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsNan(IMeasurement<TSelf> measurement) {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsNaN(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsNegativeInfinity(IMeasurement<TSelf> measurement) {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsNegativeInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsPositiveInfinity(IMeasurement<TSelf> measurement) {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsPositiveInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsZero(IMeasurement<TSelf> measurement) {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) == 0;
        }

        /// <summary>
        /// Returns the maximum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static TSelf Max(IMeasurement<TSelf> t1, IMeasurement<TSelf> t2) {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            if (t1.CompareTo(t2) >= 0) {
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
        public static TSelf Max(IMeasurement<TSelf> t1, IMeasurement<TSelf> t2, params IMeasurement<TSelf>[] measurements) {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));
            Validate.NonNull(measurements, nameof(measurements));
            Validate.NonEmpty(measurements, nameof(measurements));

            return measurements.Concat(new[] { t1, t2 }).Aggregate((x, y) => Max(x, y)).ToMeasurement();
        }

        /// <summary>
        /// Returns the minimum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static TSelf Min(IMeasurement<TSelf> t1, IMeasurement<TSelf> t2) {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            if (t1.CompareTo(t2) <= 0) {
                return t1.ToMeasurement();
            }
            else {
                return t2.ToMeasurement();
            }
        }

        /// <summary>
        /// Returns the minimum of this instance and the specified measurement.
        /// </summary>
        /// <param name="measurments">The other measurments.</param>
        /// <returns></returns>
        public static TSelf Min(IMeasurement<TSelf> t1, IMeasurement<TSelf> t2, params IMeasurement<TSelf>[] measurments) {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));
            Validate.NonNull(measurments, nameof(measurments));
            Validate.NonEmpty(measurments, nameof(measurments));

            return measurments.Concat(new[] { t1, t2 }).Aggregate((x, y) => Min(x, y)).ToMeasurement();
        }

        public static TSelf operator -(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.Negate();
        }

        public static TSelf operator -(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
            if (measurement == null || measurement2 == null) {
                return default(TSelf);
            }

            return measurement.Subtract(measurement2);
        }

        public static bool operator !=(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
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
                return default(TSelf);
            }

            return measurement.Multiply(factor);
        }

        public static TSelf operator *(double factor, Measurement<TSelf> measurement) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.Multiply(factor);
        }

        public static TSelf operator /(Measurement<TSelf> measurement, double factor) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.Divide(factor);
        }

        public static TSelf operator +(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
            if (measurement == null || measurement2 == null) {
                return default(TSelf);
            }

            return measurement.Add(measurement2);
        }

        public static TSelf operator +(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.ToMeasurement();
        }

        public static bool operator <(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
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

        public static bool operator <=(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
            if (measurement == null) {
                return true;
            }

            return measurement.CompareTo(measurement2) <= 0;
        }

        public static bool operator ==(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
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

        public static bool operator >(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
            if (measurement == null) {
                return false;
            }

            return measurement.CompareTo(measurement2) > 0;
        }

        public static bool operator >=(Measurement<TSelf> measurement, IMeasurement<TSelf> measurement2) {
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
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(IMeasurement<TSelf> that) {
            if (that == null) {
                return 1;
            }

            return this.ToDouble(this.MeasurementProvider.DefaultUnit).CompareTo(that.ToDouble(this.MeasurementProvider.DefaultUnit));
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj) {
            if (obj == null || !(obj is TSelf)) {
                return 1;
            }

            TSelf measurement = (TSelf)obj;
            return this.CompareTo(measurement);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="that">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object that) {
            if (that == null || !(that is TSelf)) {
                return false;
            }

            TSelf cast = (TSelf)that;
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
        public bool Equals(IMeasurement<TSelf> that) {
            if (object.ReferenceEquals(that, null)) {
                return false;
            }
            else {
                return this.ToDouble(this.MeasurementProvider.DefaultUnit).Equals(that.ToDouble(this.MeasurementProvider.DefaultUnit));
            }
        }

        public Ratio<DoubleMeasurement, TSelf> Reciprocal() {
            return new Ratio<DoubleMeasurement, TSelf>(new DoubleMeasurement(1), this);
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
        /// Returns a <see cref="System.Double" /> that represents this instance.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public double ToDouble(Unit<TSelf> unit)  {
            Validate.NonNull(unit, nameof(unit));

            return
                this.Value /
                unit.Value;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => this.Value.GetHashCode();
    }
}