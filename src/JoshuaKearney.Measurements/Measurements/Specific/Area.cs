using System;
using System.Collections.Generic;
using static JoshuaKearney.Measurements.Area.Units;

namespace JoshuaKearney.Measurements {

    public sealed class Area : Term<Area, Distance, Distance>,
        IDividableMeasurement<Distance, Distance>,
        IMultipliableMeasurement<Distance, Volume> {

        public Area() {
        }

        public Area(double amount, Unit<Area> unit) : base(amount, unit) {
        }

        public Area(Distance length1, Distance length2) : base(length1, length2, Provider) {
        }

        public static MeasurementProvider<Area> Provider { get; } = new AreaProvider();

        public override MeasurementProvider<Area> MeasurementProvider => Provider;

        public override MeasurementProvider<Distance> Item1Provider => Distance.Provider;

        public override MeasurementProvider<Distance> Item2Provider => Distance.Provider;

        Volume IMultipliableMeasurement<Distance, Volume>.Multiply(Distance measurement2) => this.Multiply(measurement2);

        public static class Units {
            private static Lazy<Unit<Area>> acre = new Lazy<Unit<Area>>(() => MeterSquared.Multiply(4046.8564224).ToUnit("acre"));

            private static Lazy<Unit<Area>> centimeterSquared = new Lazy<Unit<Area>>(() => Distance.Units.Centimeter.Square().ToUnit("cm^2"));

            private static Lazy<Unit<Area>> footSquared = new Lazy<Unit<Area>>(() => Distance.Units.Foot.Square().ToUnit("ft^2"));

            private static Lazy<Unit<Area>> hectare = new Lazy<Unit<Area>>(() => MeterSquared.Multiply(100).ToUnit("ha"));

            private static Lazy<Unit<Area>> inchSquared = new Lazy<Unit<Area>>(() => Distance.Units.Inch.Square().ToUnit("in^2"));

            private static Lazy<Unit<Area>> kilometerSquared = new Lazy<Unit<Area>>(() => Distance.Units.Kilometer.Square().ToUnit("km^2"));

            private static Lazy<Unit<Area>> meterSquared = new Lazy<Unit<Area>>(() => Distance.Units.Meter.Square().ToUnit("m^2"));

            private static Lazy<Unit<Area>> mileSquared = new Lazy<Unit<Area>>(() => Distance.Units.Mile.Square().ToUnit("mi^2"));

            private static Lazy<Unit<Area>> millimeterSquared = new Lazy<Unit<Area>>(() => Distance.Units.Millimeter.Square().ToUnit("mm^2"));

            private static Lazy<Unit<Area>> yardSquared = new Lazy<Unit<Area>>(() => Distance.Units.Yard.Square().ToUnit("yd^2"));

            public static Unit<Area> Acre => acre.Value;

            public static Unit<Area> CentimeterSquared => centimeterSquared.Value;
            public static Unit<Area> FootSquared => footSquared.Value;
            public static Unit<Area> Hectare => hectare.Value;
            public static Unit<Area> InchSquared => inchSquared.Value;
            public static Unit<Area> KilometerSquared => kilometerSquared.Value;
            public static Unit<Area> MeterSquared => meterSquared.Value;
            public static Unit<Area> MileSquared => mileSquared.Value;
            public static Unit<Area> MillimeterSquared => millimeterSquared.Value;
            public static Unit<Area> YardSquared => yardSquared.Value;


        }

        private class AreaProvider : CompoundMeasurementProvider<Area, Distance, Distance> {
            public override MeasurementProvider<Distance> Component1Provider => Distance.Provider;

            public override MeasurementProvider<Distance> Component2Provider => Distance.Provider;

            public override Area CreateMeasurement(double value, Unit<Area> unit) => new Area(value, unit);

            protected override IEnumerable<Unit<Area>> GetParsableUnits() => new[] { MeterSquared, Acre };
        }
    }

    public static partial class MeasurementExtensions {
        public static Volume Multiply(this Measurement<Area> area, Measurement<Distance> distance) {
            Validate.NonNull(area, nameof(area));
            Validate.NonNull(distance, nameof(distance));

            return new Volume(distance, area);
        }
    }
}