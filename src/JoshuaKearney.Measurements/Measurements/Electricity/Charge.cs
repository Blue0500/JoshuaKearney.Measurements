using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.Measurements.Electricity {

    public sealed class Charge : Measurement<Charge> {

        public Charge(double amount, Unit<Charge> unit) : base(amount, unit) {
        }

        public Charge() : base() {
        }

        public static IMeasurementProvider<Charge> Provider { get; } = new ChargeProvider();

        public override IMeasurementProvider<Charge> MeasurementProvider => Provider;

        public static class Units {
            private static Lazy<Unit<Charge>> coulomb = new Lazy<Unit<Charge>>(() => new Unit<Charge>("Coulomb", "C", 1));

            private static Lazy<Unit<Charge>> electronCharge = new Lazy<Unit<Electricity.Charge>>(() => new Charge(1, Units.ProtonCharge).Multiply(-1).CreateUnit("Electron Charge", "chargeOfElectron"));

            private static Lazy<Unit<Charge>> protonCharge = new Lazy<Unit<Charge>>(() => new Unit<Charge>("Proton Charge", "chargeOfProton", 6.241509e18));

            public static Unit<Charge> Coulomb => coulomb.Value;

            public static Unit<Charge> ElectronCharge => electronCharge.Value;

            public static Unit<Charge> ProtonCharge => protonCharge.Value;
        }

        private class ChargeProvider : IMeasurementProvider<Charge> {
            public IEnumerable<Unit<Charge>> AllUnits => new Unit<Charge>[] { Units.Coulomb, Units.ProtonCharge };

            public Unit<Charge> DefaultUnit => Units.Coulomb;

            public Charge CreateMeasurement(double value, Unit<Charge> unit) {
                return new Charge(value, unit);
            }
        }
    }
}