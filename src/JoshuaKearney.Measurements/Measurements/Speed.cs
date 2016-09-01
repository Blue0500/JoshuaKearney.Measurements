using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Speed : RatioBase<Speed, Distance, Time>, IDividableMeasurement<Time, Acceleration> {
        public static IMeasurementProvider<Speed> Provider { get; } = new SpeedProvider();

        public override IMeasurementProvider<Speed> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

        protected override IMeasurementProvider<Distance> NumeratorProvider => Distance.Provider;

        public Speed() {
        }

        public Speed(double amount, Unit<Speed> unit) : base(amount, unit) {
        }

        public Speed(Distance length, Time time) : base(length, time) {
        }

        public Speed(double amount, Unit<Distance> lengthDef, Unit<Time> timeDef) : base(amount, lengthDef, timeDef) {
        }

        //public static class Units {
        public static Unit<Speed> MetersPerSecond { get; } = Distance.Meter.Divide<Distance, Time, Speed>(Time.Second);

        public static Unit<Speed> MilesPerHour { get; } = Distance.Mile.Divide<Distance, Time, Speed>(Time.Hour);

        public static Unit<Speed> KilometersPerSecond { get; } = Distance.Kilometer.Divide<Distance, Time, Speed>(Time.Second);

        //}

        private class SpeedProvider : IMeasurementProvider<Speed> {
            public Unit<Speed> DefaultUnit => MetersPerSecond;

            public Speed CreateMeasurement(double value, Unit<Speed> unit) => new Speed(value, unit);
        }

        public Acceleration Divide(Time second) {
            Validate.NonNull(second, nameof(second));

            return new Acceleration(this, second);
        }

        public static Acceleration operator /(Speed speed, Time time) {
            if (speed == null || time == null) {
                return null;
            }

            return speed.Divide(time);
        }
    }
}