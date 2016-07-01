using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static Density ToDensity(this Ratio<Mass, Volume> density) {
            Validate.NonNull(density, nameof(density));

            return Density.From(
                density.ToDouble(Ratio<Mass, Volume>.GetDefaultUnitDefinition()),
                Density.GetDefaultUnitDefinition()
            );
        }
    }

    public sealed class Density : RatioBase<Density, Mass, Volume> {

        public Density() {
        }

        private Density(double kilogramsPerMeterCubed) : base(kilogramsPerMeterCubed) {
        }

        protected override MeasurementInfo Supplier => new MeasurementInfo(
            instanceCreator: x => new Density(x),
            defaultUnit: CommonUnits.KilogramsPerMeterCubed,
            uniqueUnits: new List<IUnit<Density>>()
        );

        public static class CommonUnits {
            public static IUnit<Density> GramsPerCentimeterCubed { get; } = Mass.CommonUnits.Gram.DivideToRatio(Volume.CommonUnits.CentimeterCubed).Cast<Density>();
            public static IUnit<Density> KilogramsPerLiter { get; } = Mass.CommonUnits.Kilogram.DivideToRatio(Volume.CommonUnits.Liter).Cast<Density>();
            public static IUnit<Density> KilogramsPerMeterCubed { get; } = Mass.CommonUnits.Kilogram.DivideToRatio(Volume.CommonUnits.MeterCubed).Cast<Density>();
            public static IUnit<Density> MetricTonsPerMeterCubed { get; } = Mass.CommonUnits.ShortTon.DivideToRatio(Volume.CommonUnits.MeterCubed).Cast<Density>();

            public static IUnit<Density> OuncesPerInchCubed { get; } = Mass.CommonUnits.Ounce.DivideToRatio(Volume.CommonUnits.InchCubed).Cast<Density>();

            public static IUnit<Density> PoundsPerFootCubed { get; } = Mass.CommonUnits.Pound.DivideToRatio(Volume.CommonUnits.FootCubed).Cast<Density>();
        }
    }
}