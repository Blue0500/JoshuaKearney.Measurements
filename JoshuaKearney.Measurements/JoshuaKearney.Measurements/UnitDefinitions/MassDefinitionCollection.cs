using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class MassDefinitionCollection : UnitDefinitionCollection<Mass> {
        public override IEnumerable<UnitDefinition<Mass>> AllUnits { get; }

        public MassDefinitionCollection() {
            AllUnits = new List<UnitDefinition<Mass>>() {
                this.MetricTon, this.Kilogram, this.Gram, this.Milligram, this.Ton, this.Pound, this.Ounce
            };
        }

        public UnitDefinition<Mass> MetricTon { get; } = new UnitDefinition<Mass>("t", x => Mass.FromKilograms(x * 1000), x => x.Kilograms / 1000, MeasurementSystem.Metric);

        public UnitDefinition<Mass> Kilogram { get; } = new UnitDefinition<Mass>("Kg", x => Mass.FromKilograms(x), x => x.Kilograms, MeasurementSystem.Metric);

        public UnitDefinition<Mass> Gram { get; } = new UnitDefinition<Mass>("g", x => Mass.FromKilograms(x / 1000), x => x.Kilograms * 1000, MeasurementSystem.Metric);

        public UnitDefinition<Mass> Milligram { get; } = new UnitDefinition<Mass>("mg", x => Mass.FromKilograms(x * 1000 * 1000), x => x.Kilograms / (1000 * 1000), MeasurementSystem.Metric);

        public UnitDefinition<Mass> Ton { get; } = new UnitDefinition<Mass>("ton", x => Mass.FromKilograms(x * 2000 * 0.45359237), x => x.Kilograms / (2000 * 0.45359237), MeasurementSystem.Customary);

        public UnitDefinition<Mass> Pound { get; } = new UnitDefinition<Mass>("lb", x => Mass.FromKilograms(x * 0.45359237), x => x.Kilograms / 0.45359237, MeasurementSystem.Customary);

        public UnitDefinition<Mass> Ounce { get; } = new UnitDefinition<Mass>("oz", x => Mass.FromKilograms(x / 16 * 0.45359237), x => x.Kilograms * 16 / 0.45359237, MeasurementSystem.Customary);

        public override UnitDefinition<Mass> StandardUnit {
            get {
                return this.Kilogram;
            }
        }
    }
}