using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.Volume.Units;

public class Program {
    public static void Main(string[] args) {
        MeasurementParser<Volume> p = new MeasurementParser<Volume>(Volume.Provider);
        Console.WriteLine(p.Parse("4 m * ft * mm").ToString(Liter));

        Console.Read();
    }
}