using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class ImperialVolume {

        public static IEnumerable<IUnit<Volume>> AllUnits { get; } = new List<IUnit<Volume>>() {
                FluidOunce, Gill, Pint, Quart, Gallon
            };

        public static IUnit<Volume> FluidOunce { get; } = Unit.Create<Volume>(
                        name: "imperial fluid ounce",
            symbol: "imp fl oz",
            unitsPerDefault: 1000000 / 28.4130625
        );

        public static IUnit<Volume> Gallon { get; } = Unit.Create<Volume>(
            name: "imperial gallon",
            symbol: "imp gal",
            unitsPerDefault: 1000000 / 4546.09
        );

        public static IUnit<Volume> Gill { get; } = Unit.Create<Volume>(
                        name: "imperial gill",
            symbol: "imp gi",
            unitsPerDefault: 1000000 / 142.0653125
        );

        public static IUnit<Volume> Pint { get; } = Unit.Create<Volume>(
            name: "imperial pint",
            symbol: "imp pt",
            unitsPerDefault: 1000000 / 568.26125
        );

        public static IUnit<Volume> Quart { get; } = Unit.Create<Volume>(
            name: "imperial quart",
            symbol: "imp qt",
            unitsPerDefault: 1000000 / 1136.5225
        );
    }
}