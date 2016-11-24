using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;

namespace Testing {

    public class Program {

        public class Banana { }

        public static void Main(string[] args) {
            Angle a = new Angle(50, Angle.Units.Revolution);
            Vector2d v = new Vector2d(5, 7);

            Frequency f = new Ratio<DoubleMeasurement, Time>(4, new Time(6, Time.Units.Second));

            Console.WriteLine(a);
            Console.WriteLine(Angle.Cos(a));

            Console.Read();
        }
    }
}