using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;


namespace Testing {

    public class Program {

        public static void Main(string[] args) {
            MeasurementParser<Ratio<Mass, Area>> p = new MeasurementParser<Ratio<Mass, Area>>(Ratio<Mass, Area>.GetProvider(Mass.Provider, Area.Provider));

            Console.WriteLine(p.Parse("4.0m * 5.0.2"));
            Console.Read();
        }
    }
}