using JoshuaKearney.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing {

    public class Program {

        public static void Main(string[] args) {
            MeasurementRatio<Mass, Area> weight = new MeasurementRatio<Mass, Area>(Mass.FromGrams(320), Area.FromCentimetersSquared(1));

            Console.WriteLine(weight.ToUnits(Mass.Units.Kilogram, Area.Units.MeterSquared));
            Console.Read();
        }
    }
}