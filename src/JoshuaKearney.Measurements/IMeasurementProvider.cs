using System;
using System.Collections;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public interface IMeasurementProvider<T> where T : Measurement<T> {

        T CreateMeasurement(double value, Unit<T> unit);

        Unit<T> DefaultUnit { get; }

        IEnumerable<Unit<T>> AllUnits { get; }
    }

    public static partial class MeasurementExtensions {

        public static T CreateMeasurementWithDefaultUnits<T>(this Lazy<IMeasurementProvider<T>> prov, double amount) where T : Measurement<T> {
            Validate.NonNull(prov, nameof(prov));
            return prov.Value.CreateMeasurement(amount, prov.Value.DefaultUnit);
        }

        public static Unit<T> GetDefaultUnit<T>(this Lazy<IMeasurementProvider<T>> prov) where T : Measurement<T> {
            Validate.NonNull(prov, nameof(prov));
            return prov.Value.DefaultUnit;
        }

        public static IEnumerable<Unit<T>> GetAllUnits<T>(this Lazy<IMeasurementProvider<T>> prov) where T : Measurement<T> {
            Validate.NonNull(prov, nameof(prov));
            return prov.Value.AllUnits;
        }

        public static IEnumerable<Unit<T>> CreateMeasurement<T>(this Lazy<IMeasurementProvider<T>> prov, double amount, Unit<T> unit) where T : Measurement<T> {
            Validate.NonNull(prov, nameof(prov));
            Validate.NonNull(unit, nameof(unit));

            return prov.CreateMeasurement(amount, unit);
        }
    }
}