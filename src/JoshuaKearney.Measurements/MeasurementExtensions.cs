using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    public static partial class MeasurementExtensions {        
        public static Unit<T> ToUnit<T>(this IMeasurement<T> measurement, string symbol) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(symbol, nameof(symbol));

            var ret = new Unit<T>(
                symbol, 
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit), 
                measurement.MeasurementProvider
            );

            return ret;
        }

        public static PrefixableUnit<T> ToPrefixableUnit<T>(this IMeasurement<T> measurement, string symbol) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(symbol, nameof(symbol));

            return new PrefixableUnit<T>(
                symbol,
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit), 
                measurement.MeasurementProvider
            );
        }

        public static T ToMeasurement<T>(this IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            if (measurement is T t) {
                return t;
            }

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Returns the absolute value of this instance
        /// </summary>
        /// <returns></returns>
        public static T Abs<T>(this IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.MeasurementProvider.CreateMeasurement(
                Math.Abs(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit)),
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Adds this instance to the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static T Add<T>(this IMeasurement<T> measurement, IMeasurement<T> that) where T : IAddableMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(that, nameof(that));

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) + 
                that.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Divides this instance by the specified ratio.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="ratio">The ratio.</param>
        /// <returns></returns>
        public static E Divide<T, E, F>(this IMeasurement<T> measurement, Ratio<F, T, E> ratio)
                where T : IMeasurement<T>
                where F : Ratio<F, T, E>
                where E : IMeasurement<E> {

            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(ratio, nameof(ratio));

            return ratio.Reciprocal().Multiply(measurement);
        }

        /// <summary>
        /// Divides this instance by the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static DoubleMeasurement Divide<T>(this IMeasurement<T> measurement, IMeasurement<T> that) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(that, nameof(that));

            return
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) / 
                that.ToDouble(measurement.MeasurementProvider.DefaultUnit);
        }

        /// <summary>
        /// Divides this instance by the specified double.
        /// </summary>
        /// <param name="factor">The factor.</param>
        /// <returns></returns>
        public static T Divide<T>(this IMeasurement<T> measurement, double factor) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) / factor,
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Divides this instance by another type of measurement to create a ratio.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public static Ratio<T, E> DivideToRatio<T, E>(this IMeasurement<T> measurement, IMeasurement<E> that)
                where T : IMeasurement<T>
                where E : IMeasurement<E> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(that, nameof(that));

            return new Ratio<T, E>(measurement, that);
        }

        /// <summary>
        /// Multiplies this instance by the specified double.
        /// </summary>
        /// <param name="factor">The double to multiply by.</param>
        /// <returns></returns>
        public static T Multiply<T>(this IMeasurement<T> measurement, double factor) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) * factor,
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        public static TNum Multiply<T, TNum, TRatio>(this IMeasurement<T> measurement, Ratio<TRatio, TNum, T> ratio)
            where T : IMeasurement<T>
            where TRatio : Ratio<TRatio, TNum, T>
            where TNum : IMeasurement<TNum> {

            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(ratio, nameof(ratio));

            return ratio.Multiply(measurement);
        }

        /// <summary>
        /// Multiplies this instance by another type of measurement, creating a term
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="that">The that.</param>
        /// <returns></returns>
        public static Term<T, E> MultiplyToTerm<T, E>(this IMeasurement<T> measurement, IMeasurement<E> that)
            where T : IMeasurement<T>
            where E : IMeasurement<E> {

            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(that, nameof(that));

            return new Term<T, E>(measurement, that);
        }

        /// <summary>
        /// Negates this instance.
        /// </summary>
        /// <returns></returns>
        public static T Negate<T>(this IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.Multiply(-1);
        }

        /// <summary>
        /// Subtracts the this instance by another measurement
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static T Subtract<T>(this IMeasurement<T> measurement, IMeasurement<T> that) where T : IAddableMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(that, nameof(that));

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) - that.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        public static Ratio<T, DoubleMeasurement> ToRatio<T>(this IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            return new Ratio<T, DoubleMeasurement>(measurement, new DoubleMeasurement(1));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString<T>(this IMeasurement<T> measurement, Unit<T> unit, string format) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(format, nameof(format));

            string unitStr = Measurement<T>.IsInfinity(measurement) || Measurement<T>.IsNan(measurement)
                ? ""
                : " " + unit.ToString();

            return (measurement.ToDouble(unit).ToString(format) + unitStr).Trim();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString<T>(this IMeasurement<T> measurement, Unit<T> unit1, params Unit<T>[] units) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(units, nameof(units));

            units = units.Concat(new[] { unit1 }).OrderBy(x => measurement.ToDouble(x)).ToArray();
            var unit = units.FirstOrDefault(x => measurement.ToDouble(x) >= 1) ?? units.FirstOrDefault() ?? measurement.MeasurementProvider.ParsableUnits.FirstOrDefault();

            return measurement.ToString(unit, "0.##");
        }
    }
}