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

        public static Lazy<IMeasurementProvider<Angle>> Provider { get; } = new Lazy<IMeasurementProvider<Angle>>(() => new AngleProvider());

        public override Lazy<IMeasurementProvider<Angle>> MeasurementProvider => Provider;

        public class Units {
            private static Lazy<Unit<Angle>> radian = new Lazy<Unit<Angle>>(() => new Unit<Angle>("rad", 1, Provider));

            private static Lazy<Unit<Angle>> degree = new Lazy<Unit<Angle>>(() => Radian.Multiply(Math.PI / 180).ToUnit("deg"));

            private static Lazy<Unit<Angle>> gradian = new Lazy<Unit<Angle>>(() => Radian.Multiply(Math.PI / 200).ToUnit("grad"));

            private static Lazy<Unit<Angle>> revolution = new Lazy<Unit<Angle>>(() => Radian.Multiply(2 * Math.PI).ToUnit("rev"));


            public static Unit<Angle> Degree => degree.Value;

            public static Unit<Angle> Gradian => gradian.Value;

            public static Unit<Angle> Radian => radian.Value;

            public static Unit<Angle> Revolution => revolution.Value;
        }

        private class AngleProvider : IMeasurementProvider<Angle> {
            private Lazy<IEnumerable<Unit<Angle>>> allUnits = new Lazy<IEnumerable<Unit<Angle>>>(() => new[] { Radian, Degree, Revolution, Gradian });

            public IEnumerable<Unit<Angle>> AllUnits => allUnits.Value;

            public Unit<Angle> DefaultUnit => Radian;

            public Angle CreateMeasurement(double value, Unit<Angle> unit) => new Angle(value, unit);
        }
    }
}