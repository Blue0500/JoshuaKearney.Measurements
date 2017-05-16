using System;
using System.Collections.Generic;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public sealed class DoubleMeasurement : Measurement<DoubleMeasurement>, 
        IMultipliableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>,
        IDivisableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>,
        IAddableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>,
        ISubtractableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement> { 

        public DoubleMeasurement() {
        }

        public DoubleMeasurement(double d) : base(d, Units.Unity) {
        }

        public DoubleMeasurement(double amount, Unit<DoubleMeasurement> unit) : base(amount, unit) {
        }

        public static MeasurementProvider<DoubleMeasurement> Provider { get; } = new DoubleMeasurementProvider();

        public override MeasurementProvider<DoubleMeasurement> MeasurementProvider => Provider;

        DoubleMeasurement IAddableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>.Add(IMeasurement<DoubleMeasurement> other) {
            return this + other.ToDouble();
        }

        DoubleMeasurement IDivisableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>.Divide(IMeasurement<DoubleMeasurement> other) {
            return this / other.ToDouble();
        }

        DoubleMeasurement IMultipliableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>.Multiply(IMeasurement<DoubleMeasurement> other) {
            return this * other.ToDouble();
        }

        DoubleMeasurement ISubtractableMeasurement<DoubleMeasurement, DoubleMeasurement, DoubleMeasurement>.Subtract(IMeasurement<DoubleMeasurement> other) {
            return this - other.ToDouble();
        }

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

            public static Unit<DoubleMeasurement> Unity => Provider.DefaultUnit;

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
        public static T Divide<T>(this IMeasurement<T> measurement1, double measurement2)
            where T : IMeasurement<T>, IDivisableMeasurement<T, DoubleMeasurement, T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            IMeasurement<DoubleMeasurement> d = new DoubleMeasurement(measurement2);
            return measurement1.Divide(d);
        }

        public static T Divide<T>(this IMeasurement<T> measurement1, IMeasurement<DoubleMeasurement> measurement2)
            where T : IMeasurement<T>, IDivisableMeasurement<T, DoubleMeasurement, T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            IDivisableMeasurement<T, DoubleMeasurement, T> multi = measurement1.ToMeasurement();
            return multi.Divide(measurement2);
        }

        public static DoubleMeasurement Divide<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
            where T : IMeasurement<T>, IDivisableMeasurement<T, T, DoubleMeasurement> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            IDivisableMeasurement<T, T, DoubleMeasurement> multi = measurement1.ToMeasurement();
            return multi.Divide(measurement2);
        }

        public static T Multiply<T>(this IMeasurement<T> measurement1, double measurement2)
            where T : IMeasurement<T>, IMultipliableMeasurement<T, DoubleMeasurement, T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            IMeasurement<DoubleMeasurement> d = new DoubleMeasurement(measurement2);
            return measurement1.Multiply(d);
        }

        public static T Multiply<T>(this IMeasurement<T> measurement1, IMeasurement<DoubleMeasurement> measurement2)
            where T : IMeasurement<T>, IMultipliableMeasurement<T, DoubleMeasurement, T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            IMultipliableMeasurement<T, DoubleMeasurement, T> multi = measurement1.ToMeasurement();
            return multi.Multiply(measurement2);
        }

        public static T Negate<T>(this IMeasurement<T> measurement1)
            where T : IMeasurement<T>, IMultipliableMeasurement<T, DoubleMeasurement, T> {

            return measurement1.Multiply(-1);
        }

        public static T Abs<T>(this IMeasurement<T> measurement1)
            where T : IMeasurement<T>, IMultipliableMeasurement<T, DoubleMeasurement, T> {

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