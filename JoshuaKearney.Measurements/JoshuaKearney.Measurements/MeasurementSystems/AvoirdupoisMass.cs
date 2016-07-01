using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class AvoirdupoisMass {

        public static IEnumerable<IUnit<Mass>> AllUnits { get; } = new List<IUnit<Mass>>() {
            Dram, Ounce, Pound, Stone, LongHundredWeight, LongTon, ShortHundredWeight, ShortTon
        };

        public static IUnit<Mass> Dram { get; } = Unit.Create<Mass>(
                        name: "dram",
            symbol: "dr",
            unitsPerDefault: 1000 / 1.7718451953125
        );

        public static IUnit<Mass> LongHundredWeight { get; } = Unit.Create<Mass>(
           name: "long hundredweight",
           symbol: "long cwt",
           unitsPerDefault: 1 / 0.45359237 / 112
        );

        public static IUnit<Mass> LongTon { get; } = Unit.Create<Mass>(
           name: "long ton",
           symbol: "long t",
           unitsPerDefault: 1 / 1016.0469088
        );

        public static IUnit<Mass> Ounce { get; } = Unit.Create<Mass>(
                                    name: "ounce",
            symbol: "oz",
            unitsPerDefault: 1000 / 28.349523125
        );

        public static IUnit<Mass> Pound { get; } = Unit.Create<Mass>(
            name: "pound",
            symbol: "lb",
            unitsPerDefault: 1 / 0.45359237
        //aliases: "lbs"
        );

        public static IUnit<Mass> Quarter { get; } = Unit.Create<Mass>(
            name: "quarter",
            symbol: "qr",
            unitsPerDefault: 1 / 6.35029318 / 2
        );

        public static IUnit<Mass> ShortHundredWeight { get; } = Unit.Create<Mass>(
            name: "short hundredweight",
            symbol: "short cwt",
            unitsPerDefault: 1000 / 45.359237
        );

        public static IUnit<Mass> ShortTon { get; } = Unit.Create<Mass>(
            name: "short ton",
            symbol: "short ton",
            unitsPerDefault: 1 / 907.18474
        //aliases: new[] { "ton", "tons" }
        );

        public static IUnit<Mass> Stone { get; } = Unit.Create<Mass>(
                                                name: "stone",
            symbol: "st",
            unitsPerDefault: 1 / 6.35029318
        );
    }
}