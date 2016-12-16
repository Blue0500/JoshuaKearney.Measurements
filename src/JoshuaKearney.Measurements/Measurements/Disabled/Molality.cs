//using System;
//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements.Measurements.Chemistry {

//    public sealed class Molality : Ratio<Molality, ChemicalAmount, Mass> {

//        public Molality() {
//        }

//        public Molality(double amount, Unit<Molality> unit) : base(amount, unit) {
//        }

//        public Molality(double amount, Unit<ChemicalAmount> chemAmountUnit, Unit<Mass> massUnit)
//            : base(amount, chemAmountUnit, massUnit) {
//        }

//        public Molality(ChemicalAmount moles, Mass mass) : base(moles, mass) {
//        }

//        public override IMeasurementSupplier<Molality> MeasurementSupplier => Provider;

//        public IMeasurementSupplier<Molality> Provider { get; } = new MolalityProvider();

//        protected override IMeasurementSupplier<Mass> DenominatorProvider => Mass.Provider;

//        protected override IMeasurementSupplier<ChemicalAmount> NumeratorProvider => ChemicalAmount.Provider;

//        public static class Units {
//            public static Unit<Molality> MolePerKilogram { get; } = new Unit<Molality>("mole per kilogram", "mol/kg", 1);
//        }

//        private class MolalityProvider : IMeasurementSupplier<Molality> {
//            public IEnumerable<Unit<Molality>> AllUnits => new[] { Units.MolePerKilogram };

//            public Unit<Molality> DefaultUnit => Units.MolePerKilogram;

//            public Molality CreateMeasurement(double value, Unit<Molality> unit) => new Molality(value, unit);
//        }
//    }
//}