using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Frequency : Measurement<Frequency> {

        public Frequency() {
        }

        public Frequency(double amount, Unit<Frequency> unit) : base(amount, unit) {
        }

        public override IMeasurementProvider<Frequency> MeasurementProvider => Provider;

        public IMeasurementProvider<Frequency> Provider { get; } = new FrequencyProvider();

        public double Multiply(Time time) {
            return this.ToDouble(Units.Hertz) * time.ToDouble(Time.Units.Second);
        }

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