using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Acceleration : RatioBase<Acceleration, Speed, Time>, IMultipliableMeasurement<Mass, Force> {
        private static Unit<Acceleration> metersPerSecondSquared = Speed.MetersPerSecond.Divide<Speed, Time, Acceleration>(Time.Second);

        public Acceleration() {
        }

        public Acceleration(double amount, Unit<Acceleration> unit) : base(amount, unit) {
        }

        public Acceleration(Speed length, Time time) : base(length, time) {
        }

        public Acceleration(double amount, Unit<Speed> lengthDef, Unit<Time> timeDef) : base(amount, lengthDef, timeDef) {
        }

        public static Acceleration Gravity { get; } = new Acceleration(9.80665, Acceleration.MetersPerSecondSquared);

        public static IMeasurementProvider<Acceleration> Provider { get; } = new AccelerationProvider();

        public override IMeasurementProvider<Acceleration> MeasurementProvider => Provider;

        public static Unit<Acceleration> MetersPerSecondSquared => metersPerSecondSquared;

        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

        protected override IMeasurementProvider<Speed> NumeratorProvider => Speed.Provider;

        private class AccelerationProvider : IMeasurementProvider<Acceleration> {
            public Unit<Acceleration> DefaultUnit => MetersPerSecondSquared;

            public Acceleration CreateMeasurement(double value, Unit<Acceleration> unit) => new Acceleration(value, unit);
        }

        public Force Multiply(Mass second) {
            Validate.NonNull(second, nameof(second));

            return new Force(second, this);
        }

        public static Force operator *(Acceleration first, Mass second) {
            if (first == null || second == null) {
                return null;
            }
            else {
                return first.Multiply(second);
            }
        }

        //
    }
}