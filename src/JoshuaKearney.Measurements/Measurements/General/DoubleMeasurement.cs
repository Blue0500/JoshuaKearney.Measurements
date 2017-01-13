using System;
using System.Collections.Generic;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {

    public static partial class MeasurementExtensions {
        public static T Multiply<T>(this Measurement<DoubleMeasurement> d, Measurement<T> measurement) where T : Measurement<T> {
            Validate.NonNull(d, nameof(d));
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.Multiply(d.ToDouble());
        }

        public static TNum Simplify<TSelf, TNum>(this Ratio<TSelf, TNum, DoubleMeasurement> measurement)
                where TSelf : Ratio<TSelf, TNum, DoubleMeasurement>
                where TNum : Measurement<TNum> {
            Validate.NonNull(measurement, nameof(measurement));
            return measurement.Select((x, y) => x.Divide(y));
        }

        public static double SimplifyToDouble<TSelf>(this Term<TSelf, DoubleMeasurement, DoubleMeasurement> term)
                where TSelf : Term<TSelf, DoubleMeasurement, DoubleMeasurement> {
            Validate.NonNull(term, nameof(term));
            return term.Select((x, y) => x.Divide(y.ToDouble()));
        }

        public static TFirst Simplify<TSelf, TFirst>(this Term<TSelf, TFirst, DoubleMeasurement> measurement)
                where TFirst : Measurement<TFirst>
                where TSelf : Term<TSelf, TFirst, DoubleMeasurement> {
            Validate.NonNull(measurement, nameof(measurement));
            return measurement.Select((x, y) => x.Multiply(y));
        }

        public static TSecond Simplify<TSelf, TSecond>(this Term<TSelf, DoubleMeasurement, TSecond> measurement)
                where TSecond : Measurement<TSecond>
                where TSelf : Term<TSelf, DoubleMeasurement, TSecond> {
            Validate.NonNull(measurement, nameof(measurement));
            return measurement.Select((x, y) => y.Multiply(x));
        }

        public static DoubleMeasurement ToDoubleMeasurement(this int i) {
            return new DoubleMeasurement(i);
        }

        public static DoubleMeasurement ToDoubleMeasurement(this double i) {
            return new DoubleMeasurement(i);
        }

        public static DoubleMeasurement ToDoubleMeasurement(this long i) {
            return new DoubleMeasurement(i);
        }

        public static DoubleMeasurement ToDoubleMeasurement(this float i) {
            return new DoubleMeasurement(i);
        }

        public static double ToDouble(this Measurement<DoubleMeasurement> d) {
            Validate.NonNull(d, nameof(d));
            return d.ToDouble(DoubleMeasurement.Units.DefaultUnit);
        }

    }

    public sealed class DoubleMeasurement : Measurement<DoubleMeasurement> { 

        public DoubleMeasurement() {
        }

        public DoubleMeasurement(double d) : base(d, Units.DefaultUnit) {
        }

        private DoubleMeasurement(double amount, Unit<DoubleMeasurement> unit) : base(amount, unit) {
        }

        public static MeasurementProvider<DoubleMeasurement> Provider { get; } = new DoubleMeasurementProvider();

        public override MeasurementProvider<DoubleMeasurement> MeasurementProvider => Provider;

        public new DoubleMeasurement Reciprocal() {
            return new DoubleMeasurement(1 / this.ToDouble());
        }

        public static implicit operator DoubleMeasurement(double d) {
            return new DoubleMeasurement(d);
        }

        public static implicit operator double(DoubleMeasurement d) {
            return d.ToDouble();
        }

        public static class Units {
            public static Unit<DoubleMeasurement> DefaultUnit { get; } = new Unit<DoubleMeasurement>("", Provider);
        }

        private class DoubleMeasurementProvider : MeasurementProvider<DoubleMeasurement> {
            protected override IEnumerable<Unit<DoubleMeasurement>> GetParsableUnits() => new[] { Units.DefaultUnit };

            protected override IEnumerable<Operator> GetOperators() => new Operator[0];

            public override DoubleMeasurement CreateMeasurement(double value, Unit<DoubleMeasurement> unit) => new DoubleMeasurement(value, Units.DefaultUnit);
        }
    }
}