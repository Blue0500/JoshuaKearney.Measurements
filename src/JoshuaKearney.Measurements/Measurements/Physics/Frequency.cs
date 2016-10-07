using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public class Frequency : Ratio<Frequency, DoubleMeasurement, Time> {

        public Frequency() {
        }

        public Frequency(double amount, Unit<Frequency> unit) : base(amount, unit) {
        }

        public Frequency(double amount, Time time) : base(amount, time) {
        }

        public override IMeasurementProvider<Frequency> MeasurementProvider => Provider;

        public IMeasurementProvider<Frequency> Provider { get; } = new FrequencyProvider();

        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

        protected override IMeasurementProvider<DoubleMeasurement> NumeratorProvider => DoubleMeasurement.Provider;

        public static class Units {
            private static PrefixableUnit<Frequency> hertz = new PrefixableUnit<Frequency>("Hertz", "Hz", 1);

            public static Unit<Frequency> GigaHertz { get; } = Prefix.Giga(Hertz);

            public static Unit<Frequency> MegaHertz { get; } = Prefix.Mega(Hertz);

            public static PrefixableUnit<Frequency> Hertz => hertz;
        }

        private class FrequencyProvider : IMeasurementProvider<Frequency> {
            public IEnumerable<Unit<Frequency>> AllUnits { get; } = new[] { Units.Hertz };

            public Unit<Frequency> DefaultUnit => Units.Hertz;

            public Frequency CreateMeasurement(double value, Unit<Frequency> unit) {
                return new Frequency(value, unit);
            }
        }
    }
}