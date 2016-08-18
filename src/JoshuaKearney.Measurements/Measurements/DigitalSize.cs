using System;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double amount, IUnit<DigitalSize> unit) : base(amount, unit) {
        }

        public static IMeasurementProvider<DigitalSize> Provider { get; } = new DigitalSizeProvider();
        public override IMeasurementProvider<DigitalSize> MeasurementProvider => Provider;

        public static class Units {

            private static Lazy<IPrefixableUnit<DigitalSize>> bit = new Lazy<IPrefixableUnit<DigitalSize>>(() => Unit.CreatePrefixable<DigitalSize>(
                name: "bit",
                symbol: "bit",
                unitsPerDefault: 8
            ));

            private static Lazy<IPrefixableUnit<DigitalSize>> sizeByte = new Lazy<IPrefixableUnit<DigitalSize>>(() => Unit.CreatePrefixable<DigitalSize>(
                name: "byte",
                symbol: "B",
                unitsPerDefault: 1
            ));

            public static IUnit<DigitalSize> Exabyte { get; } = Prefix.Exa(Octet);
            public static IUnit<DigitalSize> Gigabyte { get; } = Prefix.Giga(Octet);
            public static IUnit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(Octet);
            public static IUnit<DigitalSize> Megabyte { get; } = Prefix.Mega(Octet);

            public static IUnit<DigitalSize> Nybble { get; } = Unit.Create<DigitalSize>(
                name: "nybble",
                symbol: "Nybble",
                unitsPerDefault: 2d
            );

            public static IUnit<DigitalSize> Petabyte { get; } = Prefix.Peta(Octet);
            public static IUnit<DigitalSize> Terabyte { get; } = Prefix.Tera(Octet);
            public static IPrefixableUnit<DigitalSize> Bit => bit.Value;
            public static IPrefixableUnit<DigitalSize> Octet => sizeByte.Value;
        }

        private class DigitalSizeProvider : IMeasurementProvider<DigitalSize> {
            public IUnit<DigitalSize> DefaultUnit => Units.Octet;

            public DigitalSize CreateMeasurement(double value, IUnit<DigitalSize> unit) => new DigitalSize(value, unit);
        }
    }
}