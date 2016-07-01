using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class Metric {
        private static Lazy<IEnumerable<IUnit>> allUnits = new Lazy<IEnumerable<IUnit>>(() => new List<IUnit>() { Meter, Gram, Tonne, Liter, Are });
        public static IEnumerable<IUnit> AllUnits => allUnits.Value;

        public static IPrefixableUnit<Area> Are { get; } = Unit.CreatePrefixable<Area>(
            name: "are",
            symbol: "a",
            unitsPerDefault: .01
        );

        public static IPrefixableUnit<Mass> Gram { get; } = Unit.CreatePrefixable<Mass>(
            name: "gram",
            symbol: "g",
            unitsPerDefault: 1000d
        );

        public static IPrefixableUnit<Volume> Liter { get; } = Unit.CreatePrefixable<Volume>(
            name: "liter",
            symbol: "L",
            unitsPerDefault: 1000
        );

        public static IPrefixableUnit<Length> Meter { get; } = Unit.CreatePrefixable<Length>(
                                                            name: "meter",
            symbol: "m",
            unitsPerDefault: 1d
        );

        public static IPrefixableUnit<Mass> Tonne { get; } = Unit.CreatePrefixable<Mass>(
            name: "tonne",
            symbol: "t",
            unitsPerDefault: 1d / 1000d
        );
    }
}