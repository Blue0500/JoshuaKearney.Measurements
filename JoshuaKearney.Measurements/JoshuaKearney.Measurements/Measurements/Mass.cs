using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass>,
        IMultipliableMeasurement<Acceleration, Force>,
        IDividableMeasurement<Volume, Density> {
        public static IMeasurementProvider<Mass> Provider { get; } = new MassProvider();

        public override IMeasurementProvider<Mass> MeasurementProvider => Provider;

        public Mass() {
        }

        public Mass(double amount, IUnit<Mass> unit) : base(amount, unit) {
        }

        public static Density operator /(Mass mass, Volume volume) {
            if (mass == null || volume == null) {
                return null;
            }

            return mass.Divide(volume);
        }

        public static Force operator *(Mass mass, Acceleration accel) {
            if (mass == null || accel == null) {
                return null;
            }

            return mass.Multiply(accel);
        }

        public Density Divide(Volume volume) {
            Validate.NonNull(volume, nameof(volume));
            return new Density(this, volume);
        }

        public Force Multiply(Acceleration accel) {
            return new Force(this, accel);
        }

        public static class Units {
            public static IPrefixableUnit<Mass> Gram { get; } = MeasurementSystems.Metric.Gram;

            public static IUnit<Mass> Kilogram { get; } = Prefix.Kilo(MeasurementSystems.Metric.Gram);
            public static IPrefixableUnit<Mass> MetricTon { get; } = MeasurementSystems.Metric.Tonne;
            public static IUnit<Mass> Milligram { get; } = Prefix.Milli(MeasurementSystems.Metric.Gram);

            public static IUnit<Mass> Ounce { get; } = MeasurementSystems.AvoirdupoisMass.Ounce;
            public static IUnit<Mass> Pound { get; } = MeasurementSystems.AvoirdupoisMass.Pound;
            public static IUnit<Mass> ShortTon { get; } = MeasurementSystems.AvoirdupoisMass.ShortTon;
        }

        private class MassProvider : IMeasurementProvider<Mass> {
            public IUnit<Mass> DefaultUnit => Units.Kilogram;

            public Mass CreateMeasurement(double value, IUnit<Mass> unit) => new Mass(value, unit);
        }
    }
}