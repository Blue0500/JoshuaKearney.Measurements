using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;

namespace Testing {

    public class Program {

        public class Banana { }

        public static void Main(string[] args) {
            Console.WriteLine(new Pressure(.32, Pressure.Units.Pascal).Reciprocal());
            Console.Read();
        }
    }
}