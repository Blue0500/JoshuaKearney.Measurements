using System;
using System.Collections.Generic;
using static JoshuaKearney.Measurements.DigitalSize.Units;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double amount, Unit<DigitalSize> unit) : base(amount, unit) {
        }

        public static MeasurementProvider<DigitalSize> Provider { get; } = new DigitalSizeProvider();

        public override MeasurementProvider<DigitalSize> MeasurementProvider => Provider;

        public static class Units {

            private static Lazy<PrefixableUnit<DigitalSize>> bit = new Lazy<PrefixableUnit<DigitalSize>>(() => new PrefixableUnit<DigitalSize>(
                symbol: "bit",
                defaultsPerUnit: 1d / 8,
                provider: Provider
            ));

            private static Lazy<PrefixableUnit<DigitalSize>> sizeByte = new Lazy<PrefixableUnit<DigitalSize>>(() => new PrefixableUnit<DigitalSize>(
                symbol: "B",
                defaultsPerUnit: 1,
                provider: Provider
            ));

            public static Unit<DigitalSize> Exabyte { get; } = Prefix.Exa(Octet);

            public static Unit<DigitalSize> Gigabyte { get; } = Prefix.Giga(Octet);

            public static Unit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(Octet);

            public static Unit<DigitalSize> Megabyte { get; } = Prefix.Mega(Octet);

            public static Unit<DigitalSize> Nybble { get; } = new Unit<DigitalSize>(
                symbol: "Nybble",
                defaultsPerUnit: .5,
                provider: Provider
            );

            public static Unit<DigitalSize> Petabyte { get; } = Prefix.Peta(Octet);

            public static Unit<DigitalSize> Terabyte { get; } = Prefix.Tera(Octet);

            public static PrefixableUnit<DigitalSize> Bit => bit.Value;

            public static PrefixableUnit<DigitalSize> Octet => sizeByte.Value;
        }

        private class DigitalSizeProvider : MeasurementProvider<DigitalSize> {
            protected override Lazy<Unit<DigitalSize>> LazyDefaultUnit { get; } = new Lazy<Unit<DigitalSize>>(() => Octet);

            protected override Lazy<IEnumerable<Unit<DigitalSize>>> LazyParsableUnits { get; } = new Lazy<IEnumerable<Unit<DigitalSize>>>(() => new[] { Octet, Bit, Nybble });

            public override DigitalSize CreateMeasurement(double value, Unit<DigitalSize> unit) => new DigitalSize(value, unit);
        }
    }
}