﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Time : Measurement<Time> {

        public Time(double amount, Unit<Time> unit) : base(amount, unit) {
        }

        public Time() {
        }

        public Time(TimeSpan s) : base(s.Seconds, Units.Second) {
        }

        public TimeSpan ToTimeSpan() {
            return TimeSpan.FromSeconds(this.ToDouble(Units.Second));
        }

        public static IMeasurementProvider<Time> Provider { get; } = new TimeProvider();

        public override IMeasurementProvider<Time> MeasurementProvider => Provider;

        public static class Units {
            private static Lazy<PrefixableUnit<Time>> second = new Lazy<PrefixableUnit<Time>>(() => new PrefixableUnit<Time>("second", "s", 1));

            public static Unit<Time> Hour { get; } = new Unit<Time>("hour", "hr", 1d / 60d / 60d);

            public static Unit<Time> Millisecond { get; } = Prefix.Milli(Second);

            public static Unit<Time> Minute { get; } = new Unit<Time>("minute", "min", 1d / 60d);

            public static Unit<Time> Day { get; } = new Unit<Time>("day", "d", 1d / 24d / 60d / 60d);

            public static PrefixableUnit<Time> Second => second.Value;
        }

        public static implicit operator TimeSpan(Time time) {
            return time.ToTimeSpan();
        }

        public static implicit operator Time(TimeSpan span) {
            return new Time(span);
        }

        private class TimeProvider : IMeasurementProvider<Time> {
            public IEnumerable<Unit<Time>> BaseUnits { get; } = new[] { Units.Hour, Units.Second, Units.Minute, Units.Day };

            public Unit<Time> DefaultUnit => Units.Second;

            public Time CreateMeasurement(double value, Unit<Time> unit) => new Time(value, unit);
        }
    }
}