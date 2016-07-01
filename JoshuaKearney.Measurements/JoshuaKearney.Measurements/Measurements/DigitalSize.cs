using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class DigitalSize : Measurement<DigitalSize> {

        public DigitalSize() {
        }

        public DigitalSize(double bytes) : base(bytes) {
        }

        protected override MeasurementInfo Supplier { get; } = new MeasurementInfo(
            instanceSupplier: x => new DigitalSize(x),
            storedUnit: CommonUnits.Byte,
            uniqueUnits: new System.Lazy<IEnumerable<IUnit>>(() => {
                return new[] { CommonUnits.Bit, CommonUnits.Byte, CommonUnits.Nybble };
            })
        );

        public static class CommonUnits {

            public static IPrefixableUnit<DigitalSize> Bit { get; } = Unit.CreatePrefixable<DigitalSize>(
                name: "bit",
                symbol: "bit",
                unitsPerStoredUnit: 8
            );

            public static IPrefixableUnit<DigitalSize> Byte { get; } = Unit.CreatePrefixable<DigitalSize>(
                name: "byte",
                symbol: "B",
                unitsPerStoredUnit: 1
            );

            public static IUnit<DigitalSize> Exabyte { get; } = Prefix.Exa(Byte);

            public static IUnit<DigitalSize> Gigabyte { get; } = Prefix.Giga(Byte);

            public static IUnit<DigitalSize> Kilobyte { get; } = Prefix.Kilo(Byte);

            public static IUnit<DigitalSize> Megabyte { get; } = Prefix.Mega(Byte);

            public static IUnit<DigitalSize> Nybble { get; } = Unit.Create<DigitalSize>(
                                                                name: "nybble",
                symbol: "Nybble",
                unitsPerStored: 2d
            );

            public static IUnit<DigitalSize> Petabyte { get; } = Prefix.Peta(Byte);
            public static IUnit<DigitalSize> Terabyte { get; } = Prefix.Tera(Byte);
        }
    }
}