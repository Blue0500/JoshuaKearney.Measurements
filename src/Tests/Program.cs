using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;

namespace Testing {

    public class Program {

        public class Banana { }

        public static void Main(string[] args) {
            Force f = new Mass(6, Pound).Multiply(
                new Distance(10, Foot)
                .Divide(new Time(2, Time.Units.Second))
                .Divide(new Time(10, Time.Units.Second))
            );

            Ratio<Distance, Distance> d = new Ratio<Distance, Distance>(new Distance(5, Foot), new Distance(8, Meter));
            var x = d.SimplifyToDouble();

            Pressure p = f.Divide(new Area(10, Area.Units.CentimeterSquared));

            p.Multiply(new Concentration(500, 800));

            Console.WriteLine(p);
            Console.Read();
        }
    }
}