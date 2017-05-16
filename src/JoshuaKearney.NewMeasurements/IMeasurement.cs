using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshuaKearney.Measurements {
    public interface IMeasurement<T> : IComparable<IMeasurement<T>>, IEquatable<IMeasurement<T>> where T : IMeasurement<T> {
        MeasurementProvider<T> MeasurementProvider { get; }
        double ToDouble(Unit<T> unit);
    }

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

            string unitStr = Measurement.IsInfinity(measurement) || Measurement.IsNan(measurement)
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

        public static Term<T1, T2> MultiplyToTerm<T1, T2>(this IMeasurement<T1> measurement1, IMeasurement<T2> measurement2)
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2> {

            return new Term<T1, T2>(measurement1, measurement2);
        }
    }
}