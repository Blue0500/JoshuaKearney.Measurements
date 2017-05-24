using System;
using System.Collections.Generic;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public sealed class DoubleMeasurement : Measurement<DoubleMeasurement> { 

        public DoubleMeasurement() {
        }

        public DoubleMeasurement(double d) : base(d, Units.Unity) {
        }

        public DoubleMeasurement(double amount, Unit<DoubleMeasurement> unit) : base(amount, unit) {
        }

        public static MeasurementProvider<DoubleMeasurement> Provider { get; } = new DoubleMeasurementProvider();

        public override MeasurementProvider<DoubleMeasurement> MeasurementProvider => Provider;

        public static implicit operator DoubleMeasurement(double d) {
            return new DoubleMeasurement(d);
        }

        public static implicit operator double(DoubleMeasurement d) {
            return d.ToDouble();
        }

        public static class Units {
            private static readonly Lazy<Unit<DoubleMeasurement>> percent = new Lazy<Unit<DoubleMeasurement>>(() => Unity.Divide(100).ToUnit("%"));

            private static readonly Lazy<Unit<DoubleMeasurement>> permil = new Lazy<Unit<DoubleMeasurement>>(() => Unity.Divide(1000).ToUnit("‰"));

            private static readonly Lazy<Unit<DoubleMeasurement>> ppm = new Lazy<Unit<DoubleMeasurement>>(() => Unity.Divide(1e6).ToUnit("ppm"));

            private static readonly Lazy<Unit<DoubleMeasurement>> ppb = new Lazy<Unit<DoubleMeasurement>>(() => Unity.Divide(1e9).ToUnit("ppb"));

            private static readonly Lazy<Unit<DoubleMeasurement>> unity = new Lazy<Unit<DoubleMeasurement>>(() => new Unit<DoubleMeasurement>("unit", Provider));

            public static Unit<DoubleMeasurement> Unity => unity.Value;

            public static Unit<DoubleMeasurement> Percent => percent.Value;

            public static Unit<DoubleMeasurement> Permil => permil.Value;

            public static Unit<DoubleMeasurement> PartPerMillion => ppm.Value;

            public static Unit<DoubleMeasurement> PartPerBillion => ppb.Value;
        }

        private class DoubleMeasurementProvider : MeasurementProvider<DoubleMeasurement> {
            public override IEnumerable<Unit<DoubleMeasurement>> ParsableUnits => new[] { Units.Unity, Units.Percent, Units.Permil, Units.PartPerMillion, Units.PartPerBillion };

            public override IEnumerable<Operator> ParseOperators => new[] {
                Operator.CreateExponation<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>((x, y) => Math.Pow(x, y))
            };

            public override DoubleMeasurement CreateMeasurement(double value, Unit<DoubleMeasurement> unit) => new DoubleMeasurement(value, Units.Unity);
        }
    }

    public static partial class MeasurementExtensions {

        public static T Divide<T>(this IMeasurement<T> measurement1, DoubleMeasurement measurement2)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            double val = measurement1.ToDouble(measurement1.MeasurementProvider.DefaultUnit);
            return measurement1.MeasurementProvider.CreateMeasurement(val / measurement2, measurement1.MeasurementProvider.DefaultUnit);
        }

        public static DoubleMeasurement Divide<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return measurement1.ToDouble(measurement1.MeasurementProvider.DefaultUnit) / measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit);
        }

        public static T Multiply<T>(this IMeasurement<T> measurement1, DoubleMeasurement measurement2)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            double val = measurement1.ToDouble(measurement1.MeasurementProvider.DefaultUnit);
            return measurement1.MeasurementProvider.CreateMeasurement(val * measurement2, measurement1.MeasurementProvider.DefaultUnit);
        }

        public static T Multiply<T>(this IMeasurement<DoubleMeasurement> measurement1, IMeasurement<T> measurement2)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return measurement2.Multiply(measurement1.ToMeasurement());
        }

        public static T Negate<T>(this IMeasurement<T> measurement1)
            where T : IMeasurement<T> {

            return measurement1.Multiply(-1);
        }

        public static T Abs<T>(this IMeasurement<T> measurement1)
            where T : IMeasurement<T> {

            if (measurement1.CompareTo(measurement1.MeasurementProvider.Zero) < 0) {
                return measurement1.Negate();
            }
            else {
                return measurement1.ToMeasurement();
            }
        }

        public static DoubleMeasurement Reciprocal(this IMeasurement<DoubleMeasurement> measurement) {
            Validate.NonNull(measurement, nameof(measurement));
            return new DoubleMeasurement(1d / measurement.ToDouble());
        }

        public static double ToDouble(this IMeasurement<DoubleMeasurement> measurement) {
            Validate.NonNull(measurement, nameof(measurement));
            return measurement.ToDouble(DoubleMeasurement.Units.Unity);
        }
    }
}