namespace JoshuaKearney.Measurements {

    public interface IMeasurementProvider<T> where T : Measurement<T> {

        T CreateMeasurement(double value, IUnit<T> unit);

        IUnit<T> DefaultUnit { get; }
    }

    public static partial class Extensions {

        public static T CreateMeasurementWithDefaultUnits<T>(this IMeasurementProvider<T> prov, double amount) where T : Measurement<T> {
            return prov.CreateMeasurement(amount, prov.DefaultUnit);
        }
    }
}