using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class EnglishArea {

        public static Unit<Area> Acre { get; } = new Unit<Area>(
            name: "acre",
            symbol: "acre",
            unitsPerDefault: 1 / 4046.8564224
        );

        private static Lazy<IEnumerable<Unit<Area>>> allUnits = new Lazy<IEnumerable<Unit<Area>>>(() => new List<Unit<Area>>() { Perch, Rood, Acre });
        public static IEnumerable<Unit<Area>> AllUnits => allUnits.Value;

        public static Unit<Area> Perch { get; } = new Unit<Area>(
                                   name: "perch",
           symbol: "perch",
           unitsPerDefault: 1 / 25.29285264
       );

        public static Unit<Area> Rood { get; } = new Unit<Area>(
            name: "rood",
            symbol: "rood",
            unitsPerDefault: 1 / 1011.7141056
        );
    }
}