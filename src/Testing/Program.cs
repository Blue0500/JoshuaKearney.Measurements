using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;
using JoshuaKearney.Measurements;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            var parse = new MeasurementParser<Density>(Density.Provider);

            JoshuaKearney.Measurements.DoubleMeasurement x = new DoubleMeasurement();

            Console.WriteLine(x);

            Console.WriteLine(parse.Parse("2 kg / (7 in * yd * 29cm)"));
            Console.Read();
        }
    }
}
