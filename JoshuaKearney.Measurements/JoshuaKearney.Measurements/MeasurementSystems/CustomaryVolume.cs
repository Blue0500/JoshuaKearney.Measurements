using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class CustomaryVolume {

        public static IEnumerable<IUnit<Volume>> AllUnits { get; } = new List<IUnit<Volume>>() {
                FluidDram, Teaspoon, Tablespoon, FluidOunce, Cup, Pint, Quart, Gallon, Barrel, Hogshead, Peck, Bushel
            };

        public static IUnit<Volume> Barrel { get; } = Unit.Create<Volume>(
            name: "US quart",
            symbol: "US qt",
            unitsPerDefault: 1000 / 119.240471196
        );

        public static IUnit<Volume> Bushel { get; } = Unit.Create<Volume>(
            name: "bushel",
            symbol: "bu",
            unitsPerDefault: 1000 / 35.23907
        );

        public static IUnit<Volume> Cup { get; } = Unit.Create<Volume>(
            name: "US cup",
            symbol: "US cp",
            unitsPerDefault: 1000000 / 236.5882365
        );

        public static IUnit<Volume> FluidDram { get; } = Unit.Create<Volume>(
                                                            name: "US fluid dram",
            symbol: "US fl dr",
            unitsPerDefault: 1000000 / 3.6966911953125
        );

        public static IUnit<Volume> FluidOunce { get; } = Unit.Create<Volume>(
             name: "US fluid ounce",
             symbol: "US fl oz",
             unitsPerDefault: 1000000 / 29.5735295625
         );

        public static IUnit<Volume> Gallon { get; } = Unit.Create<Volume>(
            name: "US quart",
            symbol: "US qt",
            unitsPerDefault: 1000 / 3.785411784
        );

        public static IUnit<Volume> Hogshead { get; } = Unit.Create<Volume>(
             name: "US quart",
             symbol: "US qt",
             unitsPerDefault: 1000 / 238.480942392
         );

        public static IUnit<Volume> Peck { get; } = Unit.Create<Volume>(
            name: "peck",
            symbol: "pk",
            unitsPerDefault: 1000 / 8.809768
        );

        public static IUnit<Volume> Pint { get; } = Unit.Create<Volume>(
            name: "US pint",
            symbol: "US pt",
            unitsPerDefault: 1000000 / 473.176473
        );

        public static IUnit<Volume> Quart { get; } = Unit.Create<Volume>(
            name: "US quart",
            symbol: "US qt",
            unitsPerDefault: 1000 / 0.946352946
        );

        public static IUnit<Volume> Tablespoon { get; } = Unit.Create<Volume>(
            name: "tablespoon",
            symbol: "tbsp",
            unitsPerDefault: 1000000 / 14.78676478125
        );

        public static IUnit<Volume> Teaspoon { get; } = Unit.Create<Volume>(
                                                                                                name: "teaspoon",
            symbol: "tsp",
            unitsPerDefault: 1000000 / 4.92892159375
        );
    }
}