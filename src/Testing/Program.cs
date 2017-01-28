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
            Distance d = Distance.Units.Meter.Multiply(.17213543514321324);

            Console.WriteLine(d.ToString(Distance.Units.Meter, "R"));
            Console.Read();
        }
    }
}
