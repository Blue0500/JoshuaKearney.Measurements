﻿namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass>,
        IDividableMeasurement<Volume, Density> {
        public static IMeasurementProvider<Mass> Provider { get; } = new MassProvider();

        public override IMeasurementProvider<Mass> MeasurementProvider => Provider;

        public Mass() {
        }

        public Mass(double amount, Unit<Mass> unit) : base(amount, unit) {
        }

        public static Density operator /(Mass mass, Volume volume) {
            if (mass == null || volume == null) {
                return null;
            }

            return mass.Divide(volume);
        }

        public Density Divide(Volume volume) {
            Validate.NonNull(volume, nameof(volume));
            return new Density(this, volume);
        }

        //public static class Units {
            public static PrefixableUnit<Mass> Gram { get; } = MeasurementSystems.Metric.Gram;

            public static Unit<Mass> Kilogram { get; } = Prefix.Kilo(MeasurementSystems.Metric.Gram);
            public static PrefixableUnit<Mass> MetricTon { get; } = MeasurementSystems.Metric.Tonne;
            public static Unit<Mass> Milligram { get; } = Prefix.Milli(MeasurementSystems.Metric.Gram);

            public static Unit<Mass> Ounce { get; } = MeasurementSystems.AvoirdupoisMass.Ounce;
            public static Unit<Mass> Pound { get; } = MeasurementSystems.AvoirdupoisMass.Pound;
            public static Unit<Mass> ShortTon { get; } = MeasurementSystems.AvoirdupoisMass.ShortTon;
        //}

        private class MassProvider : IMeasurementProvider<Mass> {
            public Unit<Mass> DefaultUnit => Kilogram;

            public Mass CreateMeasurement(double value, Unit<Mass> unit) => new Mass(value, unit);
        }
    }
}