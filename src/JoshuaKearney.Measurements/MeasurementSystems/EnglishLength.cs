using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class EnglishLength {

        private static Lazy<IEnumerable<Unit<Distance>>> allUnits = new Lazy<IEnumerable<Unit<Distance>>>(() => new List<Unit<Distance>>() {
            Thou, Inch, Foot, Yard, Chain, Furlong, Mile, League, Fathom, Cable, NauticalMile, Link, Rod
        });

        public static Unit<Distance> Cable { get; } = new Unit<Distance>(
             name: "cable",
             symbol: "cable",
             unitsPerDefault: 1d / .3048 / 608d
         );

        public static Unit<Distance> Chain { get; } = new Unit<Distance>(
            name: "chain",
            symbol: "ch",
            unitsPerDefault: 1d / 3d / .3048 / 22d
        );

        public static Unit<Distance> Fathom { get; } = new Unit<Distance>(
             name: "fathom",
             symbol: "ftm",
             unitsPerDefault: 1d / .3048 / 6.08d
         );

        public static Unit<Distance> Foot { get; } = new Unit<Distance>(
            name: "foot",
            symbol: "ft",
            unitsPerDefault: 1d / .3048
        );

        public static Unit<Distance> Furlong { get; } = new Unit<Distance>(
            name: "furlong",
            symbol: "fur",
            unitsPerDefault: 1d / 3d / .3048 / 22d / 10d
        );

        public static Unit<Distance> Inch { get; } = new Unit<Distance>(
            name: "inch",
            symbol: "in",
            unitsPerDefault: 100d / 2.54
        );

        public static Unit<Distance> League { get; } = new Unit<Distance>(
            name: "league",
            symbol: "lea",
            unitsPerDefault: 1d / 1609.344 / 3
        );

        public static Unit<Distance> Link { get; } = new Unit<Distance>(
            name: "link",
            symbol: "link",
            unitsPerDefault: 100d / 2.54 / 7.92
        );

        public static Unit<Distance> Mile { get; } = new Unit<Distance>(
            name: "mile",
            symbol: "mi",
            unitsPerDefault: 1d / 1609.344
        );

        public static Unit<Distance> NauticalMile { get; } = new Unit<Distance>(
            name: "nautical mile",
            symbol: "nautical mile",
            unitsPerDefault: 1d / .3048 / 6080d
        );

        public static Unit<Distance> Rod { get; } = new Unit<Distance>(
            name: "rod",
            symbol: "rod",
            unitsPerDefault: 100 / 2.54 / 7.92 / 25
        );

        public static Unit<Distance> Thou { get; } = new Unit<Distance>(
            name: "thou",
            symbol: "th",
            unitsPerDefault: 100d / 2.54 * 1000d
        );

        public static Unit<Distance> Yard { get; } = new Unit<Distance>(
            name: "yard",
            symbol: "yd",
            unitsPerDefault: 1d / 3d / .3048
        );

        public static IEnumerable<Unit<Distance>> AllUnits => allUnits.Value;
    }
}