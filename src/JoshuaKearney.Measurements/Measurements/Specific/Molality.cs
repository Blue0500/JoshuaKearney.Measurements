using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public class Molality : Ratio<Molality, Amount, Mass> {
        public static MeasurementProvider<Molality> Provider { get; } = new MolalityProvider();

        public override MeasurementProvider<Molality> MeasurementProvider => Provider;

        public override MeasurementProvider<Amount> NumeratorProvider => Amount.Provider;

        public override MeasurementProvider<Mass> DenominatorProvider => Mass.Provider;

        public Molality() { }

        public Molality(double value, Unit<Molality> unit) : base(value, unit) { }

        public Molality(IMeasurement<Amount> amount, IMeasurement<Mass> volume) : base(amount, volume, Provider) { }

        public static class Units {
            private static readonly Lazy<PrefixableUnit<Molality>> molal = new Lazy<PrefixableUnit<Molality>>(() => Amount.Units.Mole.Divide(Mass.Units.Kilogram).ToPrefixableUnit("mol/kg"));

            private static readonly Lazy<Unit<Molality>> molePerKilogram = new Lazy<Unit<Molality>>(() => Molal.ToUnit("mol/kg"));

            public static PrefixableUnit<Molality> Molal => molal.Value;

            public static Unit<Molality> MolePerMeterCubed => molePerKilogram.Value;

        }

        private class MolalityProvider : CompoundMeasurementProvider<Molality, Amount, Mass> {
            public override Molality CreateMeasurement(double value, Unit<Molality> unit) => new Molality(value, unit);

            public override IEnumerable<Operator> ParseOperators => new Operator[] {
                Operator.CreateMultiplication<Molality, Mass, Amount>((x, y) => x.Multiply(y))
            };

            public override IEnumerable<Unit<Molality>> ParsableUnits {
                get {
                    yield return Units.Molal;
                }
            }

            public override MeasurementProvider<Amount> Component1Provider => Amount.Provider;

            public override MeasurementProvider<Mass> Component2Provider => Mass.Provider;
        }
    }
}
