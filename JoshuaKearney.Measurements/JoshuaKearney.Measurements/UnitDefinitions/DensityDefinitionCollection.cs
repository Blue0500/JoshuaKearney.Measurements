using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class DensityDefinitionCollection : UnitDefinitionCollection<Density> {
        public override IEnumerable<UnitDefinition<Density>> AllUnits { get; }

        public DensityDefinitionCollection() {
            this.AllUnits = new List<UnitDefinition<Density>>() {
                this.KilogramsPerMeterCubed, this.KilogramsPerLiter, this.GramsPerCentimeterCubed, this.MetricTonsPerMeterCubed,
                this.OuncesPerInchCubed, this.PoundsPerFootCubed
            };
        }

        public UnitDefinition<Density> KilogramsPerMeterCubed { get; } = new UnitDefinition<Density>() {
            Symbol = Mass.Units.Kilogram.Symbol + "/" + Volume.Units.MeterCubed.Symbol,
            ToStandardUnits = x => new Density().WithStandardUnits(x),
            FromStandardUnits = x => x.ToStandardUnits(),
            MeasurementSystem = MeasurementSystem.Metric
        };

        public UnitDefinition<Density> KilogramsPerLiter { get; } = new UnitDefinition<Density>() {
            Symbol = Mass.Units.Kilogram.Symbol + "/" + Volume.Units.Liter.Symbol,
            ToStandardUnits = x => new Density().SetUnits(x, Mass.Units.Kilogram, Volume.Units.Liter).ToDensity(),
            FromStandardUnits = x => x.ToUnits(Mass.Units.Kilogram, Volume.Units.Liter),
            MeasurementSystem = MeasurementSystem.Metric
        };

        public UnitDefinition<Density> GramsPerCentimeterCubed { get; } = new UnitDefinition<Density>() {
            Symbol = Mass.Units.Gram.Symbol + "/" + Volume.Units.CentimeterCubed.Symbol,
            ToStandardUnits = x => new Density().SetUnits(x, Mass.Units.Gram, Volume.Units.CentimeterCubed).ToDensity(),
            FromStandardUnits = x => x.ToUnits(Mass.Units.Gram, Volume.Units.CentimeterCubed),
            MeasurementSystem = MeasurementSystem.Metric
        };

        public UnitDefinition<Density> MetricTonsPerMeterCubed { get; } = new UnitDefinition<Density>() {
            Symbol = Mass.Units.MetricTon.Symbol + "/" + Volume.Units.MeterCubed.Symbol,
            ToStandardUnits = x => new Density().SetUnits(x, Mass.Units.MetricTon, Volume.Units.MeterCubed).ToDensity(),
            FromStandardUnits = x => x.ToUnits(Mass.Units.MetricTon, Volume.Units.MeterCubed),
            MeasurementSystem = MeasurementSystem.Metric
        };

        public UnitDefinition<Density> OuncesPerInchCubed { get; } = new UnitDefinition<Density>() {
            Symbol = Mass.Units.Ounce + "/" + Volume.Units.InchCubed,
            ToStandardUnits = x => new Density().SetUnits(x, Mass.Units.Ounce, Volume.Units.InchCubed).ToDensity(),
            FromStandardUnits = x => x.ToUnits(Mass.Units.Ounce, Volume.Units.InchCubed),
            MeasurementSystem = MeasurementSystem.Customary
        };

        public UnitDefinition<Density> PoundsPerFootCubed { get; } = new UnitDefinition<Density>() {
            Symbol = Mass.Units.Pound + "/" + Volume.Units.FootCubed,
            ToStandardUnits = x => new Density().SetUnits(x, Mass.Units.Pound, Volume.Units.FootCubed).ToDensity(),
            FromStandardUnits = x => x.ToUnits(Mass.Units.Pound, Volume.Units.FootCubed),
            MeasurementSystem = MeasurementSystem.Customary
        };

        public override UnitDefinition<Density> StandardUnit {
            get {
                return this.KilogramsPerMeterCubed;
            }
        }
    }
}