using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static Area ToArea(this Term<Length, Length> area) {
            Validate.NonNull(area, nameof(area));

            return Measurement.From(
                area.ToDouble(Term<Length, Length>.GetDefaultUnitDefinition()),
                Area.GetDefaultUnitDefinition()
            );
        }
    }

    public sealed class Area : TermBase<Area, Length, Length>,
            IMultipliableMeasurement<Length, Volume> {

        private static MeasurementInfo propertySupplier = new MeasurementInfo(
            instanceSupplier: x => new Area(x),
            storedUnit: CommonUnits.MeterSquared,
            uniqueUnits: new Lazy<IEnumerable<IUnit>>(() => {
                var ret = MeasurementSystems.EnglishArea.AllUnits
                    .Concat(new[] { MeasurementSystems.Metric.Are });

                return ret;
            })
        );

        public Area() {
        }

        private Area(double metersSquared) : base(metersSquared) {
        }

        protected override MeasurementInfo Supplier => propertySupplier;

        public static Volume operator *(Area area, Length other) {
            if (area == null || other == null) {
                return null;
            }

            return area.Multiply(other);
        }

        public Volume Multiply(Length other) {
            Validate.NonNull(other, nameof(other));

            return Volume.From(other, this);
        }

        public static class CommonUnits {
            public static IUnit<Area> Acre { get; } = MeasurementSystems.EnglishArea.Acre;
            public static IUnit<Area> CentimeterSquared { get; } = Length.CommonUnits.Centimeter.Square<Length, Area>();
            public static IUnit<Area> FootSquared { get; } = MeasurementSystems.EnglishLength.Foot.Square<Length, Area>();
            public static IUnit<Area> Hectare { get; } = Prefix.Hecto(MeasurementSystems.Metric.Are);
            public static IUnit<Area> InchSquared { get; } = Length.CommonUnits.Inch.Square<Length, Area>();
            public static IUnit<Area> KilometerSquared { get; } = Length.CommonUnits.Kilometer.Square<Length, Area>();

            public static IUnit<Area> MeterSquared { get; } = MeasurementSystems.Metric.Meter.Square<Length, Area>();
            public static IUnit<Area> MileSquared { get; } = Length.CommonUnits.Mile.Square<Length, Area>();
            public static IUnit<Area> MillimeterSquared { get; } = Length.CommonUnits.Millimeter.Square<Length, Area>();
            public static IUnit<Area> YardSquared { get; } = Length.CommonUnits.Yard.Square<Length, Area>();
        }
    }
}