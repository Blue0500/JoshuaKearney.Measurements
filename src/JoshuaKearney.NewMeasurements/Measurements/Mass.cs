using System;
using System.Collections.Generic;
using JoshuaKearney.Measurements.Parser;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass> {

        public static MeasurementProvider<Mass> Provider { get; } = new MassProvider();

        public override MeasurementProvider<Mass> MeasurementProvider => Provider;

        public Mass() {
        }

        public Mass(double amount, Unit<Mass> unit) : base(amount, unit) {
        }

        public static class Units {
            private static Lazy<PrefixableUnit<Mass>> gram = new Lazy<PrefixableUnit<Mass>>(() => new PrefixableUnit<Mass>("g", Provider));

            private static Lazy<Unit<Mass>> kilogram  = new Lazy<Unit<Mass>>(() => Prefix.Kilo(Gram));

            private static Lazy<Unit<Mass>> metricTon = new Lazy<Unit<Mass>>(() => Kilogram.Multiply(1000).ToUnit("t"));

            private static Lazy<Unit<Mass>> milligram = new Lazy<Unit<Mass>>(() => Prefix.Milli(Gram));

            private static Lazy<Unit<Mass>> ounce = new Lazy<Unit<Mass>>(() => Gram.Multiply(28.349523125).ToUnit("oz"));

            private static Lazy<Unit<Mass>> pound = new Lazy<Unit<Mass>>(() => Kilogram.Multiply(0.45359237).ToUnit("lb"));

            public static PrefixableUnit<Mass> Gram => gram.Value;

            public static Unit<Mass> Kilogram => kilogram.Value;

            public static Unit<Mass> MetricTon => metricTon.Value;

            public static Unit<Mass> Milligram => milligram.Value;

            public static Unit<Mass> Ounce => ounce.Value;

            public static Unit<Mass> Pound => pound.Value;
        }

        private class MassProvider : MeasurementProvider<Mass> {
            public override Mass CreateMeasurement(double value, Unit<Mass> unit) => new Mass(value, unit);

            public override IEnumerable<Operator> ParseOperators => base.ParseOperators.Concat(new[] {
                Operator.CreateDivision<Mass, Volume, Density>((x, y) => x.Divide(y))
            });

            public override IEnumerable<Unit<Mass>> ParsableUnits {
                get {
                    yield return Units.Kilogram;
                    yield return Units.Gram;
                    yield return Units.MetricTon;
                    yield return Units.Milligram;
                    yield return Units.Ounce;
                    yield return Units.Pound;
                }
            }
        }
    }

    public static partial class MeasurementExtensions {
        public static Density Divide(this IMeasurement<Mass> mass, IMeasurement<Volume> volume) {
            Validate.NonNull(mass, nameof(mass));
            Validate.NonNull(volume, nameof(volume));

            return new Density(mass, volume);
        }        
    }
}