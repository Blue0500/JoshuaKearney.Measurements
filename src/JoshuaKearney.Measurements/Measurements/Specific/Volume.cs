using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public partial class MeasurementExtensions {
        public static Area Divide(this Measurement<Volume> volume, Distance distance) {
            Validate.NonNull(volume, nameof(volume));
            Validate.NonNull(distance, nameof(distance));

            return ((Volume)volume).Select((x, y) => y.Divide(distance).Multiply(x));
        }
    }

    public sealed class Volume : Term<Volume, Distance, Area>,
        IDividableMeasurement<Distance, Area> {

        public Volume() {
        }

        public Volume(double amount, Unit<Volume> unit)
            : base(amount, unit) {
        }

        public Volume(Distance length, Area area)
            : base(length, area, Provider) {
        }

        public Volume(Distance dist1, Distance dist2, Distance dist3)
            : this(dist1, dist2.Multiply(dist3)) {
        }

        public Volume(double amount, Unit<Distance> lengthDef, Unit<Area> areaDef)
            : base(amount, lengthDef, areaDef, Provider) {
        }

        public Volume(double amount, Unit<Distance> dist1Unit, Unit<Distance> dist2Unit, Unit<Distance> dist3Unit)
            : this(dist1Unit.Multiply(amount), dist2Unit, dist3Unit) { }

        public static Lazy<IMeasurementProvider<Volume>> Provider { get; } = new Lazy<IMeasurementProvider<Volume>>(() => new VolumeProvider());

        public override Lazy<IMeasurementProvider<Volume>> MeasurementProvider => Provider;

        public override Lazy<IMeasurementProvider<Distance>> Item1Provider => Distance.Provider;

        public override Lazy<IMeasurementProvider<Area>> Item2Provider => Area.Provider;

        Area IDividableMeasurement<Distance, Area>.Divide(Distance measurement2) => this.Divide(measurement2);

        public static class Units {
            public static Unit<Volume> CentimeterCubed { get; } = Distance.Units.Centimeter.Cube().ToUnit("cm^3");

            public static Unit<Volume> FootCubed { get; } = Distance.Units.Foot.Cube().ToUnit("ft^3");

            public static Unit<Volume> InchCubed { get; } = Distance.Units.Inch.Cube().ToUnit("in^3");

            public static Unit<Volume> KilometerCubed { get; } = Distance.Units.Kilometer.Cube().ToUnit("km^3");

            public static PrefixableUnit<Volume> Liter { get; } = new PrefixableUnit<Volume>(
                symbol: "L",
                defaultsPerUnit: 1 / 1000,
                provider: Provider
            );

            public static Unit<Volume> MeterCubed { get; } = Distance.Units.Meter.Cube().ToUnit("m^3");

            public static Unit<Volume> MileCubed { get; } = Distance.Units.Mile.Cube().ToUnit("mi^3");

            public static Unit<Volume> Milliliter { get; } = Prefix.Milli(Liter);

            public static Unit<Volume> MillimeterCubed { get; } = Distance.Units.Millimeter.Cube().ToUnit("mm^3");

            //public static Unit<Volume> Tablespoon { get; } = MeasurementSystems.CustomaryVolume.Tablespoon;

            //public static Unit<Volume> Teaspoon { get; } = MeasurementSystems.CustomaryVolume.Teaspoon;

            //public static Unit<Volume> USCup { get; } = MeasurementSystems.CustomaryVolume.Cup;

            //public static Unit<Volume> USFluidOunce { get; } = MeasurementSystems.CustomaryVolume.FluidOunce;

            //public static Unit<Volume> USGallon { get; } = MeasurementSystems.CustomaryVolume.Gallon;

            //public static Unit<Volume> USPint { get; } = MeasurementSystems.CustomaryVolume.Pint;

            //public static Unit<Volume> USQuart { get; } = MeasurementSystems.CustomaryVolume.Quart;

            //public static Unit<Volume> YardCubed { get; } = MeasurementSystems.EnglishLength.Yard.Cube().ToUnit("yd^3");
        }

        private class VolumeProvider : IMeasurementProvider<Volume>, IComplexMeasurementProvider<Area, Distance> {
            public IEnumerable<Unit<Volume>> AllUnits { get; } = new[] { Units.Liter };

            public Lazy<IMeasurementProvider<Area>> Component1Provider => Area.Provider;

            public Lazy<IMeasurementProvider<Distance>> Component2Provider => Distance.Provider;

            public Unit<Volume> DefaultUnit => Units.MeterCubed;

            public Volume CreateMeasurement(double value, Unit<Volume> unit) => new Volume(value, unit);
        }
    }
}