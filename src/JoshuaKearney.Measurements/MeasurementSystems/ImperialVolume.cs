using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements.MeasurementSystems {

    public static class ImperialVolume {

        private static Lazy<IEnumerable<Unit<Volume>>> allUnits = new Lazy<IEnumerable<Unit<Volume>>>(() => new List<Unit<Volume>>() {
            FluidOunce, Gill, Pint, Quart, Gallon
        });

        public static IEnumerable<Unit<Volume>> AllUnits => allUnits.Value;

        public static Unit<Volume> FluidOunce { get; } = new Unit<Volume>(
                        name: "imperial fluid ounce",
            symbol: "imp fl oz",
            unitsPerDefault: 1000000 / 28.4130625
        );

        public static Unit<Volume> Gallon { get; } = new Unit<Volume>(
            name: "imperial gallon",
            symbol: "imp gal",
            unitsPerDefault: 1000000 / 4546.09
        );

        public static Unit<Volume> Gill { get; } = new Unit<Volume>(
                        name: "imperial gill",
            symbol: "imp gi",
            unitsPerDefault: 1000000 / 142.0653125
        );

        public static Unit<Volume> Pint { get; } = new Unit<Volume>(
            name: "imperial pint",
            symbol: "imp pt",
            unitsPerDefault: 1000000 / 568.26125
        );

        public static Unit<Volume> Quart { get; } = new Unit<Volume>(
            name: "imperial quart",
            symbol: "imp qt",
            unitsPerDefault: 1000000 / 1136.5225
        );
    }
}