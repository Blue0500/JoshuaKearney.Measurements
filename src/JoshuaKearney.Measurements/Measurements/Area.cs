namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static Area ToArea(this Term<Length, Length> area) {
            Validate.NonNull(area, nameof(area));

            return new Area(area.ToDouble(area.MeasurementProvider.DefaultUnit), area.MeasurementProvider.DefaultUnit.Cast<Term<Length, Length>, Area>());
        }
    }

    public sealed class Area : TermBase<Area, Length, Length>,
            IMultipliableMeasurement<Length, Volume> {
        public static IMeasurementProvider<Area> Provider { get; } = new AreaProvider();

        protected override IMeasurementProvider<Length> Item1Provider => Length.Provider;

        protected override IMeasurementProvider<Length> Item2Provider => Length.Provider;

        public override IMeasurementProvider<Area> MeasurementProvider => Provider;

        public Area() {
        }

        public Area(double amount, Unit<Area> unit) : base(amount, unit) {
        }

        public Area(Length length1, Length length2) : base(length1, length2) {
        }

        public Area(double amount, Unit<Length> length1Def, Unit<Length> length2Def) : base(amount, length1Def, length2Def) {
        }

        public static Volume operator *(Area area, Length other) {
            if (area == null || other == null) {
                return null;
            }

            return area.Multiply(other);
        }

        public Volume Multiply(Length other) {
            Validate.NonNull(other, nameof(other));

            return new Volume(other, this);
        }

        public static class Units {
            public static Unit<Area> Acre { get; } = MeasurementSystems.EnglishArea.Acre;
            public static Unit<Area> CentimeterSquared { get; } = Length.Units.Centimeter.Square<Length, Area>();
            public static Unit<Area> FootSquared { get; } = MeasurementSystems.EnglishLength.Foot.Square<Length, Area>();
            public static Unit<Area> Hectare { get; } = Prefix.Hecto(MeasurementSystems.Metric.Are);
            public static Unit<Area> InchSquared { get; } = Length.Units.Inch.Square<Length, Area>();
            public static Unit<Area> KilometerSquared { get; } = Length.Units.Kilometer.Square<Length, Area>();

            public static Unit<Area> MeterSquared { get; } = MeasurementSystems.Metric.Meter.Square<Length, Area>();
            public static Unit<Area> MileSquared { get; } = Length.Units.Mile.Square<Length, Area>();
            public static Unit<Area> MillimeterSquared { get; } = Length.Units.Millimeter.Square<Length, Area>();
            public static Unit<Area> YardSquared { get; } = Length.Units.Yard.Square<Length, Area>();
        }

        private class AreaProvider : IMeasurementProvider<Area> {
            public Unit<Area> DefaultUnit => Units.MeterSquared;

            public Area CreateMeasurement(double value, Unit<Area> unit) => new Area(value, unit);
        }
    }
}