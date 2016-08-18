using System;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public abstract class Measurement<TSelf> : IEquatable<TSelf>, IComparable<TSelf>, IComparable
        where TSelf : Measurement<TSelf> {
        protected internal double DefaultUnits { get; }

        public abstract IMeasurementProvider<TSelf> MeasurementProvider { get; }

        protected Measurement(double amount, IUnit<TSelf> unit) {
            this.DefaultUnits = amount / unit.UnitsPerDefault;
        }

        protected Measurement() {
            this.DefaultUnits = 0;
        }

        public static TSelf operator -(Measurement<TSelf> measurement) {
            if (measurement == null) {
                return null;
            }

            return measurement.Negate();
        }

        public static TSelf operator -(Measurement<TSelf> measurement, TSelf measurement2) {
            if (measurement == null || measurement2 == null) {
                return null;
            }

            return measurement.Subtract(measurement2);
        }

        public static bool operator !=(Measurement<TSelf> measurement, TSelf measurement2) {
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

        public static TSelf operator +(Measurement<TSelf> measurement, TSelf measurement2) {
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

        public static bool operator <(Measurement<TSelf> measurement, TSelf measurement2) {
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

        public static bool operator <=(Measurement<TSelf> measurement, TSelf measurement2) {
            if (measurement == null) {
                return true;
            }

            return measurement.CompareTo(measurement2) <= 0;
        }

        public static bool operator ==(Measurement<TSelf> measurement, TSelf measurement2) {
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

        public static bool operator >(Measurement<TSelf> measurement, TSelf measurement2) {
            if (measurement == null) {
                return false;
            }

            return measurement.CompareTo(measurement2) > 0;
        }

        public static bool operator >=(Measurement<TSelf> measurement, TSelf measurement2) {
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

        //public static TSelf Parse(string input) {
        //    Validate.NonNull(input, nameof(input));
        //    return Parser.Parse<TSelf>(input);
        //}

        //public static bool TryParse(string input, out TSelf result) {
        //    Validate.NonNull(input, nameof(input));
        //    return Parser.TryParse(input, out result);
        //}

        public TSelf Add(TSelf that) {
            Validate.NonNull(that, nameof(that));
            return that.MeasurementProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits + that.DefaultUnits);
        }

        public int CompareTo(TSelf that) {
            if (that == null) {
                return 1;
            }

            return this.DefaultUnits.CompareTo(that.DefaultUnits);
        }

        public int CompareTo(object obj) {
            TSelf measurement = obj as TSelf;

            if (measurement == null) {
                return 1;
            }
            else {
                return this.CompareTo(measurement);
            }
        }

        public double Divide(TSelf that) {
            Validate.NonNull(that, nameof(that));
            return this.DefaultUnits / that.DefaultUnits;
        }

        public TSelf Divide(double factor) {
            return this.MeasurementProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / factor);
        }

        public Ratio<TSelf, E> DivideToRatio<E>(E that)
                where E : Measurement<E> {
            Validate.NonNull(that, nameof(that));

            return new Ratio<TSelf, E>(this as TSelf, that);
        }

        public override bool Equals(object that) {
            TSelf cast = that as TSelf;

            if (cast != null) {
                return this.Equals(cast);
            }
            else {
                return false;
            }
        }

        public bool Equals(TSelf that) {
            if (object.ReferenceEquals(that, null)) {
                return false;
            }
            else {
                return this.DefaultUnits.Equals(that.DefaultUnits);
            }
        }

        public override int GetHashCode() => this.DefaultUnits.GetHashCode();

        public TSelf Multiply(double factor) {
            return this.MeasurementProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits * factor);
        }

        public Term<TSelf, E> MultiplyToTerm<E>(E that) where E : Measurement<E> {
            Validate.NonNull(that, nameof(that));

            return new Term<TSelf, E>(this as TSelf, that);
        }

        public TSelf Subtract(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));

            return this.MeasurementProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits - that.DefaultUnits);
        }

        public double ToDouble(IUnit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.DefaultUnits * unit.UnitsPerDefault;
        }

        public TSelf Max(TSelf that) {
            Validate.NonNull(that, nameof(that));

            if (this >= that) {
                return (TSelf)this;
            }
            else {
                return that;
            }
        }

        public TSelf Max(params TSelf[] measurements) {
            Validate.NonNull(measurements, nameof(measurements));
            Validate.NonEmpty(measurements, nameof(measurements));

            return measurements.Aggregate((x, y) => x.Max(y));
        }

        public TSelf Min(TSelf that) {
            Validate.NonNull(that, nameof(that));

            if (this <= that) {
                return (TSelf)this;
            }
            else {
                return that;
            }
        }

        public TSelf Min(params TSelf[] measurments) {
            Validate.NonNull(measurments, nameof(measurments));
            Validate.NonEmpty(measurments, nameof(measurments));

            return measurments.Aggregate((x, y) => x.Min(y));
        }

        public TSelf Abs() {
            return this.Select(Math.Abs);
        }

        public TSelf Negate() => this.Select(x => -x);

        protected TSelf Select(Func<double, double> func) {
            Validate.NonNull(func, nameof(func));

            return this.MeasurementProvider.CreateMeasurementWithDefaultUnits(func(this.DefaultUnits));
        }

        public override string ToString() {
            return this.ToString(
                this.MeasurementProvider.DefaultUnit as IUnit<TSelf>
            );
        }

        public string ToString(IUnit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit) + " " + unit.ToString();
        }
    }
}