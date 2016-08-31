using System;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double amount, Unit<DigitalSize> unit) : base(amount, unit) {
        }

        public static IMeasurementProvider<DigitalSize> Provider { get; } = new DigitalSizeProvider();
        public override IMeasurementProvider<DigitalSize> MeasurementProvider => Provider;

        //public static class Units {

            private static Lazy<PrefixableUnit<DigitalSize>> bit = new Lazy<PrefixableUnit<DigitalSize>>(() => new PrefixableUnit<DigitalSize>(
                name: "bit",
                symbol: "bit",
                unitsPerDefault: 8
            ));

            private static Lazy<PrefixableUnit<DigitalSize>> sizeByte = new Lazy<PrefixableUnit<DigitalSize>>(() => new PrefixableUnit<DigitalSize>(
                name: "byte",
                symbol: "B",
                unitsPerDefault: 1
            ));

            public static Unit<DigitalSize> Exabyte { get; } = Prefix.Exa(Octet);
            public static Unit<DigitalSize> Gigabyte { get; } = Prefix.Giga(Octet);
            public static Unit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(Octet);
            public static Unit<DigitalSize> Megabyte { get; } = Prefix.Mega(Octet);

            public static Unit<DigitalSize> Nybble { get; } = new Unit<DigitalSize>(
                name: "nybble",
                symbol: "Nybble",
                unitsPerDefault: 2d
            );

            public static Unit<DigitalSize> Petabyte { get; } = Prefix.Peta(Octet);
            public static Unit<DigitalSize> Terabyte { get; } = Prefix.Tera(Octet);
            public static PrefixableUnit<DigitalSize> Bit => bit.Value;
            public static PrefixableUnit<DigitalSize> Octet => sizeByte.Value;
        //}

        private class DigitalSizeProvider : IMeasurementProvider<DigitalSize> {
            public Unit<DigitalSize> DefaultUnit => Octet;

            public DigitalSize CreateMeasurement(double value, Unit<DigitalSize> unit) => new DigitalSize(value, unit);
        }
    }
}