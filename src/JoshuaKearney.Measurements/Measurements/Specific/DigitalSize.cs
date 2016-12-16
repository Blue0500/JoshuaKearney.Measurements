using System;
using System.Collections.Generic;
using static JoshuaKearney.Measurements.DigitalSize.Units;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double amount, Unit<DigitalSize> unit) : base(amount, unit) {
        }

        public static MeasurementSupplier<DigitalSize> Provider { get; } = new MeasurementSupplier<DigitalSize>((value, unit) => new DigitalSize(value, unit));

        public override MeasurementSupplier<DigitalSize> MeasurementSupplier => Provider;

        public static class Units {

            private static Lazy<PrefixableUnit<DigitalSize>> bit = new Lazy<PrefixableUnit<DigitalSize>>(() => Octet.Multiply(1d / 8).ToPrefixableUnit("b"));

            private static Lazy<PrefixableUnit<DigitalSize>> sizeByte = new Lazy<PrefixableUnit<DigitalSize>>(() => CreatePrefixableUnit("B", Provider));

            public static Unit<DigitalSize> Exabyte { get; } = Prefix.Exa(Octet);

            public static Unit<DigitalSize> Gigabyte { get; } = Prefix.Giga(Octet);

            public static Unit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(Octet);

            public static Unit<DigitalSize> Megabyte { get; } = Prefix.Mega(Octet);

            public static Unit<DigitalSize> Petabyte { get; } = Prefix.Peta(Octet);

            public static Unit<DigitalSize> Terabyte { get; } = Prefix.Tera(Octet);

            public static PrefixableUnit<DigitalSize> Bit => bit.Value;

            public static PrefixableUnit<DigitalSize> Octet => sizeByte.Value;
        }
    }
}