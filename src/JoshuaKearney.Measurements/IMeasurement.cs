using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshuaKearney.Measurements {
    public interface IMeasurement<T> : IComparable<T>, IEquatable<T> where T : IMeasurement<T> {
        MeasurementProvider<T> MeasurementProvider { get; }
        double ToDouble(Unit<T> unit);
    }

    public static partial class Measurement {
        public static T ToMeasurement<T>(this IMeasurement<T> measurement)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement, nameof(measurement));

            if (measurement is T t) {
                return t;
            }
            else {
                return measurement.MeasurementProvider.CreateMeasurement(
                    measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                    measurement.MeasurementProvider.DefaultUnit
                );
            }
        }

        /// <summary>
        /// Returns the absolute value of this instance
        /// </summary>
        /// <returns></returns>
        public static T Abs<T>(this IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        /// <summary>
        /// Adds this instance to the specified measurement.
        /// </summary>
        /// <param name="measurement">The other measurement.</param>
        /// <returns></returns>
        public static T Add<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2) where T : IMeasurement<T> {
            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return measurement1.MeasurementProvider.CreateMeasurement(
                measurement1.ToDouble(measurement1.MeasurementProvider.DefaultUnit) + measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit),
                measurement1.MeasurementProvider.DefaultUnit
            );
        }

        public static Unit<T> ToUnit<T>(this IMeasurement<T> measurement, string symbol) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(symbol, nameof(symbol));

            return new Unit<T>(
                symbol,
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                measurement.MeasurementProvider
            );
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
        /// <param name="measurement2">The other measurement.</param>
        /// <returns></returns>
        public static DoubleMeasurement Divide<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return measurement1.ToDouble(measurement1.MeasurementProvider.DefaultUnit) / measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit);
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
        /// <param name="measurement2">The that.</param>
        /// <returns></returns>
        public static Ratio<T, E> DivideToRatio<T, E>(this IMeasurement<T> measurement1, IMeasurement<E> measurement2)
            where T : IMeasurement<T>
            where E : IMeasurement<E> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return new Ratio<T, E>(measurement1, measurement2);
        }

        /// <summary>
        /// Multiplies this instance by the specified double.
        /// </summary>
        /// <param name="factor">The double to multiply by.</param>
        /// <returns></returns>
        public static TSelf Multiply<TSelf>(this IMeasurement<TSelf> measurement, double factor)
            where TSelf : IMeasurement<TSelf> {

            Validate.NonNull(measurement, nameof(measurement));

            return measurement.MeasurementProvider.CreateMeasurement(
                measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) * factor,
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        public static T Multiply<TSelf, T, E>(this IMeasurement<TSelf> measurement, Ratio<E, T, TSelf> ratio)
            where TSelf : IMeasurement<TSelf>
            where E : Ratio<E, T, TSelf>
            where T : IMeasurement<T> {

            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(ratio, nameof(ratio));

            return ratio.Multiply(measurement);
        }

        /// <summary>
        /// Multiplies this instance by another type of measurement, creating a term
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="measurement2">The that.</param>
        /// <returns></returns>
        public static Term<T, E> MultiplyToTerm<T, E>(this IMeasurement<T> measurement1, IMeasurement<E> measurement2)
            where T : IMeasurement<T>
            where E : IMeasurement<E> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return new Term<T, E>(measurement1, measurement2);
        }

        /// <summary>
        /// Negates this instance.
        /// </summary>
        /// <returns></returns>
        public static T Negate<T>(this IMeasurement<T> measurement)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement, nameof(measurement));

            return measurement.MeasurementProvider.CreateMeasurement(
                -measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit),
                measurement.MeasurementProvider.DefaultUnit
            );
        }

        public static Ratio<DoubleMeasurement, T> Reciprocal<T>(this IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return new Ratio<DoubleMeasurement, T>(new DoubleMeasurement(1), measurement);
        }

        /// <summary>
        /// Subtracts the this instance by another measurement
        /// </summary>
        /// <param name="measurement2">The other measurement.</param>
        /// <returns></returns>
        public static T Subtract<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement1, nameof(measurement1));
            Validate.NonNull(measurement2, nameof(measurement2));

            return measurement1.MeasurementProvider.CreateMeasurement(
                measurement1.ToDouble(measurement1.MeasurementProvider.DefaultUnit) - measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit),
                measurement1.MeasurementProvider.DefaultUnit
            );
        }

        public static Ratio<T, DoubleMeasurement> ToRatio<T>(this IMeasurement<T> measurement)
            where T : IMeasurement<T> {

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
        public static string ToString<T>(this IMeasurement<T> measurement, Unit<T> unit, string format)
            where T : IMeasurement<T> {

            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(format, nameof(format));

            string unitStr = IsInfinity(measurement) || IsNan(measurement)
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
        public static string ToString<TSelf>(this IMeasurement<TSelf> measurement, Unit<TSelf> unit1, params Unit<TSelf>[] units)
            where TSelf : IMeasurement<TSelf> {

            Validate.NonNull(measurement, nameof(measurement));
            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(units, nameof(units));

            units = units.Concat(new[] { unit1 }).OrderBy(x => measurement.ToDouble(x)).ToArray();
            var unit = units.FirstOrDefault(x => measurement.ToDouble(x) >= 1) ?? 
                units.FirstOrDefault() ?? 
                measurement.MeasurementProvider.ParsableUnits.FirstOrDefault() ?? 
                measurement.MeasurementProvider.DefaultUnit;

            return measurement.ToString(unit, "0.##");
        }
    }
}
