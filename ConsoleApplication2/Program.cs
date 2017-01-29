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
            MeasurementParser<Density> parser = new MeasurementParser<Density>(Density.Provider);

            Console.WriteLine(parser.Parse("8 * lb / [(3.1  m)**2  * ft]"));
            Console.Read();
        }
    }
}
