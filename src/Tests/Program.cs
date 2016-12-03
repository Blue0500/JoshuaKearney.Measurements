using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;


namespace Testing {

    public class Program {

        public static void Main(string[] args) {
            MeasurementParser<Ratio<Mass, Area>> p = new MeasurementParser<Ratio<Mass, Area>>(Ratio<Mass, Area>.GetProvider(Mass.Provider, Area.Provider));

            Measurement<Area> a = new Area();
            a.Divide()

            Console.WriteLine(p.Parse("4.0 kg / 5.0 m^2"));
            Console.Read();
        }
    }
}