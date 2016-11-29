using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class DoubleMeasurement : NumericMeasurement<DoubleMeasurement>,
        IDividableMeasurement<Time, Frequency> {

        public DoubleMeasurement() {
        }

        public DoubleMeasurement(double d) : base(d, Units.DefaultUnit) {
        }

        private DoubleMeasurement(double amount, Unit<DoubleMeasurement> unit) : base(amount, unit) {
        }

        public static IMeasurementProvider<DoubleMeasurement> Provider { get; } = new DoubleMeasurementProvider();

        public override IMeasurementProvider<DoubleMeasurement> MeasurementProvider => Provider;

        public static implicit operator DoubleMeasurement(double d) {
            return new DoubleMeasurement(d);
        }

        public static implicit operator double(DoubleMeasurement d) {
            return d.ToDouble();
        }

        public override double ToDouble() {
            return this.ToDouble(Units.DefaultUnit);
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