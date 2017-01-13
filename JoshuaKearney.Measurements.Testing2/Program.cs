using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Volume.Units;

namespace JoshuaKearney.Measurements.Testing2 {
    public class Program {
        public static void Main(string[] args) {
            MeasurementParser<Area> parse = new MeasurementParser<Area>(Area.Provider);

            Stopwatch w = new Stopwatch();
            w.Start();

            for (int i = 0; i < 10000; i++) {
                parse.Parse("(10 m) ^ 2 + ft^2");
            }

            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds + " ms");

            Console.Read();
        }
    }
}