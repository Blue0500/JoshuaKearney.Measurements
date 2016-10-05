using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public class ChemicalAmount : Measurement<ChemicalAmount> {
        public static IMeasurementProvider<ChemicalAmount> Provider { get; } = new ChemicalSubstanceProvider();

        public override IMeasurementProvider<ChemicalAmount> MeasurementProvider => Provider;

        public ChemicalAmount() {
        }

        public ChemicalAmount(double amount, Unit<ChemicalAmount> unit) : base(amount, unit) {
        }

        public static class Units {
            public static Unit<ChemicalAmount> Mole { get; } = new Unit<ChemicalAmount>("Mole", "mol", 1);

            public static Unit<ChemicalAmount> RepresentativeParticle { get; } = new Unit<ChemicalAmount>("Rep. Particle", "rep. part.", 6.02214085774e23);
        }

        private class ChemicalSubstanceProvider : IMeasurementProvider<ChemicalAmount> {
            public IEnumerable<Unit<ChemicalAmount>> AllUnits => new[] { Units.Mole };

            public Unit<ChemicalAmount> DefaultUnit => Units.Mole;

            public ChemicalAmount CreateMeasurement(double value, Unit<ChemicalAmount> unit) {
                return new ChemicalAmount(value, unit);
            }
        }
    }
}