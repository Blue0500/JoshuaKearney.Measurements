using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public partial class Extensions {

        public static Volume ToVolume(this Term<Area, Length> term) {
            Validate.NonNull(term, nameof(term));

            return Volume.From(
                term.ToDouble(Term<Area, Length>.GetDefaultUnitDefinition()),
                Volume.GetDefaultUnitDefinition()
            );
        }

        public static Volume ToVolume(this Term<Length, Area> term) {
            Validate.NonNull(term, nameof(term));

            return Volume.From(
                term.ToDouble(Term<Length, Area>.GetDefaultUnitDefinition()),
                Volume.GetDefaultUnitDefinition()
            );
        }
    }

    public sealed class Volume : TermBase<Volume, Length, Area>,
            IDividableMeasurement<Length, Area> {

        private static MeasurementInfo propertySupplier = new MeasurementInfo(
            instanceCreator: x => new Volume(x),
            defaultUnit: CommonUnits.MeterCubed,
            uniqueUnits: MeasurementSystems.CustomaryVolume.AllUnits
                .Concat(MeasurementSystems.ImperialVolume.AllUnits)
                .Concat(new[] { MeasurementSystems.Metric.Liter })
        );

        public Volume() {
        }

        private Volume(double metersCubed) : base(metersCubed) {
        }

        protected override MeasurementInfo Supplier => propertySupplier;

        public static Area operator /(Volume volume, Length length) {
            if (volume == null || length == null) {
                return null;
            }

            return volume.Divide(length);
        }

        public Area Divide(Length length) {
            Validate.NonNull(length, nameof(length));

            return this.DivideToSecond(length);
        }

        public static partial class CommonUnits {
            public static IUnit<Volume> CentimeterCubed { get; } = Length.CommonUnits.Centimeter.Cube<Length, Volume>();
            public static IUnit<Volume> FootCubed { get; } = MeasurementSystems.EnglishLength.Foot.Cube<Length, Volume>();
            public static IUnit<Volume> InchCubed { get; } = MeasurementSystems.EnglishLength.Inch.Cube<Length, Volume>();
            public static IUnit<Volume> KilometerCubed { get; } = Length.CommonUnits.Kilometer.Cube<Length, Volume>();
            public static IPrefixableUnit<Volume> Liter { get; } = MeasurementSystems.Metric.Liter;
            public static IUnit<Volume> MeterCubed { get; } = MeasurementSystems.Metric.Meter.Cube<Length, Volume>();
            public static IUnit<Volume> MileCubed { get; } = Length.CommonUnits.Mile.Cube<Length, Volume>();
            public static IUnit<Volume> Milliliter { get; } = Prefix.Milli(Liter);
            public static IUnit<Volume> MillimeterCubed { get; } = Length.CommonUnits.Millimeter.Cube<Length, Volume>();
            public static IUnit<Volume> Tablespoon { get; } = MeasurementSystems.CustomaryVolume.Tablespoon;
            public static IUnit<Volume> Teaspoon { get; } = MeasurementSystems.CustomaryVolume.Teaspoon;
            public static IUnit<Volume> USCup { get; } = MeasurementSystems.CustomaryVolume.Cup;
            public static IUnit<Volume> USFluidOunce { get; } = MeasurementSystems.CustomaryVolume.FluidOunce;
            public static IUnit<Volume> USGallon { get; } = MeasurementSystems.CustomaryVolume.Gallon;
            public static IUnit<Volume> USPint { get; } = MeasurementSystems.CustomaryVolume.Pint;
            public static IUnit<Volume> USQuart { get; } = MeasurementSystems.CustomaryVolume.Quart;
            public static IUnit<Volume> YardCubed { get; } = MeasurementSystems.EnglishLength.Yard.Cube<Length, Volume>();
        }
    }
}