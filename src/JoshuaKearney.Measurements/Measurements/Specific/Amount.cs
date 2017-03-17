using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public class Amount : Measurement<Amount> {
        private static readonly Lazy<Ratio<DoubleMeasurement, Amount>> avagadroConstant = new Lazy<Ratio<DoubleMeasurement, Amount>>(() => new DoubleMeasurement(6.02214085774e23).DivideToRatio(Units.Mole));

        public static Ratio<DoubleMeasurement, Amount> AvagadroConstant => avagadroConstant.Value;

        public static MeasurementProvider<Amount> Provider { get; } = new AmountProvider();

        public override MeasurementProvider<Amount> MeasurementProvider => Provider;

        public Amount() { }

        public Amount(double value, Unit<Amount> unit) : base(value, unit) { }

        public static class Units {
            private static readonly Lazy<PrefixableUnit<Amount>> mole = new Lazy<PrefixableUnit<Amount>>(() => new PrefixableUnit<Amount>("mol", Provider));

            private static readonly Lazy<Unit<Amount>> gross = new Lazy<Unit<Amount>>(() => Mole.Divide(6.02214085774e23).ToUnit("gross"));

            private static readonly Lazy<Unit<Amount>> dozen = new Lazy<Unit<Amount>>(() => Gross.Divide(12).ToUnit("doz"));

            public static PrefixableUnit<Amount> Mole => mole.Value;

            public static Unit<Amount> Gross => gross.Value;

            public static Unit<Amount> Dozen => dozen.Value;
        }

        private class AmountProvider : MeasurementProvider<Amount> {
            public override Amount CreateMeasurement(double value, Unit<Amount> unit) => new Amount(value, unit);

            public override IEnumerable<Operator> ParseOperators {
                get {
                    yield return Operator.CreateDivision<Amount, Volume, Molarity>((x, y) => x.Divide(y));
                    yield return Operator.CreateDivision<Amount, Mass, Molality>((x, y) => x.Divide(y));
                }
            }            

            public override IEnumerable<Unit<Amount>> ParsableUnits {
                get {
                    yield return Units.Mole;
                }
            }
        }
    }

    public static partial class MeasurementExtensions {
        public static Molarity Divide(this IMeasurement<Amount> amount, IMeasurement<Volume> volume) {
            Validate.NonNull(amount, nameof(amount));
            Validate.NonNull(volume, nameof(volume));

            return new Molarity(amount, volume);
        }

        public static Molality Divide(this IMeasurement<Amount> amount, IMeasurement<Mass> mass) {
            Validate.NonNull(amount, nameof(amount));
            Validate.NonNull(mass, nameof(mass));

            return new Molality(amount, mass);
        }
    }
}
