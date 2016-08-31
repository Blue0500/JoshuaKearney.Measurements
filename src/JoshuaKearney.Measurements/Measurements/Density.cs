namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static Density ToDensity(this Ratio<Mass, Volume> density) {
            Validate.NonNull(density, nameof(density));

            return new Density(density.ToDouble(density.MeasurementProvider.DefaultUnit), density.MeasurementProvider.DefaultUnit.Cast<Ratio<Mass, Volume>, Density>());
        }
    }

    public sealed class Density : RatioBase<Density, Mass, Volume> {
        public static IMeasurementProvider<Density> Provider { get; } = new DensityProvider();

        public Density() {
        }

        public Density(Mass mass, Volume volume) : base(mass, volume) {
        }

        public Density(double amount, Unit<Density> unit) : base(amount, unit) {
        }

        public Density(double amount, Unit<Mass> massDef, Unit<Volume> volumeDef) : base(amount, massDef, volumeDef) {
        }

        public override IMeasurementProvider<Density> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Volume> DenominatorProvider => Volume.Provider;

        protected override IMeasurementProvider<Mass> NumeratorProvider => Mass.Provider;

        //protected override MeasurementInfo Supplier => new MeasurementInfo(
        //    instanceCreator: x => new Density(x),
        //    defaultUnit: Units.KilogramsPerMeterCubed,
        //    uniqueUnits: new List<IUnit<Density>>()
        //);

        public static class Units {
            public static Unit<Density> GramsPerCentimeterCubed { get; } = Mass.Units.Gram.Divide<Mass, Volume, Density>(Volume.Units.CentimeterCubed);
            public static Unit<Density> KilogramsPerLiter { get; } = Mass.Units.Kilogram.Divide<Mass, Volume, Density>(Volume.Units.Liter);
            public static Unit<Density> KilogramsPerMeterCubed { get; } = Mass.Units.Kilogram.Divide<Mass, Volume, Density>(Volume.Units.MeterCubed);
            public static Unit<Density> MetricTonsPerMeterCubed { get; } = Mass.Units.ShortTon.Divide<Mass, Volume, Density>(Volume.Units.MeterCubed);

            public static Unit<Density> OuncesPerInchCubed { get; } = Mass.Units.Ounce.Divide<Mass, Volume, Density>(Volume.Units.InchCubed);

            public static Unit<Density> PoundsPerFootCubed { get; } = Mass.Units.Pound.Divide<Mass, Volume, Density>(Volume.Units.FootCubed);
        }

        private class DensityProvider : IMeasurementProvider<Density> {
            public Unit<Density> DefaultUnit => Units.KilogramsPerMeterCubed;

            public Density CreateMeasurement(double value, Unit<Density> unit) {
                return new Density(value, unit);
            }
        }
    }
}