using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Measurements.Physics {
    public sealed class Energy : Term<Energy, Force, Distance> {
        public static IMeasurementProvider<Energy> Provider { get; } = new EnergyProvider();

        public override IMeasurementProvider<Energy> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Force> Item1Provider => Force.Provider;

        protected override IMeasurementProvider<Distance> Item2Provider => Distance.Provider;

        public Energy() : base() {
        }

        public Energy(double amount, Unit<Energy> unit) : base(amount, unit) {
        }

        public Energy(Force f, Distance d) : base(f, d) {
        }

        public static class Units {
            public static Unit<Energy> Joule { get; } = new Unit<Energy>("Joule", "J", 1);

            public static PrefixableUnit<Energy> Calorie { get; } = new PrefixableUnit<Energy>("calorie", "cal", 1 / 1.184d);

            public static Unit<Energy> Kilocalorie { get; } = Prefix.Kilo(Calorie);
        }

        private class EnergyProvider : IMeasurementProvider<Energy> {
            public IEnumerable<Unit<Energy>> AllUnits => new[] { Units.Joule };

            public Unit<Energy> DefaultUnit => Units.Joule;

            public Energy CreateMeasurement(double value, Unit<Energy> unit) => new Energy(value, unit);
        }

    }
}
