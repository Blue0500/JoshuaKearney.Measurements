using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Time : Measurement<Time> {
        public static IMeasurementProvider<Time> Provider { get; } = new TimeProvider();

        public Time(double amount, IUnit<Time> unit) : base(amount, unit) {
        }

        public Time() {
        }

        public override IMeasurementProvider<Time> MeasurementProvider => Provider;

        public static class Units {
            private static Lazy<IPrefixableUnit<Time>> second = new Lazy<IPrefixableUnit<Time>>(() => Unit.CreatePrefixable<Time>("second", "s", 1));

            public static IPrefixableUnit<Time> Second => second.Value;

            public static IUnit<Time> Millisecond { get; } = Prefix.Milli(Second);

            public static IUnit<Time> Minute { get; } = Unit.Create<Time>("minute", "min", 1 / 60);

            public static IUnit<Time> Hour { get; } = Unit.Create<Time>("hour", "hr", 1 / 60 / 60);
        }

        private class TimeProvider : IMeasurementProvider<Time> {
            public IUnit<Time> DefaultUnit => Units.Second;

            public Time CreateMeasurement(double value, IUnit<Time> unit) => new Time(value, unit);
        }
    }
}