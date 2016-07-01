using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class Length : Measurement<Length>,
        IMultipliableMeasurement<Length, Area>,
        IMultipliableMeasurement<Area, Volume>,
        ISquareableMeasurement<Area>,
        ICubableMeasurement<Volume> {

        private static MeasurementInfo propertySupplier = new MeasurementInfo(
            instanceCreator: x => new Length(x),
            defaultUnit: CommonUnits.Meter,
            uniqueUnits: MeasurementSystems.EnglishLength.AllUnits.Concat(new[] { MeasurementSystems.Metric.Meter })
        );

        public Length() {
        }

        private Length(double meters) : base(meters) {
        }

        protected override MeasurementInfo Supplier => propertySupplier;

        public static Area operator *(Length length, Length length2) {
            if (length == null || length2 == null) {
                return null;
            }

            return length.Multiply(length2);
        }

        public static Volume operator *(Length length, Area area) {
            if (length == null || area == null) {
                return null;
            }

            return length.Multiply(area);
        }

        public Volume Cube() => this.Multiply(this.Square());

        public Area Multiply(Length length) {
            Validate.NonNull(length, nameof(length));
            return Area.From(this, length);
        }

        public Volume Multiply(Area area) {
            Validate.NonNull(area, nameof(area));
            return Volume.From(this, area);
        }

        public Area Square() => this.Multiply(this);

        public static class CommonUnits {
            public static IPrefixableUnit<Length> Meter { get; } = MeasurementSystems.Metric.Meter;

            public static IUnit<Length> Centimeter { get; } = Prefix.Centi(MeasurementSystems.Metric.Meter);
            public static IUnit<Length> Foot { get; } = MeasurementSystems.EnglishLength.Foot;
            public static IUnit<Length> Inch { get; } = MeasurementSystems.EnglishLength.Inch;
            public static IUnit<Length> Kilometer { get; } = Prefix.Kilo(MeasurementSystems.Metric.Meter);
            public static IUnit<Length> Mile { get; } = MeasurementSystems.EnglishLength.Mile;

            public static IUnit<Length> Millimeter { get; } = Prefix.Milli(MeasurementSystems.Metric.Meter);

            public static IUnit<Length> Yard { get; } = MeasurementSystems.EnglishLength.Yard;
        }
    }
}