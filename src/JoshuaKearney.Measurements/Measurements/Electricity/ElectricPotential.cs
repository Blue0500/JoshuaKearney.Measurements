using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class ElectricPotential : Ratio<ElectricPotential, Energy, Charge> {
        public static IMeasurementProvider<ElectricPotential> Provider { get; } = new ElectricPotentialProvider();

        public override IMeasurementProvider<ElectricPotential> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Charge> DenominatorProvider => Charge.Provider;

        protected override IMeasurementProvider<Energy> NumeratorProvider => Energy.Provider;

        public ElectricPotential() : base() {
        }

        public ElectricPotential(double value, Unit<ElectricPotential> unit) : base(value, unit) {
        }

        public ElectricPotential(Energy energy, Charge charge) : base(energy, charge) {
        }

        public ElectricPotential(double value, Unit<Energy> energyUnit, Unit<Charge> chargeUnit) : base(value, energyUnit, chargeUnit) {
        }

        public static class Units {
            private static readonly Lazy<Unit<ElectricPotential>> joulePerSecond = new Lazy<Unit<ElectricPotential>>(() => Energy.Units.Joule.Divide<Energy, Time, ElectricPotential>(Time.Units.Second));
            private static readonly Lazy<Unit<ElectricPotential>> volt = new Lazy<Unit<ElectricPotential>>(() => new Unit<ElectricPotential>("Volt", "V", 1));
            public static Unit<ElectricPotential> JoulePerSecond => joulePerSecond.Value;

            public static Unit<ElectricPotential> Volt => volt.Value;
        }

        private class ElectricPotentialProvider : IMeasurementProvider<ElectricPotential> {
            public IEnumerable<Unit<ElectricPotential>> AllUnits => new[] { Units.JoulePerSecond, Units.Volt };

            public Unit<ElectricPotential> DefaultUnit => Units.Volt;

            public ElectricPotential CreateMeasurement(double value, Unit<ElectricPotential> unit) => new ElectricPotential(value, unit);
        }
    }
}