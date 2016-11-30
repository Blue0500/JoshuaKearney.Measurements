using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass> {

        public static Lazy<IMeasurementProvider<Mass>> Provider { get; } = new Lazy<IMeasurementProvider<Mass>>(() => new MassProvider());

        public override Lazy<IMeasurementProvider<Mass>> MeasurementProvider => Provider;

        public Mass() {
        }

        public Mass(double amount, Unit<Mass> unit) : base(amount, unit) {
        }

        public static class Units {
            public static PrefixableUnit<Mass> Gram { get; } = new PrefixableUnit<Mass>(
                "g", .001, Provider
            );

            public static Unit<Mass> Kilogram { get; } = Prefix.Kilo(Gram);

            public static Unit<Mass> MetricTon { get; } = Kilogram.Multiply(1000).ToUnit("t");

            public static Unit<Mass> Milligram { get; } = Prefix.Milli(Gram);

            //public static Unit<Mass> Ounce { get; } = Kilogram

            //public static Unit<Mass> Pound { get; } = MeasurementSystems.AvoirdupoisMass.Pound;

            //public static Unit<Mass> ShortTon { get; } = MeasurementSystems.AvoirdupoisMass.ShortTon;
        }

        private class MassProvider : IMeasurementProvider<Mass> {
            public IEnumerable<Unit<Mass>> AllUnits { get; } = new[] { Units.Gram, Units.MetricTon };

            public Unit<Mass> DefaultUnit => Units.Kilogram;

            public Mass CreateMeasurement(double value, Unit<Mass> unit) => new Mass(value, unit);
        }
    }
}