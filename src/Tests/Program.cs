using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;

namespace Testing {

    public class Program {

        public class Banana { }

        public static void Main(string[] args) {
            Mass maxWeight = new Mass(45, Kilogram);
            Ratio<Mass, Area> fabricWeight = new Ratio<Mass, Area>(new Mass(45, Pound), new Area(4, Area.Units.MeterSquared));
            Distance inputWidth = new Distance(4, Meter);

            Distance weightBasedDrop = maxWeight.Divide(fabricWeight).Divide(inputWidth);
            Distance weightBasedDrop2 = maxWeight.DivideToRatio(inputWidth).Divide(fabricWeight);


            Ratio<Distance, Mass> v = new Ratio<Distance, Mass>(new Distance(4, Mile), new Mass(4, Kilogram));
            Mass m = v.Selector(x => x.Divide(new Distance(4, Meter)), y => y).Reciprocal().Simplify();


            Console.WriteLine(new Pressure(.32, Pressure.Units.Pascal).Reciprocal());
            Console.Read();
        }
    }
}