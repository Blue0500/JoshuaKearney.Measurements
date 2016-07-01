using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JoshuaKearney.Measurements {

    public abstract class Measurement {
        private readonly double defaultUnits;

        internal Measurement(double defaultUnits) {
            this.defaultUnits = defaultUnits;
        }

        internal Measurement() : this(0) {
        }

        protected internal double DefaultUnits => this.defaultUnits;

        internal interface IMeasurementInfo {

            Measurement CreateInstance(double defaultUnits);

            IEnumerable<IUnit> UniqueUnits { get; }
        }

        internal abstract IMeasurementInfo InternalMeasurementInfo { get; }
    }

    public abstract class Measurement<TSelf> : Measurement, IEquatable<TSelf>, IComparable<TSelf>, IComparable
        where TSelf : Measurement, new() {
        private static readonly MeasurementInfo supplier = (new TSelf() as Measurement<TSelf>).Supplier;

        protected abstract MeasurementInfo Supplier { get; }

        internal override IMeasurementInfo InternalMeasurementInfo => supplier;

        protected class MeasurementInfo : IMeasurementInfo {

            public MeasurementInfo(IUnit<TSelf> defaultUnit, InstanceCreator instanceCreator, IEnumerable<IUnit<TSelf>> uniqueUnits) {
                Validate.NonNull(instanceCreator, nameof(instanceCreator));
                Validate.NonNull(defaultUnit, nameof(defaultUnit));
                Validate.NonNull(uniqueUnits, nameof(uniqueUnits));

                this.CreateInstance = instanceCreator;
                this.DefaultUnit = defaultUnit;
                this.UniqueUnits = uniqueUnits;
            }

            public delegate TSelf InstanceCreator(double defaultUnits);

            public InstanceCreator CreateInstance { get; }
            public IUnit<TSelf> DefaultUnit { get; }
            public IEnumerable<IUnit<TSelf>> UniqueUnits { get; }

            IEnumerable<IUnit> IMeasurementInfo.UniqueUnits => this.UniqueUnits;

            Measurement IMeasurementInfo.CreateInstance(double defaultUnits) => this.CreateInstance(defaultUnits);
        }

        protected static TSelf From(double defaultUnits) {
            return supplier.CreateInstance(defaultUnits);
        }

        public static TSelf From(double amount, IUnit<TSelf> definition) {
            Validate.NonNull(definition, nameof(definition));

            return supplier.CreateInstance(amount / definition.UnitsPerDefault);
        }

        public static IUnit<TSelf> DefaultUnit => supplier.DefaultUnit;

        protected Measurement(double defaultUnits) : base(defaultUnits) {
        }

        protected Measurement() {
        }

        public static TSelf MaxValue { get; } = From(double.MaxValue);

        public static TSelf MinValue { get; } = From(double.MinValue);

        public static TSelf Zero { get; } = new TSelf();

        public static IUnit<TSelf> GetDefaultUnitDefinition() {
            return DefaultUnit;
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

            return (measurement as TSelf) ?? From(measurement.DefaultUnits);
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
            return From(this.DefaultUnits + that.DefaultUnits);
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
            return From(this.DefaultUnits / factor);
        }

        public Ratio<TSelf, E> DivideToRatio<E>(E that)
                where E : Measurement, new() {
            Validate.NonNull(that, nameof(that));

            return Ratio.From(
                (this as TSelf) ?? From(this.DefaultUnits),
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
            return From(this.DefaultUnits * factor);
        }

        public Term<TSelf, E> MultiplyToTerm<E>(E that) where E : Measurement, new() {
            Validate.NonNull(that, nameof(that));

            return Term.From((this as TSelf) ??
                From(this.DefaultUnits),
                that
            );
        }

        public TSelf Negate() => From(-this.DefaultUnits);

        public TSelf Subtract(Measurement<TSelf> that) {
            Validate.NonNull(that, nameof(that));

            return From(this.DefaultUnits - that.DefaultUnits);
        }

        public double ToDouble(IUnit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.DefaultUnits * unit.UnitsPerDefault;
        }

        public override string ToString() {
            return this.ToString(
                this.Supplier.DefaultUnit as IUnit<TSelf> ??
                this.Supplier.DefaultUnit.Cast<TSelf>()
            );
        }

        public string ToString(IUnit<TSelf> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit) + " " + unit.ToString();
        }
    }
}