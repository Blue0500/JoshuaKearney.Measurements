using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;
using JoshuaKearney.Measurements;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Amount.Units;
using static JoshuaKearney.Measurements.Volume.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Molality.Units;
using static JoshuaKearney.Measurements.Molarity.Units;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            var parse = new MeasurementParser<Distance>(Distance.Provider);

            Meter.Add(Foot);

            Console.WriteLine(parse.Parse("3.4 ft * 6"));

            Console.WriteLine();
            Console.Read();
        }
    }
}
