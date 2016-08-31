using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class AvoirdupoisMass {

        private static Lazy<IEnumerable<Unit<Mass>>> allUnits = new Lazy<IEnumerable<Unit<Mass>>>(() => new List<Unit<Mass>>() {
            Dram, Ounce, Pound, Stone, LongHundredWeight, LongTon, ShortHundredWeight, ShortTon
        });

        public static IEnumerable<Unit<Mass>> AllUnits => allUnits.Value;

        public static Unit<Mass> Dram { get; } = new Unit<Mass>(
                        name: "dram",
            symbol: "dr",
            unitsPerDefault: 1000 / 1.7718451953125
        );

        public static Unit<Mass> LongHundredWeight { get; } = new Unit<Mass>(
           name: "long hundredweight",
           symbol: "long cwt",
           unitsPerDefault: 1 / 0.45359237 / 112
        );

        public static Unit<Mass> LongTon { get; } = new Unit<Mass>(
           name: "long ton",
           symbol: "long t",
           unitsPerDefault: 1 / 1016.0469088
        );

        public static Unit<Mass> Ounce { get; } = new Unit<Mass>(
            name: "ounce",
            symbol: "oz",
            unitsPerDefault: 1000 / 28.349523125
        );

        public static Unit<Mass> Pound { get; } = new Unit<Mass>(
            name: "pound",
            symbol: "lb",
            unitsPerDefault: 1 / 0.45359237
        //aliases: "lbs"
        );

        public static Unit<Mass> Quarter { get; } = new Unit<Mass>(
            name: "quarter",
            symbol: "qr",
            unitsPerDefault: 1 / 6.35029318 / 2
        );

        public static Unit<Mass> ShortHundredWeight { get; } = new Unit<Mass>(
            name: "short hundredweight",
            symbol: "short cwt",
            unitsPerDefault: 1000 / 45.359237
        );

        public static Unit<Mass> ShortTon { get; } = new Unit<Mass>(
            name: "short ton",
            symbol: "short ton",
            unitsPerDefault: 1 / 907.18474
        //aliases: new[] { "ton", "tons" }
        );

        public static Unit<Mass> Stone { get; } = new Unit<Mass>(
                                                name: "stone",
            symbol: "st",
            unitsPerDefault: 1 / 6.35029318
        );
    }
}