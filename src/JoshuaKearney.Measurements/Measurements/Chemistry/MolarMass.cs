using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public class MolarMass : Ratio<MolarMass, Mass, ChemicalAmount> {
        public static IMeasurementProvider<MolarMass> Provider { get; } = new MolarMassProvider();

        public override IMeasurementProvider<MolarMass> MeasurementProvider => Provider;

        protected override IMeasurementProvider<ChemicalAmount> DenominatorProvider => ChemicalAmount.Provider;

        protected override IMeasurementProvider<Mass> NumeratorProvider => Mass.Provider;

        public MolarMass() {
        }

        public MolarMass(double amount, Unit<MolarMass> unit) : base(amount, unit) {
        }

        public MolarMass(Mass mass, ChemicalAmount conc) : base(mass, conc) {
        }

        public MolarMass(double amount, Unit<Mass> massUnit, Unit<ChemicalAmount> subsUnit) : base(amount, massUnit, subsUnit) {
        }

        public static class Units {
            public static Unit<MolarMass> KilgoramsPerMole { get; } = new Mass(1, Mass.Units.Kilogram).Divide(new ChemicalAmount(1, ChemicalAmount.Units.Mole)).CreateUnit("kilogram per mole", "kg/mol");

            public static Unit<MolarMass> GramsPerMole { get; } = new Mass(1, Mass.Units.Gram).Divide(new ChemicalAmount(1, ChemicalAmount.Units.Mole)).CreateUnit("gram per mole", "g/mol");
        }

        private class MolarMassProvider : IMeasurementProvider<MolarMass> {
            public IEnumerable<Unit<MolarMass>> AllUnits => new[] { Units.GramsPerMole, Units.KilgoramsPerMole };

            public Unit<MolarMass> DefaultUnit => Units.KilgoramsPerMole;

            public MolarMass CreateMeasurement(double value, Unit<MolarMass> unit) {
                return new MolarMass(value, unit);
            }
        }
    }
}