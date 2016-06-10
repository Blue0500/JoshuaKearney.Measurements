using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Density : Measurement<Density>, IMeasurementRatio<Mass, Volume> {
        public static DensityDefinitionCollection Units { get; } = new DensityDefinitionCollection();

        protected override UnitDefinitionCollection<Density> UnitDefinitions { get; } = Density.Units;

        Measurement IMeasurementRatio<Mass, Volume>.ToMeasurement() {
            return this;
        }

        public static Density FromUnits(double amount, UnitDefinition<Mass> massConv, UnitDefinition<Volume> volumeConv) {
            return new Density().SetUnits(amount, massConv, volumeConv).ToDensity();
        }
    }
}