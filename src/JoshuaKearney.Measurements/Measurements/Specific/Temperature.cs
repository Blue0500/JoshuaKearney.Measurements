using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public class Temperature : Measurement<Temperature> {
        public static MeasurementProvider<Temperature> Provider { get; } = new TemperatureInvervalProvider();

        public override MeasurementProvider<Temperature> MeasurementProvider => Provider;

        public Temperature() { }

        public Temperature(double amount, Unit<Temperature> unit) : base(amount, unit) { }

        public double ToCelcius() {
            return this.ToDouble(Units.Kelvin) - 273.15;
        }

        public double ToFahrenheit() {
            return this.ToDouble(Units.Rankine) - 459.67;
        }

        public static Temperature FromCelcius(double amount) {
            return new Temperature(amount + 273.15, Units.Kelvin);
        }

        public static Temperature FromFahrenheit(double amount) {
            return new Temperature(amount + 459.67, Units.Rankine);
        }

        public static class Units {
            private static Lazy<PrefixableUnit<Temperature>> kelvin = new Lazy<PrefixableUnit<Temperature>>(() => new PrefixableUnit<Temperature>("K", Provider));

            private static Lazy<PrefixableUnit<Temperature>> rankine = new Lazy<PrefixableUnit<Temperature>>(() => Kelvin.Multiply(5d / 9).ToPrefixableUnit("R"));

            public static PrefixableUnit<Temperature> Kelvin => kelvin.Value;

            public static PrefixableUnit<Temperature> Rankine => rankine.Value;
        }

        private class TemperatureInvervalProvider : MeasurementProvider<Temperature> {
            protected override IEnumerable<Unit<Temperature>> GetParsableUnits() => new[] { Units.Kelvin, Units.Rankine };

            public override Temperature CreateMeasurement(double value, Unit<Temperature> unit) => new Temperature(value, unit);

            protected override IEnumerable<Operator> GetOperators() => new Operator[0];
        }
    }
}
