using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JoshuaKearney.Measurements {

    public abstract class Measurement {
        private static Dictionary<Type, MeasurementInfo> suppliers = new Dictionary<Type, MeasurementInfo>();
        private readonly double defaultUnits;

        internal Measurement(double storedUnits) {
            this.defaultUnits = storedUnits;
        }

        internal Measurement() : this(0) {
        }

        internal double DefaultUnits => this.defaultUnits;
        protected abstract MeasurementInfo Supplier { get; }

        public static T From<T>(double amount, IUnit<T> definition) where T : Measurement, new() {
            Validate.NonNull(definition, nameof(definition));

            EnsureTypeIsCached(typeof(T));
            return (T)suppliers[typeof(T)].CreateInstance(amount / definition.UnitsPerStored);
        }

        internal static Measurement From(Type tMeasurement, double defaultUnits) {
            EnsureTypeIsCached(tMeasurement);
            return suppliers[tMeasurement].CreateInstance(defaultUnits);
        }

        internal static IUnit<T> GetDefaultUnitDefinition<T>() where T : Measurement, new() {
            EnsureTypeIsCached(typeof(T));
            return (IUnit<T>)suppliers[typeof(T)].StoredUnitDefinition;
        }

        internal static IEnumerable<IUnit> GetUnits(Type measurementType) {
            EnsureTypeIsCached(measurementType);
            return suppliers[measurementType].UniqueUnits.Value;
        }

        protected static T From<T>(double defaultUnits) where T : Measurement, new() {
            return From<T>(defaultUnits);
        }

        private static void EnsureTypeIsCached(Type measurementType) {
            if (!suppliers.ContainsKey(measurementType)) {
                suppliers.Add(measurementType, ((Measurement)Activator.CreateInstance(measurementType)).Supplier);
            }
        }
    }

    public abstract class Measurement<TSelf> : Measurement, IEquatable<TSelf>, IComparable<TSelf>, IComparable
        where TSelf : Measurement, new() {

        protected Measurement(double storedUnits) : base(storedUnits) {
        }

        protected Measurement() {
        }

        public static TSelf MaxValue { get; } = From<TSelf>(double.MaxValue);

        public static TSelf MinValue { get; } = From<TSelf>(double.MinValue);

        public static TSelf Zero { get; } = new TSelf();

        public static IUnit<TSelf> GetDefaultUnitDefinition() {
            return Measurement.GetDefaultUnitDefinition<TSelf>();
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

            return (measurement as TSelf) ?? From<TSelf>(measurement.DefaultUnits);
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

        public static TSelf Parse(string input) {
            Validate.NonNull(input, nameof(input));
            return Parser.Parse<TSelf>(input);
        }

        public static bool TryParse(string input, out TSelf result) {
            Validate.NonNull(input, nameof(input));
            return Parser.TryParse(input, out result);
        }

        public TSelf Add(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));
            return From<TSelf>(this.DefaultUnits + that.DefaultUnits);
        }

        public int CompareTo(TSelf that) {
            if (that == null) {
                return 1;
            }

            return this.DefaultUnits.CompareTo(that.DefaultUnits);
        }

        public double Divide(TSelf that) {
            Validate.NonNull(that, nameof(that));
            return this.DefaultUnits / that.DefaultUnits;
        }

        public TSelf Divide(double factor) {
            return From<TSelf>(this.DefaultUnits / factor);
        }

        public Ratio<TSelf, E> DivideToRatio<E>(E that)
                where E : Measurement, new() {
            Validate.NonNull(that, nameof(that));

            return Ratio.From(
                (this as TSelf) ?? From<TSelf>(this.DefaultUnits),
                that
            );
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
            return From<TSelf>(this.DefaultUnits * factor);
        }

        public Term<TSelf, E> MultiplyToTerm<E>(E that) where E : Measurement, new() {
            Validate.NonNull(that, nameof(that));

            return Term.From((this as TSelf) ??
                From<TSelf>(this.DefaultUnits),
                that
            );
        }

        public TSelf Negate() => From<TSelf>(-this.DefaultUnits);

        public TSelf Subtract(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));

            return From<TSelf>(this.DefaultUnits - that.DefaultUnits);
        }

        public double ToDouble(IUnit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.DefaultUnits * unit.UnitsPerStored;
        }

        public override string ToString() {
            return this.ToString(
                this.Supplier.StoredUnitDefinition as IUnit<TSelf> ??
                this.Supplier.StoredUnitDefinition.Cast<TSelf>()
            );
        }

        public string ToString(IUnit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit) + " " + unit.ToString();
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
    }
}