using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class AreaDefinitionCollection : UnitDefinitionCollection<Area> {
        public override IEnumerable<UnitDefinition<Area>> AllUnits { get; }

        public AreaDefinitionCollection() {
            AllUnits = new List<UnitDefinition<Area>>() {
                this.KilometerSquared, this.MeterSquared, this.CentimeterSquared, this.MillimeterSquared, this.Hectare,
                this.MileSquared, this.YardSquared, this.FootSquared, this.InchSquared, Acre
            };
        }

        public UnitDefinition<Area> KilometerSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Kilometer.Symbol + "²",
            kilometers => new Area().SetUnits(kilometers, Length.Units.Kilometer, Length.Units.Kilometer).ToArea(),
            area => area.ToUnits(Length.Units.Kilometer, Length.Units.Kilometer),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Area> MeterSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Meter.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Meter, Length.Units.Meter).ToArea(),
            area => area.ToUnits(Length.Units.Meter, Length.Units.Meter),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Area> CentimeterSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Centimeter.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Centimeter, Length.Units.Centimeter).ToArea(),
            x => x.ToUnits(Length.Units.Centimeter, Length.Units.Centimeter),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Area> MillimeterSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Millimeter.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Millimeter, Length.Units.Millimeter).ToArea(),
            x => x.ToUnits(Length.Units.Millimeter, Length.Units.Millimeter),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Area> MileSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Mile.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Mile, Length.Units.Mile).ToArea(),
            x => x.ToUnits(Length.Units.Mile, Length.Units.Mile),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Area> YardSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Yard.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Yard, Length.Units.Yard).ToArea(),
            x => x.ToUnits(Length.Units.Yard, Length.Units.Yard),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Area> FootSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Foot.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Foot, Length.Units.Foot).ToArea(),
            x => x.ToUnits(Length.Units.Foot, Length.Units.Foot),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Area> InchSquared { get; } = new UnitDefinition<Area>(
            Length.Units.Inch.Symbol + "²",
            x => new Area().SetUnits(x, Length.Units.Inch, Length.Units.Inch).ToArea(),
            x => x.ToUnits(Length.Units.Inch, Length.Units.Inch),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Area> Acre { get; } = new UnitDefinition<Area>() {
            Symbol = "acre",
            ToStandardUnits = x => Area.FromMilesSquared(x / 640),
            FromStandardUnits = x => x.ToUnits(Length.Units.Mile, Length.Units.Mile) * 640,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Area> Hectare { get; } = new UnitDefinition<Area>() {
            Symbol = "ha",
            ToStandardUnits = x => Area.FromKilometersSquared(x / 100),
            FromStandardUnits = x => x.KilometersSquared * 100,
            MeasurementSystem = MeasurementSystem.Metric
        };

        public override UnitDefinition<Area> StandardUnit { get { return this.MeterSquared; } }
    }
}