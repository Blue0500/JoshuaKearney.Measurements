using System;
using System.Collections.Generic;
using static JoshuaKearney.Measurements.Volume.Units;

namespace JoshuaKearney.Measurements {
    public sealed class Volume : Term<Volume, Distance, Area>, IMeasurement<Volume>,
        IDividableMeasurement<Distance, Area> {

        public Volume() {
        }

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

        public static MeasurementSupplier<Volume> Provider { get; } = new MeasurementSupplier<Volume>((value, unit) => new Volume(value, unit));

        public override MeasurementSupplier<Volume> MeasurementSupplier => Provider;
    
        public override MeasurementSupplier<Distance> Item1Provider => Distance.Provider;

        public override MeasurementSupplier<Area> Item2Provider => Area.Provider;

        Area IDividableMeasurement<Distance, Area>.Divide(Distance measurement2) => this.Divide(measurement2);

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
        }
    }

    public partial class MeasurementExtensions {
        public static Area Divide(this IMeasurement<Volume> volume, IMeasurement<Distance> distance) {
            Validate.NonNull(volume, nameof(volume));
            Validate.NonNull(distance, nameof(distance));

            return ((Volume)volume).Select((x, y) => y.Divide(distance).Multiply(x));
        }
    }
} 