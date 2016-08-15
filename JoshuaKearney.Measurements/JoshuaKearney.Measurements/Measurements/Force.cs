using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Force : TermBase<Force, Mass, Acceleration> {
        public static IMeasurementProvider<Force> Provider { get; } = new ForceProvider();

        public override IMeasurementProvider<Force> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Mass> Item1Provider => Mass.Provider;

        protected override IMeasurementProvider<Acceleration> Item2Provider => Acceleration.Provider;

        public Force() {
        }

        public Force(double amount, IUnit<Force> unit) : base(amount, unit) {
        }

        public Force(Mass mass, Acceleration acceleration) : base(mass, acceleration) {
        }

        public Force(double amount, IUnit<Mass> massDef, IUnit<Acceleration> accelDef) : base(amount, massDef, accelDef) {
        }

        public static class Units {
            public static IUnit<Force> Newton { get; } = Mass.Units.Kilogram.Multiply<Mass, Acceleration, Force>(Acceleration.Provider.DefaultUnit);

            public static IUnit<Force> PoundForce { get; } = Mass.Units.Pound.Multiply<Mass, Acceleration, Force>(Acceleration.Units.MetersPerSecondSquared);
        }

        private class ForceProvider : IMeasurementProvider<Force> {
            public IUnit<Force> DefaultUnit => Units.Newton;

            public Force CreateMeasurement(double value, IUnit<Force> unit) => new Force(value, unit);
        }
    }
}