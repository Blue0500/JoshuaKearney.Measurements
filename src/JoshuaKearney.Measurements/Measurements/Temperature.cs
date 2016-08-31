using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    public sealed class Temperature : Measurement<Temperature> {
        public static IMeasurementProvider<Temperature> Provider { get; } = new TemperatureProvider();

        public override IMeasurementProvider<Temperature> MeasurementProvider { get; } = Provider;

        public Temperature() {
        }

        private Temperature(double amount, Unit<Temperature> unit) : base(amount, unit) {
        }

        public Temperature(double kelvins) : base(kelvins, Units.Kelvin) {
        }

        public double ToCelcius() {
            return this.ToDouble(Units.Kelvin) + 273.15;
        }

        public double ToFahrenheit() {
            return 9d / 5d * this.ToCelcius() + 32;
        }

        public static Temperature FromCelcius(double celcius) {
            return new Temperature(celcius - 273.15);
        }

        public static Temperature FromKelvin(double kelvins) {
            return new Temperature(kelvins);
        }

        public static Temperature FromFahrenheit(double fahrenheit) {
            return Temperature.FromCelcius(5d / 9d * (fahrenheit - 32));
        }

        private static class Units {
            public static PrefixableUnit<Temperature> Kelvin { get; } = new PrefixableUnit<Temperature>("kelvin", "K", 1d);
        }

        private class TemperatureProvider : IMeasurementProvider<Temperature> {
            public Unit<Temperature> DefaultUnit {
                get {
                    return Temperature.Units.Kelvin;
                }
            }

            public Temperature CreateMeasurement(double amount, Unit<Temperature> unit) {
                return new Temperature(amount, unit);
            }
        }
    }
}
