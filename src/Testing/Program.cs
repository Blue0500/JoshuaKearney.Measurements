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
            MeasurementParser<Distance> p = new MeasurementParser<Distance>(Distance.Provider);
            Console.WriteLine(p.Parse("NaN"));
            Console.Read();
        }
    }
}
