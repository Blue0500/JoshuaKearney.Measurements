using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static double Simplify<TSelf>(this Ratio<TSelf, DoubleMeasurement, DoubleMeasurement> measurement)
                where TSelf : Ratio<TSelf, DoubleMeasurement, DoubleMeasurement> {
            return measurement.Simplify((x, y) => x.Divide(y));
        }

        public static TNum Simplify<TSelf, TNum>(this Ratio<TSelf, TNum, DoubleMeasurement> measurement)
                where TSelf : Ratio<TSelf, TNum, DoubleMeasurement>
                where TNum : Measurement<TNum> {
            return measurement.Simplify((x, y) => x.Divide(y));
        }

        public static TFirst Simplify<TSelf, TFirst>(this Term<TSelf, TFirst, DoubleMeasurement> measurement)
                where TFirst : Measurement<TFirst>
                where TSelf : Term<TSelf, TFirst, DoubleMeasurement> {
            return measurement.Simplify((x, y) => x.Multiply(y));
        }

        public static TSecond Simplify<TSelf, TSecond>(this Term<TSelf, DoubleMeasurement, TSecond> measurement)
                where TSecond : Measurement<TSecond>
                where TSelf : Term<TSelf, DoubleMeasurement, TSecond> {
            return measurement.Simplify((x, y) => y.Multiply(x));
        }
    }

    public sealed class DoubleMeasurement : Measurement<DoubleMeasurement>,
        IDividableMeasurement<Time, Frequency> {
        public static IMeasurementProvider<DoubleMeasurement> Provider { get; } = new DoubleMeasurementProvider();

        public override IMeasurementProvider<DoubleMeasurement> MeasurementProvider => Provider;

        public DoubleMeasurement() {
        }

        public DoubleMeasurement(double d) : base(d, Units.DefaultUnit) {
        }

        private DoubleMeasurement(double amount, Unit<DoubleMeasurement> unit) : base(amount, unit) {
        }

        public double ToDouble() => this.ToDouble(Units.DefaultUnit);

        public Frequency Divide(Time measurement2) {
            return new Frequency(this, measurement2);
        }

        public static implicit operator double(DoubleMeasurement measurement) {
            return measurement?.ToDouble() ?? 0;
        }

        public static implicit operator DoubleMeasurement(double d) {
            return new DoubleMeasurement(d);
        }

        private static class Units {
            public static Unit<DoubleMeasurement> DefaultUnit { get; } = new Unit<DoubleMeasurement>("", "", 1);
        }

        private class DoubleMeasurementProvider : IMeasurementProvider<DoubleMeasurement> {
            public IEnumerable<Unit<DoubleMeasurement>> AllUnits => new[] { Units.DefaultUnit };

            public Unit<DoubleMeasurement> DefaultUnit => Units.DefaultUnit;

            public DoubleMeasurement CreateMeasurement(double value, Unit<DoubleMeasurement> unit) {
                return new DoubleMeasurement(value, unit);
            }
        }
    }
}