using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class EnglishArea {

        public static IUnit<Area> Acre { get; } = Unit.Create<Area>(
            name: "acre",
            symbol: "acre",
            unitsPerDefault: 1 / 4046.8564224
        );

        private static Lazy<IEnumerable<IUnit<Area>>> allUnits = new Lazy<IEnumerable<IUnit<Area>>>(() => new List<IUnit<Area>>() { Perch, Rood, Acre });
        public static IEnumerable<IUnit<Area>> AllUnits => allUnits.Value;

        public static IUnit<Area> Perch { get; } = Unit.Create<Area>(
                                   name: "perch",
           symbol: "perch",
           unitsPerDefault: 1 / 25.29285264
       );

        public static IUnit<Area> Rood { get; } = Unit.Create<Area>(
            name: "rood",
            symbol: "rood",
            unitsPerDefault: 1 / 1011.7141056
        );
    }
}