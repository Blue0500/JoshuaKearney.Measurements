//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements {

//    public class Speed : Ratio<Speed, Distance, Time>, 
//        IMultipliableMeasurement<Time, Distance>,
//        IDividableMeasurement<Time, Acceleration>, 
//        IDividableMeasurement<Distance, Frequency> {

//        public static IMeasurementProvider<Speed> Provider { get; } = new SpeedProvider();

//        public static Speed SpeedOfSound { get; } = new Speed(340.29, Units.MetersPerSecond);

//        public static Speed SpeedOfLight { get; } = new Speed(299792458, Units.MetersPerSecond);

//        public override IMeasurementProvider<Speed> MeasurementProvider => Provider;

//        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

//        protected override IMeasurementProvider<Distance> NumeratorProvider => Distance.Provider;

//        public Speed() {
//        }

//        public Speed(double amount, Unit<Speed> unit) : base(amount, unit) {
//        }

//        public Speed(Distance length, Time time) : base(length, time) {
//        }

//        public Speed(double amount, Unit<Distance> lengthDef, Unit<Time> timeDef) : base(amount, lengthDef, timeDef) {
//        }

//        public static class Units {
//            public static Unit<Speed> MetersPerSecond { get; } = Distance.Units.Meter.Divide<Distance, Time, Speed>(Time.Units.Second);

//            public static Unit<Speed> MilesPerHour { get; } = Distance.Units.Mile.Divide<Distance, Time, Speed>(Time.Units.Hour);

//            public static Unit<Speed> KilometersPerSecond { get; } = Distance.Units.Kilometer.Divide<Distance, Time, Speed>(Time.Units.Second);
//        }

//        private class SpeedProvider : IMeasurementProvider<Speed>, IComplexMeasurementProvider<Distance, Time> {
//            public IEnumerable<Unit<Speed>> AllUnits { get; } = new Unit<Speed>[] { };

//            public IMeasurementProvider<Distance> Component1Provider => Distance.Provider;

//            public IMeasurementProvider<Time> Component2Provider => Time.Provider;

//            public Unit<Speed> DefaultUnit => Units.MetersPerSecond;

//            public Speed CreateMeasurement(double value, Unit<Speed> unit) => new Speed(value, unit);
//        }

//        public Acceleration Divide(Time measurement2) {
//            Validate.NonNull(measurement2, nameof(measurement2));

//            return new Acceleration(this, measurement2);
//        }

//        public Frequency Divide(Distance measurement2) {
//            return new Frequency(this.ToDouble(Units.MetersPerSecond) / measurement2.ToDouble(Distance.Units.Meter), Frequency.Units.Hertz);
//        }

//        public static Acceleration operator /(Speed speed, Time time) {
//            if (speed == null || time == null) {
//                return null;
//            }

//            return speed.Divide(time);
//        }
//    }
//}