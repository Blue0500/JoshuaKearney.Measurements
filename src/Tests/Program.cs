using JoshuaKearney.Measurements;
using System;
using static JoshuaKearney.Measurements.DigitalSize.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Time.Units;
using static JoshuaKearney.Measurements.Volume.Units;

namespace Testing {

    public class Program {

        public class Banana { }

        public static void Main(string[] args) {
            // Ratio<Mass, Volume> r = new Density(45, Mass.Units.Gram, Volume.Units.InchCubed);
            // Distance d = new Distance();

            Term<Mass, Volume> arialDensity = new Term<Mass, Volume>(new Mass(45, Kilogram), new Volume(4, MeterCubed));
            Console.WriteLine(arialDensity);

            Ratio<DigitalSize, Time> downloadSpeed = new Ratio<DigitalSize, Time>(new DigitalSize(2.1, Prefix.Giga(Bit)), new Time(1, Second));
            Console.WriteLine(downloadSpeed);

            Label<Banana> bananas = new Label<Banana>(4, Label.Units.Dozen);
            Console.WriteLine(bananas);

            //Class1 c = new Class1();
            //c.
            // Mole m = new Mole(45);

            //Concentration c = .45;
            // Console.WriteLine(c.ToString());

            //Quantity<string> strings = new Quantity<string>(45);

            //Ratio<Quantity<int>, Mass> a;

            //Console.WriteLine(strings);

            //Density d = new Density(2.70, Density.Units.GramsPerCentimeterCubed);
            //Distance width = new Distance(15, Distance.Units.Inch);
            //Distance length = new Distance(6, Distance.Units.Inch);

            //Console.WriteLine(new Mass(8.455, Mass.Units.Gram).Divide(d).Divide(width).Divide(length).ToDouble(Distance.Units.Inch));

            //Speed s = new Speed(45, Distance.Units.Meter, Time.Units.Second);
            //Acceleration a = new Acceleration(s, new Time(7, Time.Units.Second));

            ////Pressure pa = new Pressure(4, Pressure.Units.Bar);
            //Stopwatch w = new Stopwatch();
            //w.Start();

            //var p = new MeasurementParser<Force>();
            //Force a = p.Parse("9m  / s / s * 4 kg");

            //w.Stop();
            //Console.WriteLine(a + " " + w.ElapsedMilliseconds + " ms");

            //w.Restart();

            //p = new MeasurementParser<Force>();
            //a = p.Parse("9m  / s / s * 4 kg");

            //w.Stop();
            //Console.WriteLine(a + " " + w.ElapsedMilliseconds + " ms");

            ////Console.WriteLine(new Mass(1, Mass.Pound).Multiply(Acceleration.Gravity));

            ////Console.WriteLine(new Force(1, Force.Newton).ToString(Force.PoundForce));

            ////Math.

            //// Measurement.Factory.
            //// Console.WriteLine(Term.From((Length)null, DigitalSize.From(7, Bit)));
            ////Stopwatch w = new Stopwatch();
            // w.Start();

            //Console.WriteLine(Length.Parse("45 km"));

            //w.Stop();
            // Console.WriteLine($"{ret} in {w.ElapsedMilliseconds} ms");

            // w.Restart();

            // ret = Length.Parse("45 km");

            // w.Stop();
            //Console.WriteLine($"{ret} in {w.ElapsedMilliseconds} ms");
            ////var x = Volume.Parse("4 m*mi*yd");
            //Console.WriteLine(Volume.Parse("4 m*mi*yd"));

            ////var x = Ratio.Parse<Mass, Area>("45 g/km^2");
            //Console.WriteLine(Ratio.Parse<Mass, Area>("45 Mg/km^2"));
            //Console.WriteLine(Term.Parse<Term<Length, DigitalSize>, Mass>("99 (Mm*KiB)*mg"));
            ////var y = 4;

            //Console.WriteLine(Ratio.From(Term.From(Length.From(4, Meter), Mass.From(89, Gram)), DigitalSize.From(78, Bit)));
            //Console.WriteLine(x);
            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds + " ms");

            //w.Restart();
            //Console.WriteLine(Volume.Parse("4 m*mi*yd"));

            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds + " ms");
            //Stopwatch w = new Stopwatch();
            //w.Start();

            //var res = Volume.Parse("45 km*Km*fur");

            //w.Stop();
            //Console.WriteLine(res + "\n" + w.ElapsedMilliseconds + " ms");
            //w.Reset();

            //w.Start();
            //var res2 = Volume.Parse("45 km*Km*fur");
            //w.Stop();

            //Console.WriteLine(res2 + "\n" + w.ElapsedMilliseconds + " ms");

            Console.Read();
        }
    }
}