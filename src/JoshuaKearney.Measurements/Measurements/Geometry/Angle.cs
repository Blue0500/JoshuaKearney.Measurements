using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JoshuaKearney.Measurements.Angle.Units;

namespace JoshuaKearney.Measurements {

    public class Angle : Measurement<Angle> {
        public static double Sin(Angle a) {
            return Math.Sin(a.ToDouble(Radian));
        }

        public static double Cos(Angle a) {
            return Math.Cos(a.ToDouble(Radian));
        }

        public static double Tan(Angle a) {
            return Math.Tan(a.ToDouble(Radian));
        }

        public static double Sec(Angle a) {
            return 1 / Cos(a);
        }

        public static double Csc(Angle a) {
            return 1 / Sin(a);
        }

        public static double Cot(Angle a) {
            return 1 / Tan(a);
        }

        public static Angle ArcSin(double d) {
            return new Angle(Math.Asin(d), Radian);
        }

        public static Angle ArcCos(double d) {
            return new Angle(Math.Acos(d), Radian);
        }

        public static Angle ArcTan(double d) {
            return new Angle(Math.Atan(d), Radian);
        }

        public static Angle ArcSec(double d) {
            return ArcCos(1 / d);
        }

        public static Angle ArcCsc(double d) {
            return ArcSin(1 / d);
        }

        public static Angle ArcCot(double d) {
            return ArcTan(1 / d);
        }

        public Angle() {
        }

        public Angle(double amount, Unit<Angle> unit) : base(amount, unit) {
        }

        public static IMeasurementProvider<Angle> Provider { get; } = new AngleProvider();

        public override IMeasurementProvider<Angle> MeasurementProvider => Provider;

        public class Units {
            private static Lazy<Unit<Angle>> radian = new Lazy<Unit<Angle>>(() => new Unit<Angle>("radian", "rad", 1));

            public static Unit<Angle> Degree { get; } = new Angle(1, Radian).Multiply(Math.PI).Divide(180).CreateUnit("degree", "°");

            public static Unit<Angle> Gradian { get; } = new Angle(1, Radian).Multiply(Math.PI).Divide(200).CreateUnit("gradian", "grad");

            public static Unit<Angle> Radian => radian.Value;

            public static Unit<Angle> Revolution { get; } = new Angle(1, Radian).Multiply(2 * Math.PI).CreateUnit("revolution", "rev");
        }

        private class AngleProvider : IMeasurementProvider<Angle> {
            public IEnumerable<Unit<Angle>> AllUnits { get; } = new[] { Units.Radian, Units.Degree, Units.Revolution, Units.Gradian };

            public Unit<Angle> DefaultUnit => Radian;

            public Angle CreateMeasurement(double value, Unit<Angle> unit) {
                return new Angle(value, unit);
            }
        }
    }
}