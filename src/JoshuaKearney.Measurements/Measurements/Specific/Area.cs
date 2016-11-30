using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class MeasurementExtensions {
        public static Volume Multiply(this Measurement<Area> area, Measurement<Distance> distance) {
            Validate.NonNull(area, nameof(area));
            Validate.NonNull(distance, nameof(distance));

            return new Volume(distance, area);
        }
    }

    public sealed class Area : Term<Area, Distance, Distance>,
        IDividableMeasurement<Distance, Distance>,
        IMultipliableMeasurement<Distance, Volume> {

        public Area() {
        }

        public Area(double amount, Unit<Area> unit) : base(amount, unit) {
        }

        public Area(Distance length1, Distance length2) : base(length1, length2, Provider) {
        }

        public Area(double amount, Unit<Distance> length1Def, Unit<Distance> length2Def) : base(amount, length1Def, length2Def, Provider) {
        }

        public static Lazy<IMeasurementProvider<Area>> Provider { get; } = new Lazy<IMeasurementProvider<Area>>(() => new AreaProvider());

        public override Lazy<IMeasurementProvider<Area>> MeasurementProvider => Provider;

        public override Lazy<IMeasurementProvider<Distance>> Item1Provider => Distance.Provider;

        public override Lazy<IMeasurementProvider<Distance>> Item2Provider => Distance.Provider;

        Volume IMultipliableMeasurement<Distance, Volume>.Multiply(Distance measurement2) => this.Multiply(measurement2);

        public static class Units {
            private static Lazy<Unit<Area>> acre = new Lazy<Unit<Area>>(() => new Unit<Area>(
                symbol: "acre",
                defaultsPerUnit: 4046.8564224,
                provider: Provider
            ));

            private static Lazy<Unit<Area>> centimeterSquared = new Lazy<Unit<Area>>(() => Distance.Units.Centimeter.Square().ToUnit("cm^2"));

            private static Lazy<Unit<Area>> footSquared = new Lazy<Unit<Area>>(() => Distance.Units.Foot.Square().ToUnit("ft^2"));

            private static Lazy<Unit<Area>> hectare = new Lazy<Unit<Area>>(() => Prefix.Hecto(
                new PrefixableUnit<Area>("a", 100, Provider)
            ));

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

        private class AreaProvider : IMeasurementProvider<Area>, IComplexMeasurementProvider<Distance, Distance> {
            private Lazy<IEnumerable<Unit<Area>>> allUnits = new Lazy<IEnumerable<Unit<Area>>>(() => new[] { Units.Acre });

            public IEnumerable<Unit<Area>> AllUnits => allUnits.Value;

            public Lazy<IMeasurementProvider<Distance>> Component1Provider => Distance.Provider;

            public Lazy<IMeasurementProvider<Distance>> Component2Provider => Distance.Provider;

            public Unit<Area> DefaultUnit => Units.MeterSquared;

            public Area CreateMeasurement(double value, Unit<Area> unit) => new Area(value, unit);
        }
    }
}