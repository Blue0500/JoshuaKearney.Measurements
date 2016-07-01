using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double bytes) : base(bytes) {
        }

        protected override MeasurementInfo Supplier { get; } = new MeasurementInfo(
            instanceCreator: x => new DigitalSize(x),
            defaultUnit: CommonUnits.SizeByte,
            uniqueUnits: new[] { CommonUnits.Bit, CommonUnits.SizeByte, CommonUnits.Nybble }
        );

        public static class CommonUnits {

            private static Lazy<IPrefixableUnit<DigitalSize>> bit = new Lazy<IPrefixableUnit<DigitalSize>>(() => Unit.CreatePrefixable<DigitalSize>(
                name: "bit",
                symbol: "bit",
                unitsPerDefault: 8
            ));

            public static IPrefixableUnit<DigitalSize> Bit => bit.Value;

            public static IUnit<DigitalSize> Exabyte { get; } = Prefix.Exa(SizeByte);

            public static IUnit<DigitalSize> Gigabyte { get; } = Prefix.Giga(SizeByte);

            public static IUnit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(SizeByte);

            public static IUnit<DigitalSize> Megabyte { get; } = Prefix.Mega(SizeByte);

            public static IUnit<DigitalSize> Nybble { get; } = Unit.Create<DigitalSize>(
                name: "nybble",
                symbol: "Nybble",
                unitsPerDefault: 2d
            );

            public static IUnit<DigitalSize> Petabyte { get; } = Prefix.Peta(SizeByte);

            private static Lazy<IPrefixableUnit<DigitalSize>> sizeByte = new Lazy<IPrefixableUnit<DigitalSize>>(() => Unit.CreatePrefixable<DigitalSize>(
                name: "byte",
                symbol: "B",
                unitsPerDefault: 1
            ));

            public static IPrefixableUnit<DigitalSize> SizeByte => sizeByte.Value;
            public static IUnit<DigitalSize> Terabyte { get; } = Prefix.Tera(SizeByte);
        }
    }
}