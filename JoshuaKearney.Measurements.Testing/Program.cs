using JoshuaKearney.Measurements.JsonConverters;
using JoshuaKearney.Measurements.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Volume.Units;

namespace JoshuaKearney.Measurements.Testing {
    public class Program {
        public static void Main(string[] args) {
            MeasurementParser<Ratio<Mass, Area>> p = new MeasurementParser<Ratio<Mass, Area>>(Ratio<Mass, Area>.GetProvider(Mass.Provider, Area.Provider));

            Console.WriteLine(Pound.Divide(16).ToString(Ounce));
            Console.WriteLine(p.Parse("4 oz / yd^2"));

            Density d = Gram.Divide(MeterCubed);

            Console.Read();
        }
    }
}
