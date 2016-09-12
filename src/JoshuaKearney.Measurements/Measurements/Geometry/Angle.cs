using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Angle : Measurement<Angle> {
        public static IMeasurementProvider<Angle> Provider { get; } = new AngleProvider();

        public override IMeasurementProvider<Angle> MeasurementProvider => Provider;

        public Angle() {
        }

        public Angle(double amount, Unit<Angle> unit) : base(amount, unit) {
        }

        public class Units {
            public static Unit<Angle> Radian { get; } = new Unit<Angle>("radian", "rad", 1);

            public static Unit<Angle> Degree { get; } = new Angle(1, Radian).Multiply(Math.PI).Divide(180).CreateUnit("degree", "°");

            public static Unit<Angle> Revolution { get; } = new Angle(1, Radian).Multiply(2 * Math.PI).CreateUnit("revolution", "rev");

            public static Unit<Angle> Gradian { get; } = new Angle(1, Radian).Multiply(Math.PI).Divide(200).CreateUnit("gradian", "grad");
        }

        private class AngleProvider : IMeasurementProvider<Angle> {
            public IEnumerable<Unit<Angle>> BaseUnits { get; } = new[] { Units.Radian, Units.Degree, Units.Revolution, Units.Gradian };

            public Unit<Angle> DefaultUnit => Units.Radian;

            public Angle CreateMeasurement(double value, Unit<Angle> unit) {
                return new Angle(value, unit);
            }
        }
    }
}