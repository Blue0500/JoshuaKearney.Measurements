using JoshuaKearney.Measurements;
using JoshuaKearney.Measurements.Parser;
using System;

namespace NewTesting {
    class Program {
        static void Main(string[] args) {
            MeasurementParser<Ratio<Mass, Area>> densityParser = new MeasurementParser<Ratio<Mass, Area>>(Ratio<Mass, Area>.GetProvider(Mass.Provider, Area.Provider));

            Console.WriteLine(densityParser.Parse("4 lb / (3.5 cm^2)"));

            //Console.WriteLine(d);
            Console.Read();
        }
    }
}