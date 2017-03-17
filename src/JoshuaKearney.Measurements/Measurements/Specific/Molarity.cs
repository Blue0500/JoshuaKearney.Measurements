using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public class Molarity : Ratio<Molarity, Amount, Volume> {
        public static MeasurementProvider<Molarity> Provider { get; } = new AmountConcentrationProvider();

        public override MeasurementProvider<Molarity> MeasurementProvider => Provider;

        public override MeasurementProvider<Amount> NumeratorProvider => Amount.Provider;

        public override MeasurementProvider<Volume> DenominatorProvider => Volume.Provider;

        public Molarity() { }

        public Molarity(double value, Unit<Molarity> unit) : base(value, unit) { }

        public Molarity(IMeasurement<Amount> amount, IMeasurement<Volume> volume) : base(amount, volume, Provider) { }

        public static class Units {
            private static readonly Lazy<PrefixableUnit<Molarity>> molar = new Lazy<PrefixableUnit<Molarity>>(() => Amount.Units.Mole.Divide(Volume.Units.Liter).ToPrefixableUnit("M"));

            private static readonly Lazy<Unit<Molarity>> molePerLiter = new Lazy<Unit<Molarity>>(() => Amount.Units.Mole.Divide(Volume.Units.Liter).ToUnit("mol/L"));

            private static readonly Lazy<Unit<Molarity>> molePerMeterCubed = new Lazy<Unit<Molarity>>(() => Amount.Units.Mole.Divide(Volume.Units.MeterCubed).ToUnit("mol/m^3"));

            public static PrefixableUnit<Molarity> Molar => molar.Value;

            public static Unit<Molarity> MolePerLiter => molePerLiter.Value;

            public static Unit<Molarity> MolePerMeterCubed => molePerMeterCubed.Value;

        }

        private class AmountConcentrationProvider : CompoundMeasurementProvider<Molarity, Amount, Volume> {
            public override Molarity CreateMeasurement(double value, Unit<Molarity> unit) => new Molarity(value, unit);

            public override IEnumerable<Operator> ParseOperators {
                get {
                    yield return Operator.CreateMultiplication<Molarity, Volume, Amount>((x, y) => x.Multiply(y));
                }
            }          

            public override IEnumerable<Unit<Molarity>> ParsableUnits {
                get {
                    yield return Units.Molar;
                }
            }

            public override MeasurementProvider<Amount> Component1Provider => Amount.Provider;

            public override MeasurementProvider<Volume> Component2Provider => Volume.Provider;
        }
    }
}
