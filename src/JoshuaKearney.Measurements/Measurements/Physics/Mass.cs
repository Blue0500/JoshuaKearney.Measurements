using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass>,
        IMultipliableMeasurement<Acceleration, Force>,
        IDividableMeasurement<Volume, Density>,
        IDividableMeasurement<ChemicalAmount, MolarMass> {

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

        public static class Units {
            public static PrefixableUnit<Mass> Gram { get; } = MeasurementSystems.Metric.Gram;

            public static Unit<Mass> Kilogram { get; } = Prefix.Kilo(MeasurementSystems.Metric.Gram);

            public static Unit<Mass> MetricTon { get; } = MeasurementSystems.Metric.Tonne;

            public static Unit<Mass> Milligram { get; } = Prefix.Milli(MeasurementSystems.Metric.Gram);

            public static Unit<Mass> Ounce { get; } = MeasurementSystems.AvoirdupoisMass.Ounce;

            public static Unit<Mass> Pound { get; } = MeasurementSystems.AvoirdupoisMass.Pound;

            public static Unit<Mass> ShortTon { get; } = MeasurementSystems.AvoirdupoisMass.ShortTon;
        }

        public Force Multiply(Acceleration measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return new Force(this, measurement2);
        }

        public MolarMass Divide(ChemicalAmount measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return new MolarMass(this, measurement2);
        }

        //public Volume Divide(Density measurement2) {
        //    Validate.NonNull(measurement2, nameof(measurement2));

        //    return new Volume(this.DefaultUnits / measurement2.DefaultUnits, Volume.Units.MeterCubed);
        //}

        public static Force operator *(Mass first, Acceleration measurement2) {
            if (first == null || measurement2 == null) {
                return null;
            }
            else {
                return first.Multiply(measurement2);
            }
        }

        private class MassProvider : IMeasurementProvider<Mass> {
            public IEnumerable<Unit<Mass>> AllUnits { get; } = new[] { Units.Gram, Units.MetricTon, Units.Ounce, Units.Pound, Units.ShortTon };

            public Unit<Mass> DefaultUnit => Units.Kilogram;

            public Mass CreateMeasurement(double value, Unit<Mass> unit) => new Mass(value, unit);
        }
    }
}