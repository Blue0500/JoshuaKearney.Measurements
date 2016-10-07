using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public class Acceleration : Ratio<Acceleration, Speed, Time>//, IMultipliableMeasurement<Mass, Force>
        {
        public Acceleration() {
        }

        public Acceleration(double amount, Unit<Acceleration> unit) : base(amount, unit) {
        }

        public Acceleration(Speed length, Time time) : base(length, time) {
        }

        public Acceleration(double amount, Unit<Speed> lengthDef, Unit<Time> timeDef) : base(amount, lengthDef, timeDef) {
        }

        public static Acceleration Gravity { get; } = new Acceleration(9.80665, Acceleration.Units.MetersPerSecondSquared);

        public static IMeasurementProvider<Acceleration> Provider { get; } = new AccelerationProvider();

        public override IMeasurementProvider<Acceleration> MeasurementProvider => Provider;

        public static class Units {
            private static Unit<Acceleration> metersPerSecondSquared = Speed.Units.MetersPerSecond.Divide<Speed, Time, Acceleration>(Time.Units.Second);

            public static Unit<Acceleration> MetersPerSecondSquared => metersPerSecondSquared;
        }

        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

        protected override IMeasurementProvider<Speed> NumeratorProvider => Speed.Provider;

        private class AccelerationProvider : IMeasurementProvider<Acceleration>, IComplexMeasurementProvider<Speed, Time> {
            public IEnumerable<Unit<Acceleration>> AllUnits => new Unit<Acceleration>[] { };

            public IMeasurementProvider<Speed> Component1Provider => Speed.Provider;

            public IMeasurementProvider<Time> Component2Provider => Time.Provider;

            public Unit<Acceleration> DefaultUnit => Units.MetersPerSecondSquared;

            public Acceleration CreateMeasurement(double value, Unit<Acceleration> unit) => new Acceleration(value, unit);
        }

        public Force Multiply(Mass measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return new Force(measurement2, this);
        }

        public static Force operator *(Acceleration first, Mass measurement2) {
            if (first == null || measurement2 == null) {
                return null;
            }
            else {
                return first.Multiply(measurement2);
            }
        }
    }
}