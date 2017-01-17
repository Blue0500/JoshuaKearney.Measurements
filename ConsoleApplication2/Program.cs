using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements;
using JoshuaKearney.Measurements.Parser;

namespace ConsoleApplication2 {
    class Program {
        static void Main(string[] args) {
            MeasurementParser<DoubleMeasurement> parser = new MeasurementParser<DoubleMeasurement>(DoubleMeasurement.Provider);

            Console.WriteLine(parser.Parse("3 + 2^3 * 6 / (3 - 1)"));
            Console.Read();
        }
    }
}
