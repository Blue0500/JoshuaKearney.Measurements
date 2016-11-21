using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public partial class MeasurementExtensions {

        public static Volume ToVolume(this Term<Area, Distance> term) {
            Validate.NonNull(term, nameof(term));

            return new Volume(term.ToDouble(term.MeasurementProvider.DefaultUnit), term.MeasurementProvider.DefaultUnit.Cast<Term<Area, Distance>, Volume>());
        }

        public static Volume ToVolume(this Term<Distance, Area> term) {
            Validate.NonNull(term, nameof(term));

            return new Volume(term.ToDouble(term.MeasurementProvider.DefaultUnit), term.MeasurementProvider.DefaultUnit.Cast<Term<Distance, Area>, Volume>());
        }
    }

    public sealed class Volume : Term<Volume, Distance, Area>,
        IMultipliableMeasurement<Pressure, Energy>,
        IDividableMeasurement<Distance, Area> {

        public Volume() {
        }

        public Volume(double amount, Unit<Volume> unit)
            : base(amount, unit) {
        }

        public Volume(Distance length, Area area)
            : base(length, area) {
        }

        public Volume(Distance dist1, Distance dist2, Distance dist3)
            : this(dist1, dist2.Multiply(dist3)) {
        }

        public Volume(double amount, Unit<Distance> lengthDef, Unit<Area> areaDef)
            : base(amount, lengthDef, areaDef) {
        }

        public Volume(double amount, Unit<Distance> dist1Unit, Unit<Distance> dist2Unit, Unit<Distance> dist3Unit)
            : this(amount, dist1Unit, dist2Unit.Multiply<Distance, Distance, Area>(dist3Unit)) { }

        public static IMeasurementProvider<Volume> Provider { get; } = new VolumeProvider();

        public override IMeasurementProvider<Volume> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Distance> Item1Provider => Distance.Provider;

        protected override IMeasurementProvider<Area> Item2Provider => Area.Provider;

        public static Area operator /(Volume volume, Distance length) {
            if (volume == null || length == null) {
                return null;
            }

            return volume.Divide(length);
        }

        public Area Divide(Distance length) {
            Validate.NonNull(length, nameof(length));

            return this.DivideToSecond(length);
        }

        public Energy Multiply(Pressure measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return measurement2.Multiply(this);
        }

        public string ToString(Unit<Distance> unit1, Unit<Distance> unit2, Unit<Distance> unit3) {
            return this.ToString(unit1, unit2.Multiply<Distance, Distance, Area>(unit3));
        }

        public double ToDouble(Unit<Distance> unit1, Unit<Distance> unit2, Unit<Distance> unit3) {
            return this.ToDouble(unit1, unit2.Multiply<Distance, Distance, Area>(unit3));
        }

        public static class Units {
            public static Unit<Volume> CentimeterCubed { get; } = Distance.Units.Centimeter.Cube<Distance, Volume>();

            public static Unit<Volume> FootCubed { get; } = MeasurementSystems.EnglishLength.Foot.Cube<Distance, Volume>();

            public static Unit<Volume> InchCubed { get; } = MeasurementSystems.EnglishLength.Inch.Cube<Distance, Volume>();

            public static Unit<Volume> KilometerCubed { get; } = Distance.Units.Kilometer.Cube<Distance, Volume>();

            public static PrefixableUnit<Volume> Liter { get; } = MeasurementSystems.Metric.Liter;

            public static Unit<Volume> MeterCubed { get; } = MeasurementSystems.Metric.Meter.Cube<Distance, Volume>();

            public static Unit<Volume> MileCubed { get; } = Distance.Units.Mile.Cube<Distance, Volume>();

            public static Unit<Volume> Milliliter { get; } = Prefix.Milli(MeasurementSystems.Metric.Liter);

            public static Unit<Volume> MillimeterCubed { get; } = Distance.Units.Millimeter.Cube<Distance, Volume>();

            public static Unit<Volume> Tablespoon { get; } = MeasurementSystems.CustomaryVolume.Tablespoon;

            public static Unit<Volume> Teaspoon { get; } = MeasurementSystems.CustomaryVolume.Teaspoon;

            public static Unit<Volume> USCup { get; } = MeasurementSystems.CustomaryVolume.Cup;

            public static Unit<Volume> USFluidOunce { get; } = MeasurementSystems.CustomaryVolume.FluidOunce;

            public static Unit<Volume> USGallon { get; } = MeasurementSystems.CustomaryVolume.Gallon;

            public static Unit<Volume> USPint { get; } = MeasurementSystems.CustomaryVolume.Pint;

            public static Unit<Volume> USQuart { get; } = MeasurementSystems.CustomaryVolume.Quart;

            public static Unit<Volume> YardCubed { get; } = MeasurementSystems.EnglishLength.Yard.Cube<Distance, Volume>();
        }

        private class VolumeProvider : IMeasurementProvider<Volume>, IComplexMeasurementProvider<Area, Distance> {
            public IEnumerable<Unit<Volume>> AllUnits { get; } = new[] { Units.Liter };

            public IMeasurementProvider<Area> Component1Provider => Area.Provider;

            public IMeasurementProvider<Distance> Component2Provider => Distance.Provider;

            public Unit<Volume> DefaultUnit => Units.MeterCubed;

            public Volume CreateMeasurement(double value, Unit<Volume> unit) => new Volume(value, unit);
        }
    }
}