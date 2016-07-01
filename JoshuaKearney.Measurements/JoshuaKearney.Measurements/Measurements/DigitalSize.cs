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

            public static IPrefixableUnit<DigitalSize> Bit { get; } = Unit.CreatePrefixable<DigitalSize>(
                name: "bit",
                symbol: "bit",
                unitsPerDefault: 8
            );

            public static IPrefixableUnit<DigitalSize> SizeByte { get; } = Unit.CreatePrefixable<DigitalSize>(
                name: "byte",
                symbol: "B",
                unitsPerDefault: 1
            );

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
            public static IUnit<DigitalSize> Terabyte { get; } = Prefix.Tera(SizeByte);
        }
    }
}