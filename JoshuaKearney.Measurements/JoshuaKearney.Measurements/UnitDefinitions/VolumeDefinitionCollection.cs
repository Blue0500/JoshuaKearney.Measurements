using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class VolumeDefinitionCollection : UnitDefinitionCollection<Volume> {
        public override IEnumerable<UnitDefinition<Volume>> AllUnits { get; }

        public VolumeDefinitionCollection() {
            AllUnits = new List<UnitDefinition<Volume>>() {
                this.KilometerCubed, this.MeterCubed, this.CentimeterCubed, this.MillimeterCubed,
                this.Liter, this.Milliliter, this.Gallon, this.Quart, this.Pint, this.Cup,
                this.FluidOunces, this.Tablespoon, Teaspoon,
                this.MileCubed, this.YardCubed, this.FootCubed, this.InchCubed
            };
        }

        public UnitDefinition<Volume> KilometerCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Kilometer.Symbol + "³",
            kilometers => new Volume().SetUnits(kilometers, Area.Units.KilometerSquared, Length.Units.Kilometer).ToVolume(),
            area => area.ToUnits(Area.Units.KilometerSquared, Length.Units.Kilometer),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Volume> MeterCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Meter.Symbol + "³",
            x => new Volume().WithStandardUnits(x),
            area => area.ToStandardUnits(),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Volume> CentimeterCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Centimeter.Symbol + "³",
            x => new Volume().SetUnits(x, Area.Units.CentimeterSquared, Length.Units.Centimeter).ToVolume(),
            x => x.ToUnits(Area.Units.CentimeterSquared, Length.Units.Centimeter),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Volume> MillimeterCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Millimeter.Symbol + "³",
            x => new Volume().SetUnits(x, Area.Units.MillimeterSquared, Length.Units.Millimeter).ToVolume(),
            x => x.ToUnits(Area.Units.MillimeterSquared, Length.Units.Millimeter),
            MeasurementSystem.Metric
        );

        public UnitDefinition<Volume> MileCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Mile.Symbol + "³",
            x => new Volume().SetUnits(x, Area.Units.MileSquared, Length.Units.Mile).ToVolume(),
            x => x.ToUnits(Area.Units.MileSquared, Length.Units.Mile),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Volume> YardCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Yard.Symbol + "³",
            x => new Volume().SetUnits(x, Area.Units.YardSquared, Length.Units.Yard).ToVolume(),
            x => x.ToUnits(Area.Units.YardSquared, Length.Units.Yard),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Volume> FootCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Foot.Symbol + "³",
            x => new Volume().SetUnits(x, Area.Units.FootSquared, Length.Units.Foot).ToVolume(),
            x => x.ToUnits(Area.Units.FootSquared, Length.Units.Foot),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Volume> InchCubed { get; } = new UnitDefinition<Volume>(
            Length.Units.Inch.Symbol + "³",
            x => new Volume().SetUnits(x, Area.Units.InchSquared, Length.Units.Inch).ToVolume(),
            x => x.ToUnits(Area.Units.InchSquared, Length.Units.Inch),
            MeasurementSystem.Customary
        );

        public UnitDefinition<Volume> Liter { get; } = new UnitDefinition<Volume>() {
            Symbol = "L",
            ToStandardUnits = x => Volume.FromCentimetersCubed(x * 1000),
            FromStandardUnits = x => x.CentimetersCubed / 1000,
            MeasurementSystem = MeasurementSystem.Metric
        };

        public UnitDefinition<Volume> Milliliter { get; } = new UnitDefinition<Volume>() {
            Symbol = "L",
            ToStandardUnits = x => Volume.FromCentimetersCubed(x),
            FromStandardUnits = x => x.CentimetersCubed,
            MeasurementSystem = MeasurementSystem.Metric
        };

        public UnitDefinition<Volume> Gallon { get; } = new UnitDefinition<Volume>() {
            Symbol = "G",
            ToStandardUnits = x => Volume.FromInchesCubed(x * 231),
            FromStandardUnits = x => x.InchesCubed / 231,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Volume> Quart { get; } = new UnitDefinition<Volume>() {
            Symbol = "qt",
            ToStandardUnits = x => Volume.FromGallons(x / 4),
            FromStandardUnits = x => x.Gallons * 4,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Volume> Pint { get; } = new UnitDefinition<Volume>() {
            Symbol = "pt",
            ToStandardUnits = x => Volume.FromQuarts(x / 2),
            FromStandardUnits = x => x.Quarts * 2,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Volume> Cup { get; } = new UnitDefinition<Volume>() {
            Symbol = "c",
            ToStandardUnits = x => Volume.FromPints(x / 2),
            FromStandardUnits = x => x.Pints * 2,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Volume> FluidOunces { get; } = new UnitDefinition<Volume>() {
            Symbol = "fl oz",
            ToStandardUnits = x => Volume.FromCups(x / 8),
            FromStandardUnits = x => x.Cups * 8,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Volume> Tablespoon { get; } = new UnitDefinition<Volume>() {
            Symbol = "tbsp",
            ToStandardUnits = x => Volume.FromFluidOunces(x / 2),
            FromStandardUnits = x => x.FluidOunces * 2,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Volume> Teaspoon { get; } = new UnitDefinition<Volume>() {
            Symbol = "tsp",
            ToStandardUnits = x => Volume.FromTableSpoons(x / 3),
            FromStandardUnits = x => x.Tablespoons * 3,
            MeasurementSystem = MeasurementSystem.Customary
        };

        public override UnitDefinition<Volume> StandardUnit {
            get {
                return this.MeterCubed;
            }
        }
    }
}