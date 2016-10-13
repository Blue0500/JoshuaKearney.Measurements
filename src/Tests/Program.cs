using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Distance.Units;

namespace Testing {

    public class Program {

        public class Banana { }

        public static void Main(string[] args) {
            Force f = new Mass(6, Pound).Multiply(
                new Distance(10, Foot)
                .Divide(new Time(2, Time.Units.Second))
                .Divide(new Time(10, Time.Units.Second))    
            );

            Pressure p = f.Divide(new Area(10, Area.Units.CentimeterSquared));

            Console.WriteLine(p);
            Console.Read();
        }
    }
}