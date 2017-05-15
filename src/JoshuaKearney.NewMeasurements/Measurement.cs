using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace JoshuaKearney.Measurements {
    //public interface IAddableMeasurement<T> : IMeasurement<T> where T : IMeasurement<T> { }

    public interface IAddableMeasurement<T, TResult> : IMeasurement<T> where T : IMeasurement<T> where TResult : IMeasurement<TResult> {
        TResult Add(IMeasurement<T> other);
        TResult Subtract(IMeasurement<T> other);
    } 

    public static class Measurement {
        public static bool IsInfinity<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsNan<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsNaN(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsNegativeInfinity<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsNegativeInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsPositiveInfinity<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return double.IsPositiveInfinity(measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit));
        }

        public static bool IsZero<T>(IMeasurement<T> measurement) where T : IMeasurement<T> {
            Validate.NonNull(measurement, nameof(measurement));

            return measurement.ToDouble(measurement.MeasurementProvider.DefaultUnit) == 0;
        }

        /// <summary>
        /// Returns the maximum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static T Max<T>(IMeasurement<T> t1, IMeasurement<T> t2) where T : IMeasurement<T> {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            if (t1.CompareTo(t2) >= 0) {
                return t1.ToMeasurement();
            }
            else {
                return t2.ToMeasurement();
            }
        }

        /// <summary>
        /// Returns the maximum of this instance and the specified measurement.
        /// </summary>
        /// <param name="measurements">The other measurements.</param>
        /// <returns></returns>
        public static T Max<T>(IMeasurement<T> t1, IMeasurement<T> t2, params IMeasurement<T>[] measurements) where T : IMeasurement<T> {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));
            Validate.NonNull(measurements, nameof(measurements));
            Validate.NonEmpty(measurements, nameof(measurements));

            return measurements.Concat(new[] { t1, t2 }).Aggregate((x, y) => Max(x, y)).ToMeasurement();
        }

        /// <summary>
        /// Returns the minimum of this instance and the specified measurement.
        /// </summary>
        /// <param name="that">The other measurement.</param>
        /// <returns></returns>
        public static T Min<T>(IMeasurement<T> t1, IMeasurement<T> t2) where T : IMeasurement<T> {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));

            if (t1.CompareTo(t2) <= 0) {
                return t1.ToMeasurement();
            }
            else {
                return t2.ToMeasurement();
            }
        }

        /// <summary>
        /// Returns the minimum of this instance and the specified measurement.
        /// </summary>
        /// <param name="measurments">The other measurments.</param>
        /// <returns></returns>
        public static T Min<T>(IMeasurement<T> t1, IMeasurement<T> t2, params IMeasurement<T>[] measurments) where T : IMeasurement<T> {
            Validate.NonNull(t1, nameof(t1));
            Validate.NonNull(t2, nameof(t2));
            Validate.NonNull(measurments, nameof(measurments));
            Validate.NonEmpty(measurments, nameof(measurments));

            return measurments.Concat(new[] { t1, t2 }).Aggregate((x, y) => Min(x, y)).ToMeasurement();
        }

        public static int Compare<T>(IMeasurement<T> measurement1, IMeasurement<T> measurement2) where T : IMeasurement<T> {
            if (measurement1 == null && measurement2 == null) {
                return 0;
            }
            else if (measurement1 == null || measurement2 == null) {
                return 1;
            }

            return measurement1.ToDouble(
                measurement1.MeasurementProvider.DefaultUnit
            )
            .CompareTo(
                measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit)
            );
        }

        public static bool Equals<T>(IMeasurement<T> measurement1, IMeasurement<T> measurement2) where T : IMeasurement<T> {
            if (measurement1 == null && measurement2 == null) {
                return true;
            }
            else if (measurement1 == null || measurement2 == null) {
                return false;
            }

            return measurement1.ToDouble(
                measurement1.MeasurementProvider.DefaultUnit
            )
            .Equals(
                measurement2.ToDouble(measurement1.MeasurementProvider.DefaultUnit)
            );
        }
    }
}