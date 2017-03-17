using System;
using System.Collections.Generic;
using JoshuaKearney.Measurements.Parser;
using static JoshuaKearney.Measurements.DigitalSize.Units;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double amount, Unit<DigitalSize> unit) : base(amount, unit) {
        }

        public static MeasurementProvider<DigitalSize> Provider { get; } = new DigitalSizeProvider();

        public override MeasurementProvider<DigitalSize> MeasurementProvider => Provider;

        public static class Units {

            private static Lazy<PrefixableUnit<DigitalSize>> bit = new Lazy<PrefixableUnit<DigitalSize>>(() => Octet.Multiply(1d / 8).ToPrefixableUnit("b"));

            private static Lazy<PrefixableUnit<DigitalSize>> sizeByte = new Lazy<PrefixableUnit<DigitalSize>>(() => new PrefixableUnit<DigitalSize>("B", Provider));

            public static Unit<DigitalSize> Exabyte { get; } = Prefix.Exa(Octet);

            public static Unit<DigitalSize> Gigabyte { get; } = Prefix.Giga(Octet);

            public static Unit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(Octet);

            public static Unit<DigitalSize> Megabyte { get; } = Prefix.Mega(Octet);

            public static Unit<DigitalSize> Petabyte { get; } = Prefix.Peta(Octet);

            public static Unit<DigitalSize> Terabyte { get; } = Prefix.Tera(Octet);

            public static PrefixableUnit<DigitalSize> Bit => bit.Value;

            public static PrefixableUnit<DigitalSize> Octet => sizeByte.Value;
        }

        private class DigitalSizeProvider : MeasurementProvider<DigitalSize> {
            public override DigitalSize CreateMeasurement(double value, Unit<DigitalSize> unit) => new DigitalSize(value, unit);

            public override IEnumerable<Unit<DigitalSize>> ParsableUnits { get {
                    yield return Units.Octet;
                    yield return Units.Bit;
                }
            }

            public override IEnumerable<Operator> ParseOperators => Enumerable.Empty<Operator>();
        }
    }
}