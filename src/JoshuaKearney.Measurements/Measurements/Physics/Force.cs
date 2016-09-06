using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Force : TermBase<Force, Mass, Acceleration>,
        IDividableMeasurement<Area, Pressure>,
        IDividableMeasurement<Mass, Acceleration> {

        public Force() {
        }

        public Force(double amount, Unit<Force> unit) : base(amount, unit) {
        }

        public Force(Mass mass, Acceleration acceleration) : base(mass, acceleration) {
        }

        public Force(double amount, Unit<Mass> massDef, Unit<Acceleration> accelDef) : base(amount, massDef, accelDef) {
        }

        public static IMeasurementProvider<Force> Provider { get; } = new ForceProvider();

        public override IMeasurementProvider<Force> MeasurementProvider => Provider;

        public static class Units {
            private static Unit<Force> newton = new Unit<Force>("newton", "N", 1);

            public static Unit<Force> PoundForce { get; } = new Mass(1, Mass.Units.Pound).Multiply(Acceleration.Gravity).CreateUnit("pount-force", "lbf");

            // new Unit<Force>("pound-force", "lbf", new Force(1, Newton).Divide(Acceleration.Gravity).Divide(new Mass(1, Mass.Units.Pound)));

            public static Unit<Force> Newton => newton;
        }

        protected override IMeasurementProvider<Mass> Item1Provider => Mass.Provider;

        protected override IMeasurementProvider<Acceleration> Item2Provider => Acceleration.Provider;

        public static Acceleration operator /(Force force, Mass mass) {
            if (force == null || mass == null) {
                return null;
            }
            else {
                return force.Divide(mass);
            }
        }

        public Acceleration Divide(Mass second) {
            Validate.NonNull(second, nameof(second));

            return this.DivideToSecond(second);
        }

        public Pressure Divide(Area second) {
            Validate.NonNull(second, nameof(second));

            return new Pressure(this, second);
        }

        private class ForceProvider : IMeasurementProvider<Force>, IComplexMeasurementProvider<Mass, Acceleration> {
            public IEnumerable<Unit<Force>> BaseUnits { get; } = new[] { Units.Newton, Units.PoundForce };

            public IMeasurementProvider<Mass> Component1Provider => Mass.Provider;

            public IMeasurementProvider<Acceleration> Component2Provider => Acceleration.Provider;

            public Unit<Force> DefaultUnit => Units.Newton;

            public Force CreateMeasurement(double value, Unit<Force> unit) => new Force(value, unit);
        }
    }
}