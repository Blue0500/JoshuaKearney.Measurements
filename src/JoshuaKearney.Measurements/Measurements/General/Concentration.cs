using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Concentration : Measurement<Concentration> {
        public static IMeasurementProvider<Concentration> Provider { get; } = new ConcentrationProvider();

        public override IMeasurementProvider<Concentration> MeasurementProvider => Provider;

        public Concentration() {
        }

        public Concentration(double partsPer, double wholeAmount) : this(partsPer / wholeAmount * 100, Units.Percent) {
        }

        public Concentration(double amount, Unit<Concentration> unit) : base(amount, unit) {
        }

        public static class Units {
            public static Unit<Concentration> Percent { get; } = new Unit<Concentration>("percent", "%", 1);

            public static Unit<Concentration> PartsPerThousand { get; } = new Unit<Concentration>("parts per thousand", "‰", 10);

            public static Unit<Concentration> PartsPerMillion { get; } = new Unit<Concentration>("parts per million", "ppm", 10000);

            public static Unit<Concentration> PartsPerBillion { get; } = new Unit<Concentration>("parts per billion", "ppb", 10000000);

            public static Unit<Concentration> ParsPerTrillion { get; } = new Unit<Concentration>("parts per trillion", "ppt", 10000000000);
        }

        public static implicit operator double(Concentration c) {
            return c.ToDouble();
        }

        public static implicit operator Concentration(double d) {
            return new Concentration(d, 1);
        }

        private class ConcentrationProvider : IMeasurementProvider<Concentration> {
            public IEnumerable<Unit<Concentration>> BaseUnits { get; } = new[] { Units.Percent, Units.PartsPerThousand, Units.PartsPerMillion, Units.PartsPerBillion, Units.ParsPerTrillion };

            public Unit<Concentration> DefaultUnit => Units.Percent;

            public Concentration CreateMeasurement(double value, Unit<Concentration> unit) {
                return new Concentration(value, unit);
            }
        }

        public double ToDouble() {
            return this.ToDouble(Units.Percent) / 100;
        }
    }
}