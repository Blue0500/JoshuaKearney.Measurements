using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static TNum Simplify<TSelf, TNum, T>(this Ratio<TSelf, TNum, T> measurement)
                where T : NumericMeasurement<T>
                where TSelf : Ratio<TSelf, TNum, T>
                where TNum : Measurement<TNum> {
            return measurement.Simplify((x, y) => x.Divide(y));
        }

        public static double SimplifyToDouble<TSelf, T, E>(this TSelf term)
                where T : NumericMeasurement<T>
                where E : NumericMeasurement<E>
                where TSelf : Term<TSelf, T, E> {
            return term.Simplify((x, y) => x.Divide(y));
        }

        public static TFirst Simplify<TSelf, TFirst, T>(this Term<TSelf, TFirst, T> measurement)
                where T : NumericMeasurement<T>
                where TFirst : Measurement<TFirst>
                where TSelf : Term<TSelf, TFirst, T> {
            return measurement.Simplify((x, y) => x.Multiply(y));
        }

        public static TSecond Simplify<TSelf, TSecond, T>(this Term<TSelf, T, TSecond> measurement)
                where T : NumericMeasurement<T>
                where TSecond : Measurement<TSecond>
                where TSelf : Term<TSelf, T, TSecond> {
            return measurement.Simplify((x, y) => y.Multiply(x));
        }
    }

    public abstract class NumericMeasurement<T> :
        Measurement<T>,
        IDividableMeasurement<Time, Frequency>
        where T : NumericMeasurement<T> {

        public NumericMeasurement() {
        }

        public NumericMeasurement(double d, Unit<T> unit) : base(d, unit) {
        }

        public Frequency Divide(Time measurement2) {
            return new Frequency(this.ToDouble(), measurement2);
        }

        public TThat Multiply<TThat>(TThat measurement2) where TThat : Measurement<TThat> {
            return measurement2.Multiply(this);
        }

        public abstract double ToDouble();

        public static implicit operator double(NumericMeasurement<T> measurement) {
            return measurement.ToDouble();
        }
    }
}