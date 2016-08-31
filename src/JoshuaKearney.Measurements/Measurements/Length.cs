namespace JoshuaKearney.Measurements {

    public sealed class Distance : Measurement<Distance>,
            IMultipliableMeasurement<Distance, Area>,
            IMultipliableMeasurement<Area, Volume>,
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

        public Volume Cube() => this.Multiply(this.Square());

        public Area Multiply(Distance length) {
            Validate.NonNull(length, nameof(length));
            return new Area(this, length);
        }

        public Volume Multiply(Area area) {
            Validate.NonNull(area, nameof(area));
            return new Volume(this, area);
        }

        public Area Square() => this.Multiply(this);

        //public static class Units {
            public static PrefixableUnit<Distance> Meter { get; } = MeasurementSystems.Metric.Meter;

            public static Unit<Distance> Centimeter { get; } = Prefix.Centi(MeasurementSystems.Metric.Meter);
            public static Unit<Distance> Foot { get; } = MeasurementSystems.EnglishLength.Foot;
            public static Unit<Distance> Inch { get; } = MeasurementSystems.EnglishLength.Inch;
            public static Unit<Distance> Kilometer { get; } = Prefix.Kilo(MeasurementSystems.Metric.Meter);
            public static Unit<Distance> Mile { get; } = MeasurementSystems.EnglishLength.Mile;
            public static Unit<Distance> Millimeter { get; } = Prefix.Milli(MeasurementSystems.Metric.Meter);
            public static Unit<Distance> Yard { get; } = MeasurementSystems.EnglishLength.Yard;
        //}

        private class LengthProvider : IMeasurementProvider<Distance> {
            public Unit<Distance> DefaultUnit => Meter;

            public Distance CreateMeasurement(double value, Unit<Distance> unit) => new Distance(value, unit);
        }
    }
}