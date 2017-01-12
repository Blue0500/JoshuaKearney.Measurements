using JoshuaKearney.Measurements.NewParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Volume.Units;

namespace JoshuaKearney.Measurements.Testing2 {
    public class Program {
        public static void Main(string[] args) {
            MeasurementParser<Area> parse = new MeasurementParser<Area>(Area.Provider);

            Area result;
            Console.WriteLine(parse.TryParse("3 ((((((m))))))^2", out result));
            Console.WriteLine(result);

            Console.Read();
        }
    }
}