using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class EnglishLength {

        public static IEnumerable<IUnit<Length>> AllUnits { get; } = new List<IUnit<Length>>() {
            Thou, Inch, Foot, Yard, Chain, Furlong, Mile, League, Fathom, Cable, NauticalMile, Link, Rod
        };

        public static IUnit<Length> Cable { get; } = Unit.Create<Length>(
             name: "cable",
             symbol: "cable",
             unitsPerDefault: 1d / .3048 / 608d
         );

        public static IUnit<Length> Chain { get; } = Unit.Create<Length>(
            name: "chain",
            symbol: "ch",
            unitsPerDefault: 1d / 3d / .3048 / 22d
        );

        public static IUnit<Length> Fathom { get; } = Unit.Create<Length>(
             name: "fathom",
             symbol: "ftm",
             unitsPerDefault: 1d / .3048 / 6.08d
         );

        public static IUnit<Length> Foot { get; } = Unit.Create<Length>(
            name: "foot",
            symbol: "ft",
            unitsPerDefault: 1d / .3048
        );

        public static IUnit<Length> Furlong { get; } = Unit.Create<Length>(
            name: "furlong",
            symbol: "fur",
            unitsPerDefault: 1d / 3d / .3048 / 22d / 10d
        );

        public static IUnit<Length> Inch { get; } = Unit.Create<Length>(
            name: "inch",
            symbol: "in",
            unitsPerDefault: 100d / 2.54
        );

        public static IUnit<Length> League { get; } = Unit.Create<Length>(
            name: "league",
            symbol: "lea",
            unitsPerDefault: 1d / 1609.344 / 3
        );

        public static IUnit<Length> Link { get; } = Unit.Create<Length>(
            name: "link",
            symbol: "link",
            unitsPerDefault: 100d / 2.54 / 7.92
        );

        public static IUnit<Length> Mile { get; } = Unit.Create<Length>(
            name: "mile",
            symbol: "mi",
            unitsPerDefault: 1d / 1609.344
        );

        public static IUnit<Length> NauticalMile { get; } = Unit.Create<Length>(
            name: "nautical mile",
            symbol: "nautical mile",
            unitsPerDefault: 1d / .3048 / 6080d
        );

        public static IUnit<Length> Rod { get; } = Unit.Create<Length>(
            name: "rod",
            symbol: "rod",
            unitsPerDefault: 100 / 2.54 / 7.92 / 25
        );

        public static IUnit<Length> Thou { get; } = Unit.Create<Length>(
            name: "thou",
            symbol: "th",
            unitsPerDefault: 100d / 2.54 * 1000d
        );

        public static IUnit<Length> Yard { get; } = Unit.Create<Length>(
            name: "yard",
            symbol: "yd",
            unitsPerDefault: 1d / 3d / .3048
        );
    }
}