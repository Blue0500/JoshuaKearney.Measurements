using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Speed : Ratio<Speed, Distance, Time>, IDividableMeasurement<Time, Acceleration> {
        public static IMeasurementProvider<Speed> Provider { get; } = new SpeedProvider();

        public static Speed SpeedOfSound { get; } = new Speed(340.29, Units.MetersPerSecond);

        public static Speed SpeedOfLight { get; } = new Speed(299792458, Units.MetersPerSecond);

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

        public static class Units {
            public static Unit<Speed> MetersPerSecond { get; } = Distance.Units.Meter.Divide<Distance, Time, Speed>(Time.Units.Second);

            public static Unit<Speed> MilesPerHour { get; } = Distance.Units.Mile.Divide<Distance, Time, Speed>(Time.Units.Hour);

            public static Unit<Speed> KilometersPerSecond { get; } = Distance.Units.Kilometer.Divide<Distance, Time, Speed>(Time.Units.Second);
        }

        private class SpeedProvider : IMeasurementProvider<Speed>, IComplexMeasurementProvider<Distance, Time> {
            public IEnumerable<Unit<Speed>> BaseUnits { get; } = new Unit<Speed>[] { };

            public IMeasurementProvider<Distance> Component1Provider => Distance.Provider;

            public IMeasurementProvider<Time> Component2Provider => Time.Provider;

            public Unit<Speed> DefaultUnit => Units.MetersPerSecond;

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