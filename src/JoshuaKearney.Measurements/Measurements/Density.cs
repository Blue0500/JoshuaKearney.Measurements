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

        public Density(double amount, IUnit<Density> unit) : base(amount, unit) {
        }

        public Density(double amount, IUnit<Mass> massDef, IUnit<Volume> volumeDef) : base(amount, massDef, volumeDef) {
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
            public static IUnit<Density> GramsPerCentimeterCubed { get; } = Mass.Units.Gram.Divide<Mass, Volume, Density>(Volume.Units.CentimeterCubed);
            public static IUnit<Density> KilogramsPerLiter { get; } = Mass.Units.Kilogram.Divide<Mass, Volume, Density>(Volume.Units.Liter);
            public static IUnit<Density> KilogramsPerMeterCubed { get; } = Mass.Units.Kilogram.Divide<Mass, Volume, Density>(Volume.Units.MeterCubed);
            public static IUnit<Density> MetricTonsPerMeterCubed { get; } = Mass.Units.ShortTon.Divide<Mass, Volume, Density>(Volume.Units.MeterCubed);

            public static IUnit<Density> OuncesPerInchCubed { get; } = Mass.Units.Ounce.Divide<Mass, Volume, Density>(Volume.Units.InchCubed);

            public static IUnit<Density> PoundsPerFootCubed { get; } = Mass.Units.Pound.Divide<Mass, Volume, Density>(Volume.Units.FootCubed);
        }

        private class DensityProvider : IMeasurementProvider<Density> {
            public IUnit<Density> DefaultUnit => Units.KilogramsPerMeterCubed;

            public Density CreateMeasurement(double value, IUnit<Density> unit) {
                return new Density(value, unit);
            }
        }
    }
}