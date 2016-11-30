using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class MeasurementExtensions {

        public static Volume Cube(this Measurement<Distance> distance) {
            Validate.NonNull(distance, nameof(distance));

            return distance.Multiply(distance).Multiply(distance);
        }

        public static Area Multiply(this Measurement<Distance> distance1, Measurement<Distance> distance2) {
            Validate.NonNull(distance1, nameof(distance1));
            Validate.NonNull(distance2, nameof(distance2));

            return new Area(distance1, distance1);
        }

        public static Volume Multiply(this Measurement<Distance> distance, Measurement<Area> area) {
            Validate.NonNull(distance, nameof(distance));
            Validate.NonNull(area, nameof(area));

            return new Volume(distance, area);
        }

        public static Area Square(this Measurement<Distance> distance) {
            Validate.NonNull(distance, nameof(distance));

            return distance.Multiply(distance);
        }
    }

    public sealed class Distance : Measurement<Distance>,
            IMultipliableMeasurement<Distance, Area>,
            IMultipliableMeasurement<Area, Volume>,
            ISquareableMeasurement<Area>,
            ICubableMeasurement<Volume> {

        public Distance() {
        }

        public Distance(double amount, Unit<Distance> unit) : base(amount, unit) {
        }

        public static Lazy<IMeasurementProvider<Distance>> Provider { get; } = new Lazy<IMeasurementProvider<Distance>>(() => new DistanceProvider());

        public override Lazy<IMeasurementProvider<Distance>> MeasurementProvider => Provider;

        Volume ICubableMeasurement<Volume>.Cube() => this.Cube();

        Volume IMultipliableMeasurement<Area, Volume>.Multiply(Area measurement2) => this.Multiply(measurement2);

        Area IMultipliableMeasurement<Distance, Area>.Multiply(Distance measurement2) => this.Multiply(measurement2);

        Area ISquareableMeasurement<Area>.Square() => this.Square();

        public static class Units {
            private static Lazy<Unit<Distance>> centimeter = new Lazy<Unit<Distance>>(() => Prefix.Centi(Meter));

            private static Lazy<Unit<Distance>> foot = new Lazy<Unit<Distance>>(() => new Unit<Distance>(
                symbol: "ft",
                defaultsPerUnit: .3048,
                provider: Provider
            ));

            private static Lazy<Unit<Distance>> inch = new Lazy<Unit<Distance>>(() => Foot.Divide(12).ToUnit("in"));

            private static Lazy<Unit<Distance>> kilometer = new Lazy<Unit<Distance>>(() => Prefix.Kilo(Meter));

            private static Lazy<PrefixableUnit<Distance>> meter = new Lazy<PrefixableUnit<Distance>>(() => 
                new PrefixableUnit<Distance>(
                    symbol: "m",
                    defaultsPerUnit: 1,
                    provider: Provider
                )
            );

            private static Lazy<Unit<Distance>> mile = new Lazy<Unit<Distance>>(() => Foot.Multiply(5280).ToUnit("mi"));

            private static Lazy<Unit<Distance>> millimeter = new Lazy<Unit<Distance>>(() => Prefix.Milli(Meter));

            private static Lazy<Unit<Distance>> yard = new Lazy<Unit<Distance>>(() => Foot.Multiply(3).ToUnit("yd"));

            public static Unit<Distance> Centimeter => centimeter.Value;

            public static Unit<Distance> Foot => foot.Value;

            public static Unit<Distance> Inch => inch.Value;

            public static Unit<Distance> Kilometer => kilometer.Value;

            public static PrefixableUnit<Distance> Meter => meter.Value;

            public static Unit<Distance> Mile => mile.Value;

            public static Unit<Distance> Millimeter => millimeter.Value;

            public static Unit<Distance> Yard => yard.Value;
        }

        private class DistanceProvider : IMeasurementProvider<Distance> {

            private Lazy<IEnumerable<Unit<Distance>>> allUnits = new Lazy<IEnumerable<Unit<Distance>>>(
                () => new[] { Units.Centimeter, Units.Kilometer, Units.Meter, Units.Foot, Units.Inch, Units.Mile, Units.Yard, Units.Millimeter }
            );

            public IEnumerable<Unit<Distance>> AllUnits => allUnits.Value;

            public Unit<Distance> DefaultUnit => Units.Meter;

            public Distance CreateMeasurement(double value, Unit<Distance> unit) => new Distance(value, unit);
        }
    }
}