using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class Distance : Measurement<Distance>,
            IMultipliableMeasurement<Distance, Area>,
            IMultipliableMeasurement<Area, Volume>,
            IDividableMeasurement<Time, Speed>,
            ISquareableMeasurement<Area>,
            ICubableMeasurement<Volume> {
        public static IMeasurementProvider<Distance> Provider { get; } = new LengthProvider();

        public override IMeasurementProvider<Distance> MeasurementProvider => Provider;

        public Distance() {
        }

        public Distance(double amount, Unit<Distance> unit) : base(amount, unit) {
        }

        public static Area operator *(Distance length, Distance length2) {
            if (length == null || length2 == null) {
                return null;
            }

            return length.Multiply(length2);
        }

        public static Volume operator *(Distance length, Area area) {
            if (length == null || area == null) {
                return null;
            }

            return length.Multiply(area);
        }

        public static Speed operator /(Distance length, Time time) {
            if (length == null || time == null) {
                return null;
            }

            return length.Divide(time);
        }

        /// <summary>
        /// Returns a measurement that represents the cube of this instance
        /// </summary>
        /// <returns></returns>
        public Volume Cube() => this.Multiply(this.Square());

        public Area Multiply(Distance length) {
            Validate.NonNull(length, nameof(length));
            return new Area(this, length);
        }

        public Volume Multiply(Area area) {
            Validate.NonNull(area, nameof(area));
            return new Volume(this, area);
        }

        /// <summary>
        /// Returns a measurement that represents the cube of this instance
        /// </summary>
        /// <returns></returns>
        public Area Square() => this.Multiply(this);

        public Speed Divide(Time time) {
            Validate.NonNull(time, nameof(time));

            return new Speed(this, time);
        }

        public static class Units {
            public static PrefixableUnit<Distance> Meter { get; } = MeasurementSystems.Metric.Meter;

            public static Unit<Distance> Centimeter { get; } = Prefix.Centi(MeasurementSystems.Metric.Meter);

            public static Unit<Distance> Foot { get; } = MeasurementSystems.EnglishLength.Foot;

            public static Unit<Distance> Inch { get; } = MeasurementSystems.EnglishLength.Inch;

            public static Unit<Distance> Kilometer { get; } = Prefix.Kilo(MeasurementSystems.Metric.Meter);

            public static Unit<Distance> Mile { get; } = MeasurementSystems.EnglishLength.Mile;

            public static Unit<Distance> Millimeter { get; } = Prefix.Milli(MeasurementSystems.Metric.Meter);

            public static Unit<Distance> Yard { get; } = MeasurementSystems.EnglishLength.Yard;
        }

        private class LengthProvider : IMeasurementProvider<Distance> {
            public IEnumerable<Unit<Distance>> BaseUnits { get; } = new[] { Units.Meter, Units.Foot, Units.Inch, Units.Mile, Units.Yard };

            public Unit<Distance> DefaultUnit => Units.Meter;

            public Distance CreateMeasurement(double value, Unit<Distance> unit) => new Distance(value, unit);
        }
    }
}