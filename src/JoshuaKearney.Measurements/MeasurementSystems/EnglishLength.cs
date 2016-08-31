using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class EnglishLength {

        private static Lazy<IEnumerable<Unit<Length>>> allUnits = new Lazy<IEnumerable<Unit<Length>>>(() => new List<Unit<Length>>() {
            Thou, Inch, Foot, Yard, Chain, Furlong, Mile, League, Fathom, Cable, NauticalMile, Link, Rod
        });

        public static Unit<Length> Cable { get; } = new Unit<Length>(
             name: "cable",
             symbol: "cable",
             unitsPerDefault: 1d / .3048 / 608d
         );

        public static Unit<Length> Chain { get; } = new Unit<Length>(
            name: "chain",
            symbol: "ch",
            unitsPerDefault: 1d / 3d / .3048 / 22d
        );

        public static Unit<Length> Fathom { get; } = new Unit<Length>(
             name: "fathom",
             symbol: "ftm",
             unitsPerDefault: 1d / .3048 / 6.08d
         );

        public static Unit<Length> Foot { get; } = new Unit<Length>(
            name: "foot",
            symbol: "ft",
            unitsPerDefault: 1d / .3048
        );

        public static Unit<Length> Furlong { get; } = new Unit<Length>(
            name: "furlong",
            symbol: "fur",
            unitsPerDefault: 1d / 3d / .3048 / 22d / 10d
        );

        public static Unit<Length> Inch { get; } = new Unit<Length>(
            name: "inch",
            symbol: "in",
            unitsPerDefault: 100d / 2.54
        );

        public static Unit<Length> League { get; } = new Unit<Length>(
            name: "league",
            symbol: "lea",
            unitsPerDefault: 1d / 1609.344 / 3
        );

        public static Unit<Length> Link { get; } = new Unit<Length>(
            name: "link",
            symbol: "link",
            unitsPerDefault: 100d / 2.54 / 7.92
        );

        public static Unit<Length> Mile { get; } = new Unit<Length>(
            name: "mile",
            symbol: "mi",
            unitsPerDefault: 1d / 1609.344
        );

        public static Unit<Length> NauticalMile { get; } = new Unit<Length>(
            name: "nautical mile",
            symbol: "nautical mile",
            unitsPerDefault: 1d / .3048 / 6080d
        );

        public static Unit<Length> Rod { get; } = new Unit<Length>(
            name: "rod",
            symbol: "rod",
            unitsPerDefault: 100 / 2.54 / 7.92 / 25
        );

        public static Unit<Length> Thou { get; } = new Unit<Length>(
            name: "thou",
            symbol: "th",
            unitsPerDefault: 100d / 2.54 * 1000d
        );

        public static Unit<Length> Yard { get; } = new Unit<Length>(
            name: "yard",
            symbol: "yd",
            unitsPerDefault: 1d / 3d / .3048
        );

        public static IEnumerable<Unit<Length>> AllUnits => allUnits.Value;
    }
}