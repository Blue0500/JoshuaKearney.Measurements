using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static Area ToArea(this Term<Distance, Distance> area) {
            Validate.NonNull(area, nameof(area));

            return new Area(area.ToDouble(area.MeasurementProvider.DefaultUnit), area.MeasurementProvider.DefaultUnit.Cast<Term<Distance, Distance>, Area>());
        }
    }

    public sealed class Area : TermBase<Area, Distance, Distance>,
            IMultipliableMeasurement<Distance, Volume> {
        public static IMeasurementProvider<Area> Provider { get; } = new AreaProvider();

        protected override IMeasurementProvider<Distance> Item1Provider => Distance.Provider;

        protected override IMeasurementProvider<Distance> Item2Provider => Distance.Provider;

        public override IMeasurementProvider<Area> MeasurementProvider => Provider;

        public Area() {
        }

        public Area(double amount, Unit<Area> unit) : base(amount, unit) {
        }

        public Area(Distance length1, Distance length2) : base(length1, length2) {
        }

        public Area(double amount, Unit<Distance> length1Def, Unit<Distance> length2Def) : base(amount, length1Def, length2Def) {
        }

        public static Volume operator *(Area area, Distance other) {
            if (area == null || other == null) {
                return null;
            }

            return area.Multiply(other);
        }

        public Volume Multiply(Distance other) {
            Validate.NonNull(other, nameof(other));

            return new Volume(other, this);
        }

        public static class Units {
            public static Unit<Area> Acre { get; } = MeasurementSystems.EnglishArea.Acre;

            public static Unit<Area> CentimeterSquared { get; } = Distance.Units.Centimeter.Square<Distance, Area>();

            public static Unit<Area> FootSquared { get; } = MeasurementSystems.EnglishLength.Foot.Square<Distance, Area>();

            public static Unit<Area> Hectare { get; } = Prefix.Hecto(MeasurementSystems.Metric.Are);

            public static Unit<Area> InchSquared { get; } = Distance.Units.Inch.Square<Distance, Area>();

            public static Unit<Area> KilometerSquared { get; } = Distance.Units.Kilometer.Square<Distance, Area>();

            public static Unit<Area> MeterSquared { get; } = MeasurementSystems.Metric.Meter.Square<Distance, Area>();

            public static Unit<Area> MileSquared { get; } = Distance.Units.Mile.Square<Distance, Area>();

            public static Unit<Area> MillimeterSquared { get; } = Distance.Units.Millimeter.Square<Distance, Area>();

            public static Unit<Area> YardSquared { get; } = Distance.Units.Yard.Square<Distance, Area>();
        }

        private class AreaProvider : IMeasurementProvider<Area>, IComplexMeasurementProvider<Distance, Distance> {
            public IEnumerable<Unit<Area>> BaseUnits { get; } = new[] { Units.Acre };

            public IMeasurementProvider<Distance> Component1Provider => Distance.Provider;

            public IMeasurementProvider<Distance> Component2Provider => Distance.Provider;

            public Unit<Area> DefaultUnit => Units.MeterSquared;

            public Area CreateMeasurement(double value, Unit<Area> unit) => new Area(value, unit);
        }
    }
}