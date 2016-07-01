using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass>,
        IDividableMeasurement<Volume, Density> {

        private static MeasurementInfo propertySupplier = new MeasurementInfo(
            instanceSupplier: x => new Mass(x),
            storedUnit: CommonUnits.Kilogram,
            uniqueUnits: new Lazy<IEnumerable<IUnit>>(() => {
                return MeasurementSystems.AvoirdupoisMass.AllUnits
                    .Concat(new[] { MeasurementSystems.Metric.Tonne, MeasurementSystems.Metric.Gram });
            })
        );

        public Mass() {
        }

        private Mass(double kilograms) : base(kilograms) {
        }

        protected override MeasurementInfo Supplier => propertySupplier;

        public static Density operator /(Mass mass, Volume volume) {
            if (mass == null || volume == null) {
                return null;
            }

            return mass.Divide(volume);
        }

        public Density Divide(Volume volume) {
            Validate.NonNull(volume, nameof(volume));
            return Density.From(this, volume);
        }

        public static class CommonUnits {
            public static IPrefixableUnit<Mass> Gram { get; } = MeasurementSystems.Metric.Gram;

            public static IUnit<Mass> Kilogram { get; } = Prefix.Kilo(Gram);
            public static IPrefixableUnit<Mass> MetricTon { get; } = MeasurementSystems.Metric.Tonne;
            public static IUnit<Mass> Milligram { get; } = Prefix.Milli(Gram);

            public static IUnit<Mass> Ounce { get; } = MeasurementSystems.AvoirdupoisMass.Ounce;
            public static IUnit<Mass> Pound { get; } = MeasurementSystems.AvoirdupoisMass.Pound;
            public static IUnit<Mass> ShortTon { get; } = MeasurementSystems.AvoirdupoisMass.ShortTon;
        }
    }
}