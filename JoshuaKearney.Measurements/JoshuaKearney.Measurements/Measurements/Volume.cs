using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public partial class Extensions {

        public static Volume ToVolume(this Term<Area, Length> term) {
            Validate.NonNull(term, nameof(term));

            return new Volume(term.ToDouble(term.MeasurementProvider.DefaultUnit), term.MeasurementProvider.DefaultUnit.Cast<Term<Area, Length>, Volume>());
        }

        public static Volume ToVolume(this Term<Length, Area> term) {
            Validate.NonNull(term, nameof(term));

            return new Volume(term.ToDouble(term.MeasurementProvider.DefaultUnit), term.MeasurementProvider.DefaultUnit.Cast<Term<Length, Area>, Volume>());
        }
    }

    public sealed class Volume : TermBase<Volume, Length, Area>,
            IDividableMeasurement<Length, Area> {
        public static IMeasurementProvider<Volume> Provider { get; } = new VolumeProvider();

        protected override IMeasurementProvider<Length> Item1Provider => Length.Provider;

        protected override IMeasurementProvider<Area> Item2Provider => Area.Provider;

        public override IMeasurementProvider<Volume> MeasurementProvider => Provider;

        public Volume() {
        }

        public Volume(double amount, IUnit<Volume> unit) : base(amount, unit) {
        }

        public Volume(Length length, Area area) : base(length, area) {
        }

        public Volume(double amount, IUnit<Length> lengthDef, IUnit<Area> areaDef) : base(amount, lengthDef, areaDef) {
        }

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

        public static partial class Units {
            public static IUnit<Volume> CentimeterCubed { get; } = Length.Units.Centimeter.Cube<Length, Volume>();
            public static IUnit<Volume> FootCubed { get; } = MeasurementSystems.EnglishLength.Foot.Cube<Length, Volume>();
            public static IUnit<Volume> InchCubed { get; } = MeasurementSystems.EnglishLength.Inch.Cube<Length, Volume>();
            public static IUnit<Volume> KilometerCubed { get; } = Length.Units.Kilometer.Cube<Length, Volume>();
            public static IPrefixableUnit<Volume> Liter { get; } = MeasurementSystems.Metric.Liter;
            public static IUnit<Volume> MeterCubed { get; } = MeasurementSystems.Metric.Meter.Cube<Length, Volume>();
            public static IUnit<Volume> MileCubed { get; } = Length.Units.Mile.Cube<Length, Volume>();
            public static IUnit<Volume> Milliliter { get; } = Prefix.Milli(MeasurementSystems.Metric.Liter);
            public static IUnit<Volume> MillimeterCubed { get; } = Length.Units.Millimeter.Cube<Length, Volume>();
            public static IUnit<Volume> Tablespoon { get; } = MeasurementSystems.CustomaryVolume.Tablespoon;
            public static IUnit<Volume> Teaspoon { get; } = MeasurementSystems.CustomaryVolume.Teaspoon;
            public static IUnit<Volume> USCup { get; } = MeasurementSystems.CustomaryVolume.Cup;
            public static IUnit<Volume> USFluidOunce { get; } = MeasurementSystems.CustomaryVolume.FluidOunce;
            public static IUnit<Volume> USGallon { get; } = MeasurementSystems.CustomaryVolume.Gallon;
            public static IUnit<Volume> USPint { get; } = MeasurementSystems.CustomaryVolume.Pint;
            public static IUnit<Volume> USQuart { get; } = MeasurementSystems.CustomaryVolume.Quart;
            public static IUnit<Volume> YardCubed { get; } = MeasurementSystems.EnglishLength.Yard.Cube<Length, Volume>();
        }

        private class VolumeProvider : IMeasurementProvider<Volume> {
            public IUnit<Volume> DefaultUnit => Units.MeterCubed;

            public Volume CreateMeasurement(double value, IUnit<Volume> unit) => new Volume(value, unit);
        }
    }
}