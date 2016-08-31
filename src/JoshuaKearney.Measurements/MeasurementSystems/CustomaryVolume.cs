using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class CustomaryVolume {

        private static Lazy<IEnumerable<Unit<Volume>>> allUnits = new Lazy<IEnumerable<Unit<Volume>>>(() => new List<Unit<Volume>>() {
            FluidDram, Teaspoon, Tablespoon, FluidOunce, Cup, Pint, Quart, Gallon, Barrel, Hogshead, Peck, Bushel
        });

        public static IEnumerable<Unit<Volume>> AllUnits => allUnits.Value;

        public static Unit<Volume> Barrel { get; } = new Unit<Volume>(
            name: "US quart",
            symbol: "US qt",
            unitsPerDefault: 1000 / 119.240471196
        );

        public static Unit<Volume> Bushel { get; } = new Unit<Volume>(
            name: "bushel",
            symbol: "bu",
            unitsPerDefault: 1000 / 35.23907
        );

        public static Unit<Volume> Cup { get; } = new Unit<Volume>(
            name: "US cup",
            symbol: "US cp",
            unitsPerDefault: 1000000 / 236.5882365
        );

        public static Unit<Volume> FluidDram { get; } = new Unit<Volume>(
                                                            name: "US fluid dram",
            symbol: "US fl dr",
            unitsPerDefault: 1000000 / 3.6966911953125
        );

        public static Unit<Volume> FluidOunce { get; } = new Unit<Volume>(
             name: "US fluid ounce",
             symbol: "US fl oz",
             unitsPerDefault: 1000000 / 29.5735295625
         );

        public static Unit<Volume> Gallon { get; } = new Unit<Volume>(
            name: "US quart",
            symbol: "US qt",
            unitsPerDefault: 1000 / 3.785411784
        );

        public static Unit<Volume> Hogshead { get; } = new Unit<Volume>(
             name: "US quart",
             symbol: "US qt",
             unitsPerDefault: 1000 / 238.480942392
         );

        public static Unit<Volume> Peck { get; } = new Unit<Volume>(
            name: "peck",
            symbol: "pk",
            unitsPerDefault: 1000 / 8.809768
        );

        public static Unit<Volume> Pint { get; } = new Unit<Volume>(
            name: "US pint",
            symbol: "US pt",
            unitsPerDefault: 1000000 / 473.176473
        );

        public static Unit<Volume> Quart { get; } = new Unit<Volume>(
            name: "US quart",
            symbol: "US qt",
            unitsPerDefault: 1000 / 0.946352946
        );

        public static Unit<Volume> Tablespoon { get; } = new Unit<Volume>(
            name: "tablespoon",
            symbol: "tbsp",
            unitsPerDefault: 1000000 / 14.78676478125
        );

        public static Unit<Volume> Teaspoon { get; } = new Unit<Volume>(
                                                                                                name: "teaspoon",
            symbol: "tsp",
            unitsPerDefault: 1000000 / 4.92892159375
        );
    }
}