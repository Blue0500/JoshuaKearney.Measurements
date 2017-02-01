using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;
using JoshuaKearney.Measurements;
using static JoshuaKearney.Measurements.Amount.Units;
using static JoshuaKearney.Measurements.Volume.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Molality.Units;
using static JoshuaKearney.Measurements.Molarity.Units;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            var parse = new MeasurementParser<Distance>(Distance.Provider);

            Console.WriteLine(Mol);
            Console.Read();
        }
    }
}
