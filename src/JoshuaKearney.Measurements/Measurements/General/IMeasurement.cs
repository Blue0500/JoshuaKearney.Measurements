using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    public interface IMeasurement<T> where T : IMeasurement<T> {
        MeasurementSupplier<T> MeasurementSupplier { get; }

        double ToDouble(Unit<T> unit);
    }

    public static partial class MeasurementExtensions {
        public static string ToString<TSelf>(this IMeasurement<TSelf> self, Unit<TSelf> unit1, params Unit<TSelf>[] units)
            where TSelf : IMeasurement<TSelf> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(units, nameof(units));

            units = units.Concat(new[] { unit1 }).OrderBy(x => self.ToDouble(x)).ToArray();
            var unit = units.FirstOrDefault(x => self.ToDouble(x) >= 1) ?? units.FirstOrDefault() ?? self.MeasurementSupplier.ParsableUnits.FirstOrDefault();

            return self.ToString(unit, "0.##");
        }

        public static T IfNaN<T>(this IMeasurement<T> self, IMeasurement<T> alternate)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(alternate, nameof(alternate));

            if (Measurement<T>.IsNan(self)) {
                return alternate.ToMeasurement();
            }
            else {
                return self.ToMeasurement();
            }
        }

        public static string ToString<T>(this IMeasurement<T> self, Unit<T> unit, string format)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(format, nameof(format));

            string unitStr = Measurement<T>.IsInfinity(self) || Measurement<T>.IsNan(self)
                ? ""
                : " " + unit.ToString();

            return (self.ToDouble(unit).ToString(format) + unitStr).Trim();
        }

        public static T ToMeasurement<T>(this IMeasurement<T> self)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));

            if (self is T) {
                return (T)self;
            }
            else {
                return self.MeasurementSupplier.CreateMeasurement(
                    self.ToDouble(self.MeasurementSupplier.DefaultUnit),
                    self.MeasurementSupplier.DefaultUnit
                );
            }
        }

        public static Ratio<T, DoubleMeasurement> ToRatio<T>(this IMeasurement<T> self)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));

            return new Ratio<T, DoubleMeasurement>(self, new DoubleMeasurement(1));
        }

        public static PrefixableUnit<T> ToPrefixableUnit<T>(this IMeasurement<T> self, string symbol)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(symbol, nameof(symbol));

            return new PrefixableUnit<T>(
                symbol, self.ToDouble(self.MeasurementSupplier.DefaultUnit),
                self.MeasurementSupplier
            );
        }

        public static Unit<T> ToUnit<T>(this IMeasurement<T> self, string symbol)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(symbol, nameof(symbol));

            return new Unit<T>(
                symbol, 
                self.ToDouble(self.MeasurementSupplier.DefaultUnit), 
                self.MeasurementSupplier
            );
        }

        public static T Subtract<T>(this IMeasurement<T> self, IMeasurement<T> that)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(that, nameof(that));

            return self.MeasurementSupplier.CreateMeasurement(
                self.ToDouble(self.MeasurementSupplier.DefaultUnit) - that.ToDouble(self.MeasurementSupplier.DefaultUnit),
                self.MeasurementSupplier.DefaultUnit
            );
        }

        public static T Negate<T>(this IMeasurement<T> self)
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));

            return self.MeasurementSupplier.CreateMeasurement(
                -self.ToDouble(self.MeasurementSupplier.DefaultUnit),
                self.MeasurementSupplier.DefaultUnit
            );
        }

        public static TRatioNum Multiply<TSelf, TRatioSelf, TRatioNum>(this IMeasurement<TSelf> self, IMeasurement<TRatioSelf> ratio)
            where TSelf : IMeasurement<TSelf>
            where TRatioSelf : Ratio<TRatioSelf, TRatioNum, TSelf>
            where TRatioNum : IMeasurement<TRatioNum> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(ratio, nameof(ratio));

            return ratio.Multiply(self);
        }

        public static T Multiply<T>(this IMeasurement<T> self, double factor) 
            where T : IMeasurement<T> {
            Validate.NonNull(self, nameof(self));

            return self.MeasurementSupplier.CreateMeasurement(
                self.ToDouble(self.MeasurementSupplier.DefaultUnit) * factor,
                self.MeasurementSupplier.DefaultUnit
            );
        }

        public static T Divide<T>(this IMeasurement<T> self, double factor) where T : IMeasurement<T> {
            return self.MeasurementSupplier.CreateMeasurement(
                self.ToDouble(self.MeasurementSupplier.DefaultUnit) / factor,
                self.MeasurementSupplier.DefaultUnit
            );
        }

        public static DoubleMeasurement Divide<T>(this IMeasurement<T> self, IMeasurement<T> that) 
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(that, nameof(that));

            return self.ToDouble(self.MeasurementSupplier.DefaultUnit) / that.ToDouble(that.MeasurementSupplier.DefaultUnit);
        }

        public static E Divide<TSelf, E, F>(this IMeasurement<TSelf> self, Ratio<F, TSelf, E> ratio)
            where TSelf : IMeasurement<TSelf>
            where F : Ratio<F, TSelf, E>
            where E : IMeasurement<E> {

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(ratio, nameof(ratio));

            return ratio.Reciprocal().Multiply(self);
        }

        public static T Add<T>(this IMeasurement<T> self, IMeasurement<T> that)
            where T : IMeasurement<T> {

            Validate.NonNull(that, nameof(that));

            return self.MeasurementSupplier.CreateMeasurement(
                self.ToDouble(self.MeasurementSupplier.DefaultUnit) + that.ToDouble(self.MeasurementSupplier.DefaultUnit),
                self.MeasurementSupplier.DefaultUnit
            );
        }

        public static T Abs<T>(this IMeasurement<T> self) 
            where T : IMeasurement<T> {

            Validate.NonNull(self, nameof(self));
            return self.MeasurementSupplier.CreateMeasurement(
                Math.Abs(self.ToDouble(self.MeasurementSupplier.DefaultUnit)), 
                self.MeasurementSupplier.DefaultUnit
            );
        }

        public static Ratio<TSelf, E> DivideToRatio<TSelf, E>(this IMeasurement<TSelf> self, IMeasurement<E> that)
            where TSelf : IMeasurement<TSelf>
            where E : IMeasurement<E> {


            Validate.NonNull(that, nameof(that));
            return new Ratio<TSelf, E>(self, that);
        }

        public static Term<TSelf, E> MultiplyToTerm<TSelf, E>(this IMeasurement<TSelf> self, IMeasurement<E> that)
            where TSelf : class, IMeasurement<TSelf>
            where E : class, IMeasurement<E> {
            Validate.NonNull(that, nameof(that));

            return new Term<TSelf, E>(self, that);
        }
    }
}