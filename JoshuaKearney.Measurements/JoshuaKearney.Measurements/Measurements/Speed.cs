using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Speed : RatioBase<Speed, Length, Time>, IDividableMeasurement<Time, Acceleration> {
        public static IMeasurementProvider<Speed> Provider { get; } = new SpeedProvider();

        public override IMeasurementProvider<Speed> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

        protected override IMeasurementProvider<Length> NumeratorProvider => Length.Provider;

        public Speed() {
        }

        public Speed(double amount, IUnit<Speed> unit) : base(amount, unit) {
        }

        public Speed(Length length, Time time) : base(length, time) {
        }

        public Speed(double amount, IUnit<Length> lengthDef, IUnit<Time> timeDef) : base(amount, lengthDef, timeDef) {
        }

        public static class Units {
            public static IUnit<Speed> MetersPerSecond { get; } = Length.Units.Meter.Divide<Length, Time, Speed>(Time.Units.Second);
        }

        private class SpeedProvider : IMeasurementProvider<Speed> {
            public IUnit<Speed> DefaultUnit => Units.MetersPerSecond;

            public Speed CreateMeasurement(double value, IUnit<Speed> unit) => new Speed(value, unit);
        }

        public Acceleration Divide(Time second) => new Acceleration(this, second);

        public static Acceleration operator /(Speed speed, Time time) {
            if (speed == null || time == null) {
                return null;
            }

            return speed.Divide(time);
        }
    }
}