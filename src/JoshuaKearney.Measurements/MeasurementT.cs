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
    public abstract class Measurement<TSelf> : IEquatable<TSelf>, IComparable<TSelf>, IComparable, IMeasurement<TSelf>
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

        protected double Value { get; }

        public abstract MeasurementProvider<TSelf> MeasurementProvider { get; }

        public static implicit operator Ratio<TSelf, DoubleMeasurement>(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement.ToRatio();
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
            if (obj is TSelf self) {
                return this.CompareTo(self);
            }
            else {
                return 1;
            }           
        }        

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="that">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object that) {
            if (that is TSelf cast) {
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
                return this.ToDouble(this.MeasurementProvider.DefaultUnit).Equals(that.ToDouble(this.MeasurementProvider.DefaultUnit));
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode() => this.Value.GetHashCode();     

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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.ToString(this.MeasurementProvider.ParsableUnits.First());
        }        
    }
}