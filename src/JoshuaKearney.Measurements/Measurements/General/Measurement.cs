using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace JoshuaKearney.Measurements {
    public interface IMeasurement<T> : IComparable<IMeasurement<T>>, IEquatable<IMeasurement<T>> where T : IMeasurement<T> {
        MeasurementProvider<T> MeasurementProvider { get; }
        double ToDouble(Unit<T> unit);
        bool Equals(object other);
        string ToString();
    }

    public interface IAddableMeasurement<T> : IMeasurement<T> where T : IMeasurement<T> { }

    public interface IAddableMeasurement<T, TResult> : IMeasurement<T> where T : IMeasurement<T> where TResult : IMeasurement<TResult> {
        TResult Add(IMeasurement<T> other);
        TResult Subtract(IMeasurement<T> other);
    } 

    public abstract class Measurement {
        internal Measurement() { }

        public static bool IsInfinity<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsNan<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsNaN(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsNegativeInfinity<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsNegativeInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsPositiveInfinity<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsPositiveInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsZero<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) == 0;
        }

        /// <summary>
        /// Returns the maximum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static T Max<T>(IMeasurement<T> t1, IMeasurement<T> t2) where T : IMeasurement<T> {
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
        public static T Max<T>(IMeasurement<T> t1, IMeasurement<T> t2, params IMeasurement<T>[] measurements) where T : IMeasurement<T> {
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
        public static T Min<T>(IMeasurement<T> t1, IMeasurement<T> t2) where T : IMeasurement<T> {
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
        public static T Min<T>(IMeasurement<T> t1, IMeasurement<T> t2, params IMeasurement<T>[] measurments) where T : IMeasurement<T> {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));
            Validate.NonNull(measurments, nameof(measurments));
            Validate.NonEmpty(measurments, nameof(measurments));

            return measurments.Concat(new[] { t1, t2 }).Aggregate((x, y) => Min(x, y)).ToMeasurement();
        }

        public static int Compare<T>(IMeasurement<T> measurement1, IMeasurement<T> measurement2) where T : IMeasurement<T> {
            if (measurement1 == null && measurement2 == null) {
                return 0;
            }
            else if (measurement1 == null || measurement2 == null) {
                return 1;
            }

            return measurement1.ToDouble(
                measurement1.MeasurementProvider.DefaultUnit
            )
            .CompareTo(
                measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit)
            );
        }

        public static bool Equals<T>(IMeasurement<T> measurement1, IMeasurement<T> measurement2) where T : IMeasurement<T> {
            if (measurement1 == null && measurement2 == null) {
                return true;
            }
            else if (measurement1 == null || measurement2 == null) {
                return false;
            }

            return measurement1.ToDouble(
                measurement1.MeasurementProvider.DefaultUnit
            )
            .Equals(
                measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit)
            );
        }
    }

    public abstract class Measurement<TSelf> : Measurement, IEquatable<IMeasurement<TSelf>>, IComparable<IMeasurement<TSelf>>, IComparable, IMeasurement<TSelf>
        where TSelf : IMeasurement<TSelf> {

        // For unit
        internal Measurement(double amount) {
            this.Value = amount;
        }

        protected Measurement(double amount, Unit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            this.Value = amount * unit.Value;
        }

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

        public static TSelf operator -(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.Negate();
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
        public int CompareTo(IMeasurement<TSelf> that) => Compare(this, that);

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
        public bool Equals(IMeasurement<TSelf> that) => Equals(this, that);

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