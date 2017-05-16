using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoshuaKearney.Measurements {
    public abstract class Measurement<TSelf> : IEquatable<IMeasurement<TSelf>>, IComparable<IMeasurement<TSelf>>, IComparable, IMeasurement<TSelf>
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

        public static implicit operator TSelf(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return default(TSelf);
            }

            return measurement.ToMeasurement();
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
        public int CompareTo(IMeasurement<TSelf> that) => Measurement.Compare(this, that);

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
        public double ToDouble(Unit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.Value / unit.Value;
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