using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double amount, Unit<DigitalSize> unit) : base(amount, unit) {
        }

        public static Lazy<IMeasurementProvider<DigitalSize>> Provider { get; } = new Lazy<IMeasurementProvider<DigitalSize>>(() => new DigitalSizeProvider());

        public override Lazy<IMeasurementProvider<DigitalSize>> MeasurementProvider => Provider;

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

        private class DigitalSizeProvider : IMeasurementProvider<DigitalSize> {
            public IEnumerable<Unit<DigitalSize>> AllUnits { get; } = new[] { Units.Bit, Units.Octet };

            public Unit<DigitalSize> DefaultUnit => Units.Octet;

            public DigitalSize CreateMeasurement(double value, Unit<DigitalSize> unit) => new DigitalSize(value, unit);
        }
    }
}