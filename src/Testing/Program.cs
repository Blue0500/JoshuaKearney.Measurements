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
            Term<Angle, DoubleMeasurement> t = new Angle(45, Angle.Units.Degree).MultiplyToTerm(new DoubleMeasurement(90));

            Console.WriteLine(t);
            Console.Read();
        }
    }
}
