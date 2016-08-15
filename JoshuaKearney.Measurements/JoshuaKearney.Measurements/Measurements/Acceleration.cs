using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Acceleration : RatioBase<Acceleration, Speed, Time> {
        public static Acceleration Gravity { get; } = new Acceleration(9.81, Acceleration.Units.MetersPerSecondSquared);

        public static IMeasurementProvider<Acceleration> Provider { get; } = new AccelerationProvider();

        public override IMeasurementProvider<Acceleration> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

        protected override IMeasurementProvider<Speed> NumeratorProvider => Speed.Provider;

        public Acceleration() {
        }

        public Acceleration(double amount, IUnit<Acceleration> unit) : base(amount, unit) {
        }

        public Acceleration(Speed length, Time time) : base(length, time) {
        }

        public Acceleration(double amount, IUnit<Speed> lengthDef, IUnit<Time> timeDef) : base(amount, lengthDef, timeDef) {
        }

        public static class Units {
            public static IUnit<Acceleration> MetersPerSecondSquared { get; } = Speed.Units.MetersPerSecond.Divide<Speed, Time, Acceleration>(Time.Units.Second);
        }

        private class AccelerationProvider : IMeasurementProvider<Acceleration> {
            public IUnit<Acceleration> DefaultUnit => Units.MetersPerSecondSquared;

            public Acceleration CreateMeasurement(double value, IUnit<Acceleration> unit) => new Acceleration(value, unit);
        }
    }
}