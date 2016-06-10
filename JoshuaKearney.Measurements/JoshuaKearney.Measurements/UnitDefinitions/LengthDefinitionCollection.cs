using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class LengthDefinitionCollection : UnitDefinitionCollection<Length> {
        public override IEnumerable<UnitDefinition<Length>> AllUnits { get; }

        public LengthDefinitionCollection() {
            AllUnits = new List<UnitDefinition<Length>>() {
                this.Kilometer, this.Meter, this.Centimeter, this.Millimeter, this.Mile, this.Yard, this.Foot, this.Inch
            };
        }

        public UnitDefinition<Length> Kilometer { get; } = new UnitDefinition<Length>("km", x => Length.FromMeters(x * 1000), x => x.Meters / 1000, MeasurementSystem.Metric);

        public UnitDefinition<Length> Meter { get; } = new UnitDefinition<Length>("m", x => new Length().WithStandardUnits(x), x => x.Meters, MeasurementSystem.Metric);

        public UnitDefinition<Length> Centimeter { get; } = new UnitDefinition<Length>("cm", x => Length.FromMeters(x / 100), x => x.Meters * 100, MeasurementSystem.Metric);

        public UnitDefinition<Length> Millimeter { get; } = new UnitDefinition<Length>("mm", x => Length.FromMeters(x / 1000), x => x.Meters * 1000, MeasurementSystem.Metric);

        public UnitDefinition<Length> Mile { get; } = new UnitDefinition<Length>("mi", x => Length.FromMeters(x * 1609.344), x => x.Meters / 1609.344, MeasurementSystem.Customary);

        public UnitDefinition<Length> Yard { get; } = new UnitDefinition<Length>("yd", x => Length.FromMeters(x * 3 * .3048), x => x.Meters / 3 / .3048, MeasurementSystem.Customary);

        public UnitDefinition<Length> Foot { get; } = new UnitDefinition<Length>("ft", x => Length.FromMeters(x * .3048), x => x.Meters / .3048, MeasurementSystem.Customary);

        public UnitDefinition<Length> Inch { get; } = new UnitDefinition<Length>("in", x => Length.FromMeters(x / 12 * .3048), x => x.Meters * 12 / .3048, MeasurementSystem.Customary);

        public override UnitDefinition<Length> StandardUnit {
            get {
                return this.Meter;
            }
        }
    }
}