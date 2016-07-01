using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public static class MeasurementSystems {

        public static class AvoirdupoisMass {

            public static IEnumerable<IUnit<Mass>> AllUnits { get; } = new List<IUnit<Mass>>() {
                Dram, Ounce, Pound, Stone, LongHundredWeight, LongTon, ShortHundredWeight, ShortTon
            };

            public static IUnit<Mass> Dram { get; } = Unit.Create<Mass>(
                            name: "dram",
                symbol: "dr",
                unitsPerStored: 1000 / 1.7718451953125
            );

            public static IUnit<Mass> LongHundredWeight { get; } = Unit.Create<Mass>(
               name: "long hundredweight",
               symbol: "long cwt",
               unitsPerStored: 1 / 0.45359237 / 112
            );

            public static IUnit<Mass> LongTon { get; } = Unit.Create<Mass>(
               name: "long ton",
               symbol: "long t",
               unitsPerStored: 1 / 1016.0469088
            );

            public static IUnit<Mass> Ounce { get; } = Unit.Create<Mass>(
                                        name: "ounce",
                symbol: "oz",
                unitsPerStored: 1000 / 28.349523125
            );

            public static IUnit<Mass> Pound { get; } = Unit.Create<Mass>(
                name: "pound",
                symbol: "lb",
                unitsPerStored: 1 / 0.45359237
            //aliases: "lbs"
            );

            public static IUnit<Mass> Quarter { get; } = Unit.Create<Mass>(
                name: "quarter",
                symbol: "qr",
                unitsPerStored: 1 / 6.35029318 / 2
            );

            public static IUnit<Mass> ShortHundredWeight { get; } = Unit.Create<Mass>(
                name: "short hundredweight",
                symbol: "short cwt",
                unitsPerStored: 1000 / 45.359237
            );

            public static IUnit<Mass> ShortTon { get; } = Unit.Create<Mass>(
                name: "short ton",
                symbol: "short ton",
                unitsPerStored: 1 / 907.18474
            //aliases: new[] { "ton", "tons" }
            );

            public static IUnit<Mass> Stone { get; } = Unit.Create<Mass>(
                                                    name: "stone",
                symbol: "st",
                unitsPerStored: 1 / 6.35029318
            );
        }

        public static class CustomaryVolume {

            public static IEnumerable<IUnit<Volume>> AllUnits { get; } = new List<IUnit<Volume>>() {
                FluidDram, Teaspoon, Tablespoon, FluidOunce, Cup, Pint, Quart, Gallon, Barrel, Hogshead, Peck, Bushel
            };

            public static IUnit<Volume> Barrel { get; } = Unit.Create<Volume>(
                name: "US quart",
                symbol: "US qt",
                unitsPerStored: 1000 / 119.240471196
            );

            public static IUnit<Volume> Bushel { get; } = Unit.Create<Volume>(
                name: "bushel",
                symbol: "bu",
                unitsPerStored: 1000 / 35.23907
            );

            public static IUnit<Volume> Cup { get; } = Unit.Create<Volume>(
                name: "US cup",
                symbol: "US cp",
                unitsPerStored: 1000000 / 236.5882365
            );

            public static IUnit<Volume> FluidDram { get; } = Unit.Create<Volume>(
                                                                name: "US fluid dram",
                symbol: "US fl dr",
                unitsPerStored: 1000000 / 3.6966911953125
            );

            public static IUnit<Volume> FluidOunce { get; } = Unit.Create<Volume>(
                 name: "US fluid ounce",
                 symbol: "US fl oz",
                 unitsPerStored: 1000000 / 29.5735295625
             );

            public static IUnit<Volume> Gallon { get; } = Unit.Create<Volume>(
                name: "US quart",
                symbol: "US qt",
                unitsPerStored: 1000 / 3.785411784
            );

            public static IUnit<Volume> Hogshead { get; } = Unit.Create<Volume>(
                 name: "US quart",
                 symbol: "US qt",
                 unitsPerStored: 1000 / 238.480942392
             );

            public static IUnit<Volume> Peck { get; } = Unit.Create<Volume>(
                name: "peck",
                symbol: "pk",
                unitsPerStored: 1000 / 8.809768
            );

            public static IUnit<Volume> Pint { get; } = Unit.Create<Volume>(
                name: "US pint",
                symbol: "US pt",
                unitsPerStored: 1000000 / 473.176473
            );

            public static IUnit<Volume> Quart { get; } = Unit.Create<Volume>(
                name: "US quart",
                symbol: "US qt",
                unitsPerStored: 1000 / 0.946352946
            );

            public static IUnit<Volume> Tablespoon { get; } = Unit.Create<Volume>(
                name: "tablespoon",
                symbol: "tbsp",
                unitsPerStored: 1000000 / 14.78676478125
            );

            public static IUnit<Volume> Teaspoon { get; } = Unit.Create<Volume>(
                                                                                                    name: "teaspoon",
                symbol: "tsp",
                unitsPerStored: 1000000 / 4.92892159375
            );
        }

        public static class EnglishArea {

            public static IUnit<Area> Acre { get; } = Unit.Create<Area>(
                name: "acre",
                symbol: "acre",
                unitsPerStored: 1 / 4046.8564224
            );

            public static IEnumerable<IUnit<Area>> AllUnits { get; } = new List<IUnit<Area>>() { Perch, Rood, Acre };

            public static IUnit<Area> Perch { get; } = Unit.Create<Area>(
                                       name: "perch",
               symbol: "perch",
               unitsPerStored: 1 / 25.29285264
           );

            public static IUnit<Area> Rood { get; } = Unit.Create<Area>(
                name: "rood",
                symbol: "rood",
                unitsPerStored: 1 / 1011.7141056
            );
        }

        public static class EnglishLength {

            public static IEnumerable<IUnit<Length>> AllUnits { get; } = new List<IUnit<Length>>() {
                Thou, Inch, Foot, Yard, Chain, Furlong, Mile, League, Fathom, Cable, NauticalMile, Link, Rod
            };

            public static IUnit<Length> Cable { get; } = Unit.Create<Length>(
                 name: "cable",
                 symbol: "cable",
                 unitsPerStored: 1d / .3048 / 608d
             );

            public static IUnit<Length> Chain { get; } = Unit.Create<Length>(
                name: "chain",
                symbol: "ch",
                unitsPerStored: 1d / 3d / .3048 / 22d
            );

            public static IUnit<Length> Fathom { get; } = Unit.Create<Length>(
                 name: "fathom",
                 symbol: "ftm",
                 unitsPerStored: 1d / .3048 / 6.08d
             );

            public static IUnit<Length> Foot { get; } = Unit.Create<Length>(
                name: "foot",
                symbol: "ft",
                unitsPerStored: 1d / .3048
            );

            public static IUnit<Length> Furlong { get; } = Unit.Create<Length>(
                name: "furlong",
                symbol: "fur",
                unitsPerStored: 1d / 3d / .3048 / 22d / 10d
            );

            public static IUnit<Length> Inch { get; } = Unit.Create<Length>(
                name: "inch",
                symbol: "in",
                unitsPerStored: 100d / 2.54
            );

            public static IUnit<Length> League { get; } = Unit.Create<Length>(
                name: "league",
                symbol: "lea",
                unitsPerStored: 1d / 1609.344 / 3
            );

            public static IUnit<Length> Link { get; } = Unit.Create<Length>(
                name: "link",
                symbol: "link",
                unitsPerStored: 100d / 2.54 / 7.92
            );

            public static IUnit<Length> Mile { get; } = Unit.Create<Length>(
                name: "mile",
                symbol: "mi",
                unitsPerStored: 1d / 1609.344
            );

            public static IUnit<Length> NauticalMile { get; } = Unit.Create<Length>(
                name: "nautical mile",
                symbol: "nautical mile",
                unitsPerStored: 1d / .3048 / 6080d
            );

            public static IUnit<Length> Rod { get; } = Unit.Create<Length>(
                name: "rod",
                symbol: "rod",
                unitsPerStored: 100 / 2.54 / 7.92 / 25
            );

            public static IUnit<Length> Thou { get; } = Unit.Create<Length>(
                                                                                                                                                                name: "thou",
                symbol: "th",
                unitsPerStored: 100d / 2.54 * 1000d
            );

            public static IUnit<Length> Yard { get; } = Unit.Create<Length>(
                name: "yard",
                symbol: "yd",
                unitsPerStored: 1d / 3d / .3048
            );
        }

        public static class ImperialVolume {

            public static IEnumerable<IUnit<Volume>> AllUnits { get; } = new List<IUnit<Volume>>() {
                FluidOunce, Gill, Pint, Quart, Gallon
            };

            public static IUnit<Volume> FluidOunce { get; } = Unit.Create<Volume>(
                            name: "imperial fluid ounce",
                symbol: "imp fl oz",
                unitsPerStored: 1000000 / 28.4130625
            );

            public static IUnit<Volume> Gallon { get; } = Unit.Create<Volume>(
                name: "imperial gallon",
                symbol: "imp gal",
                unitsPerStored: 1000000 / 4546.09
            );

            public static IUnit<Volume> Gill { get; } = Unit.Create<Volume>(
                            name: "imperial gill",
                symbol: "imp gi",
                unitsPerStored: 1000000 / 142.0653125
            );

            public static IUnit<Volume> Pint { get; } = Unit.Create<Volume>(
                name: "imperial pint",
                symbol: "imp pt",
                unitsPerStored: 1000000 / 568.26125
            );

            public static IUnit<Volume> Quart { get; } = Unit.Create<Volume>(
                name: "imperial quart",
                symbol: "imp qt",
                unitsPerStored: 1000000 / 1136.5225
            );
        }

        public static class Metric {
            public static IEnumerable<IUnit> AllUnits { get; } = new List<IUnit>() { Meter, Gram, Tonne, Liter, Are };

            public static IPrefixableUnit<Area> Are { get; } = Unit.CreatePrefixable<Area>(
                name: "are",
                symbol: "a",
                unitsPerStoredUnit: .01
            );

            public static IPrefixableUnit<Mass> Gram { get; } = Unit.CreatePrefixable<Mass>(
                name: "gram",
                symbol: "g",
                unitsPerStoredUnit: 1000d
            );

            public static IPrefixableUnit<Volume> Liter { get; } = Unit.CreatePrefixable<Volume>(
                name: "liter",
                symbol: "L",
                unitsPerStoredUnit: 1000
            );

            public static IPrefixableUnit<Length> Meter { get; } = Unit.CreatePrefixable<Length>(
                                                                name: "meter",
                symbol: "m",
                unitsPerStoredUnit: 1d
            );

            public static IPrefixableUnit<Mass> Tonne { get; } = Unit.CreatePrefixable<Mass>(
                name: "tonne",
                symbol: "t",
                unitsPerStoredUnit: 1d / 1000d
            );
        }
    }
}