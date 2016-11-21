﻿using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.Measurements.Chemistry {

    public sealed class Molality : Ratio<Molality, ChemicalAmount, Mass> {

        public Molality() {
        }

        public Molality(double amount, Unit<Molality> unit) : base(amount, unit) {
        }

        public Molality(double amount, Unit<ChemicalAmount> chemAmountUnit, Unit<Mass> massUnit)
            : base(amount, chemAmountUnit, massUnit) {
        }

        public Molality(ChemicalAmount moles, Mass mass) : base(moles, mass) {
        }

        public override IMeasurementProvider<Molality> MeasurementProvider => Provider;

        public IMeasurementProvider<Molality> Provider { get; } = new MolalityProvider();

        protected override IMeasurementProvider<Mass> DenominatorProvider => Mass.Provider;

        protected override IMeasurementProvider<ChemicalAmount> NumeratorProvider => ChemicalAmount.Provider;

        public static class Units {
            public static Unit<Molality> MolePerKilogram { get; } = new Unit<Molality>("mole per kilogram", "mol/kg", 1);
        }

        private class MolalityProvider : IMeasurementProvider<Molality> {
            public IEnumerable<Unit<Molality>> AllUnits => new[] { Units.MolePerKilogram };

            public Unit<Molality> DefaultUnit => Units.MolePerKilogram;

            public Molality CreateMeasurement(double value, Unit<Molality> unit) => new Molality(value, unit);
        }
    }
}