using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Time : Measurement<Time> {
        private static Lazy<PrefixableUnit<Time>> second = new Lazy<PrefixableUnit<Time>>(() => new PrefixableUnit<Time>("second", "s", 1));

        public Time(double amount, Unit<Time> unit) : base(amount, unit) {
        }

        public Time() {
        }

        public Time(TimeSpan s) : base(s.Seconds, Second) {
        }

        public TimeSpan ToTimeSpan() {
            return TimeSpan.FromSeconds(this.ToDouble(Second));
        }

        public static Unit<Time> Hour { get; } = new Unit<Time>("hour", "hr", 1d / 60d / 60d);

        public static Unit<Time> Millisecond { get; } = Prefix.Milli(Second);

        public static Unit<Time> Minute { get; } = new Unit<Time>("minute", "min", 1d / 60d);

        public static Unit<Time> Day { get; } = new Unit<Time>("day", "d", 1d / 24d / 60d / 60d);

        public static IMeasurementProvider<Time> Provider { get; } = new TimeProvider();

        public override IMeasurementProvider<Time> MeasurementProvider => Provider;

        public static PrefixableUnit<Time> Second => second.Value;

        public static implicit operator TimeSpan(Time time) {
            return time.ToTimeSpan();
        }

        public static implicit operator Time(TimeSpan span) {
            return new Time(span);
        }

        private class TimeProvider : IMeasurementProvider<Time> {
            public Unit<Time> DefaultUnit => Second;

            public Time CreateMeasurement(double value, Unit<Time> unit) => new Time(value, unit);
        }
    }
}