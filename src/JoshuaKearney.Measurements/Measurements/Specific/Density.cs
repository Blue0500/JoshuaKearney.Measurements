using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Volume.Units;
using static JoshuaKearney.Measurements.Density.Units;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public class Density : Ratio<Density, Mass, Volume> {
        public static MeasurementProvider<Density> Provider { get; } = new DensityProvider();

        public Density() : base() { }

        public Density(double value, Unit<Density> unit) : base(value, unit) { }

        public Density(IMeasurement<Mass> mass, IMeasurement<Volume> volume) : base(mass, volume, Provider) { }

        public override MeasurementProvider<Volume> DenominatorProvider => Volume.Provider;

        public override MeasurementProvider<Density> MeasurementProvider => Provider;

        public override MeasurementProvider<Mass> NumeratorProvider => Mass.Provider;

        public static class Units {
            public static Unit<Density> KilogramPerMeterCubed { get; } = Kilogram.Divide(MeterCubed).ToUnit("kg/m^3");

            public static Unit<Density> GramPerMilliliter { get; } = Gram.Divide(Milliliter).ToUnit("g/mL");
        }

        private class DensityProvider : CompoundMeasurementProvider<Density, Mass, Volume> {
            public override MeasurementProvider<Mass> Component1Provider => Mass.Provider;

            public override MeasurementProvider<Volume> Component2Provider => Volume.Provider;

            public override Density CreateMeasurement(double value, Unit<Density> unit) => new Density(value, unit);

            protected override IEnumerable<Unit<Density>> GetParsableUnits() => new[] { KilogramPerMeterCubed, GramPerMilliliter };

            protected override IEnumerable<Operator> GetOperators() => new[] {
                Operator.CreateMultiplication<Density, Volume, Mass>((x, y) => x.Multiply(y))
            };
        }
    }
}
