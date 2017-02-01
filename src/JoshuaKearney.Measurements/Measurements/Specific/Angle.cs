using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;
using static JoshuaKearney.Measurements.Angle.Units;

namespace JoshuaKearney.Measurements {

    public class Angle : Measurement<Angle> {
        public static double Sin(Angle a) {
            Validate.NonNull(a, nameof(a));

            return Math.Sin(a.ToDouble(Radian));
        }

        public static double Cos(Angle a) {
            Validate.NonNull(a, nameof(a));

            return Math.Cos(a.ToDouble(Radian));
        }

        public static double Tan(Angle a) {
            Validate.NonNull(a, nameof(a));

            return Math.Tan(a.ToDouble(Radian));
        }

        public static double Sec(Angle a) {
            Validate.NonNull(a, nameof(a));

            return 1 / Cos(a);
        }

        public static double Csc(Angle a) {
            Validate.NonNull(a, nameof(a));

            return 1 / Sin(a);
        }

        public static double Cot(Angle a) {
            Validate.NonNull(a, nameof(a));

            return 1 / Tan(a);
        }

        public static Angle Asin(double d) {
            return new Angle(Math.Asin(d), Radian);
        }

        public static Angle Acos(double d) {
            return new Angle(Math.Acos(d), Radian);
        }

        public static Angle Atan(double d) {
            return new Angle(Math.Atan(d), Radian);
        }

        public static Angle Atan2(double y, double x) {
            return new Angle(Math.Atan2(y, x), Radian);
        }

        public static Angle Asec(double d) {
            return Acos(1 / d);
        }

        public static Angle Acsc(double d) {
            return Asin(1 / d);
        }

        public static Angle Acot(double d) {
            return Atan(1 / d);
        }

        public Angle() {
        }

        public Angle(double amount, Unit<Angle> unit) : base(amount, unit) {
        }

        public static MeasurementProvider<Angle> Provider { get; } = new AngleProvider();

        public override MeasurementProvider<Angle> MeasurementProvider => Provider;

        public Distance GetArcLength(Distance radius) {
            return radius.Multiply(this.ToDouble(Units.Radian));
        }

        public Distance GetRadiusLength(Distance arcLength) {
            return arcLength.Multiply(1 / this.ToDouble(Units.Radian));
        }

        public class Units {
            private static Lazy<PrefixableUnit<Angle>> radian = new Lazy<PrefixableUnit<Angle>>(() => new PrefixableUnit<Angle>("rad", Provider));

            private static Lazy<Unit<Angle>> degree = new Lazy<Unit<Angle>>(() => Radian.Multiply(Math.PI / 180).ToUnit("deg"));

            private static Lazy<Unit<Angle>> gradian = new Lazy<Unit<Angle>>(() => Radian.Multiply(Math.PI / 200).ToUnit("grad"));

            private static Lazy<Unit<Angle>> revolution = new Lazy<Unit<Angle>>(() => Radian.Multiply(2 * Math.PI).ToUnit("rev"));

            private static Lazy<Unit<Angle>> arcMinute = new Lazy<Unit<Angle>>(() => Degree.Divide(60).ToUnit("arcmin"));

            private static Lazy<PrefixableUnit<Angle>> arcSecond = new Lazy<PrefixableUnit<Angle>>(() => ArcMinute.Divide(60).ToPrefixableUnit("arcsec"));

            public static Unit<Angle> Degree => degree.Value;

            public static Unit<Angle> Gradian => gradian.Value;

            public static Unit<Angle> Radian => radian.Value;

            public static Unit<Angle> Revolution => revolution.Value;

            public static Unit<Angle> ArcMinute => arcMinute.Value;

            public static Unit<Angle> ArcSecond => arcSecond.Value;
        }

        private class AngleProvider : MeasurementProvider<Angle> {
            protected override IEnumerable<Unit<Angle>> GetParsableUnits() => new[] { Radian, Degree, Revolution, Gradian, ArcMinute, ArcSecond };

            public override Angle CreateMeasurement(double value, Unit<Angle> unit) => new Angle(value, unit);

            protected override IEnumerable<Operator> GetOperators() => new Operator[0];
        }
    }
}