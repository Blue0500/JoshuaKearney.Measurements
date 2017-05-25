using System;
using System.Collections.Generic;
using static JoshuaKearney.Measurements.Volume.Units;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public sealed class Volume : Term<Volume, Distance, Area> {

        public Volume(double amount, Unit<Volume> unit)
            : base(amount, unit) {
        }

        public Volume(IMeasurement<Distance> length, IMeasurement<Area> area)
            : base(length, area, Provider) {
        }

        public Volume(IMeasurement<Distance> dist1, IMeasurement<Distance> dist2, IMeasurement<Distance> dist3)
            : this(dist1, dist2.Multiply(dist3)) {
        }

        public Volume(double amount, Unit<Distance> dist1Unit, Unit<Distance> dist2Unit, Unit<Distance> dist3Unit)
            : this(dist1Unit.Multiply(amount), dist2Unit, dist3Unit) { }

        public static MeasurementProvider<Volume> Provider { get; } = new VolumeProvider();

        public override MeasurementProvider<Volume> MeasurementProvider => Provider;
    
        public override MeasurementProvider<Distance> Item1Provider => Distance.Provider;

        public override MeasurementProvider<Area> Item2Provider => Area.Provider;

        public static class Units {
            private static Lazy<Unit<Volume>> centimeterCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Centimeter.Cube().ToUnit("cm^3"));

            private static Lazy<Unit<Volume>> footCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Foot.Cube().ToUnit("ft^3"));

            private static Lazy<Unit<Volume>> inchCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Inch.Cube().ToUnit("in^3"));

            private static Lazy<Unit<Volume>> kilometerCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Kilometer.Cube().ToUnit("km^3"));

            private static Lazy<Unit<Volume>> meterCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Meter.Cube().ToUnit("m^3"));

            private static Lazy<Unit<Volume>> mileCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Mile.Cube().ToUnit("mi^3"));

            private static Lazy<Unit<Volume>> milliliter = new Lazy<Unit<Volume>>(() => Prefix.Milli(Liter));

            private static Lazy<Unit<Volume>> millimeterCubed = new Lazy<Unit<Volume>>(() => Distance.Units.Millimeter.Cube().ToUnit("mm^3"));

            private static Lazy<PrefixableUnit<Volume>> liter = new Lazy<PrefixableUnit<Volume>>(() => Prefix.Deci(Distance.Units.Meter).Cube().ToPrefixableUnit("L"));

            public static PrefixableUnit<Volume> Liter => liter.Value;

            public static Unit<Volume> MillimeterCubed => millimeterCubed.Value;

            public static Unit<Volume> Milliliter => milliliter.Value;

            public static Unit<Volume> MileCubed => mileCubed.Value;

            public static Unit<Volume> MeterCubed => meterCubed.Value;

            public static Unit<Volume> KilometerCubed => kilometerCubed.Value;

            public static Unit<Volume> InchCubed => inchCubed.Value;

            public static Unit<Volume> FootCubed => footCubed.Value;

            public static Unit<Volume> CentimeterCubed => centimeterCubed.Value;

            //public static Unit<Volume> Tablespoon { get; } = MeasurementSystems.CustomaryVolume.Tablespoon;

            //public static Unit<Volume> Teaspoon { get; } = MeasurementSystems.CustomaryVolume.Teaspoon;

            //public static Unit<Volume> USCup { get; } = MeasurementSystems.CustomaryVolume.Cup;

            //public static Unit<Volume> USFluidOunce { get; } = MeasurementSystems.CustomaryVolume.FluidOunce;

            //public static Unit<Volume> USGallon { get; } = MeasurementSystems.CustomaryVolume.Gallon;

            //public static Unit<Volume> USPint { get; } = MeasurementSystems.CustomaryVolume.Pint;

            //public static Unit<Volume> USQuart { get; } = MeasurementSystems.CustomaryVolume.Quart;

            //public static Unit<Volume> YardCubed { get; } = MeasurementSystems.EnglishLength.Yard.Cube().ToUnit("yd^3");
        }

        private class VolumeProvider : CompoundMeasurementProvider<Volume, Distance, Area> {
            public override MeasurementProvider<Area> Component2Provider => Area.Provider;

            public override MeasurementProvider<Distance> Component1Provider => Distance.Provider;

            public override Volume CreateMeasurement(double value, Unit<Volume> unit) => new Volume(value, unit);

            protected override IEnumerable<Unit<Volume>> GetParsableUnits() => new[] { MeterCubed, CentimeterCubed, FootCubed, InchCubed, KilometerCubed, Liter, MileCubed, MillimeterCubed };

            protected override IEnumerable<Operator> GetOperators() => new[] {
                Operator.CreateDivision<Volume, Distance, Area>((x, y) => x.Divide(y)),
                Operator.CreateDivision<Volume, Area, Distance>((x, y) => x.Divide(y))
            };
        }
    }

    public partial class Measurement {
        public static Area Divide(this IMeasurement<Volume> volume, IMeasurement<Distance> distance) {
            Validate.NonNull(volume, nameof(volume));
            Validate.NonNull(distance, nameof(distance));

            return volume.ToMeasurement().Select((x, y) => y.Divide(distance).Multiply(x));
        }

        public static Distance Divide(this IMeasurement<Volume> volume, IMeasurement<Area> area) {
            Validate.NonNull(volume, nameof(volume));
            Validate.NonNull(area, nameof(area));

            return volume.ToMeasurement().Select((x, y) => y.Divide(area).Multiply(x));
        }
    }
}