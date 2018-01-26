using System;
using JoshuaKearney.Measurements;
using JoshuaKearney.Measurements.Parser;
using JoshuaKearney.Measurements.CsvConverters;

namespace JoshuaKearney.Measurements.Testing {
    internal class Program {
        public static void Main(string[] args) {
            MeasurementParser<Density> parser = new MeasurementParser<Density>(Density.Provider);

            Console.WriteLine(parser.Parse("8 * lb / [(3.1  m)**2  * ft]"));
            Console.Read();
        }
    }
}