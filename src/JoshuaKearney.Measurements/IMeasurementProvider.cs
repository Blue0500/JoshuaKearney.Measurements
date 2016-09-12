using System.Collections;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public interface IMeasurementProvider<T> where T : Measurement<T> {

        T CreateMeasurement(double value, Unit<T> unit);

        Unit<T> DefaultUnit { get; }

        IEnumerable<Unit<T>> AllUnits { get; }
    }

    public static partial class Extensions {

        public static T CreateMeasurementWithDefaultUnits<T>(this IMeasurementProvider<T> prov, double amount) where T : Measurement<T> {
            return prov.CreateMeasurement(amount, prov.DefaultUnit);
        }
    }
}