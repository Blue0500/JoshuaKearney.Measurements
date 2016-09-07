using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    //public class Quantity<TSelf, T> : Measurement<TSelf> where TSelf : Quantity<TSelf> {
    //    public override IMeasurementProvider<TSelf> MeasurementProvider {
    //        get {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}

    //public class Quantity<T> : Measurement<Quantity<T>> {
    //    public static IMeasurementProvider<Quantity<T>> Provider { get; } = new QuantityProvider();

    //    public override IMeasurementProvider<Quantity<T>> MeasurementProvider => Provider;

    //    public Quantity(double amount) : this(amount, Units.DefaultUnit) {
    //    }

    //    private Quantity(double amount, Unit<Quantity<T>> unit) : base(amount, unit) {
    //    }

    //    private static class Units {
    //        public static Unit<Quantity<T>> DefaultUnit { get; } = new Unit<Quantity<T>>("", "", 1);
    //    }

    //    private class QuantityProvider : IMeasurementProvider<Quantity<T>> {
    //        public IEnumerable<Unit<Quantity<T>>> BaseUnits { get; } = new Unit<Quantity<T>>[] { };

    //        public Unit<Quantity<T>> DefaultUnit { get; } = Units.DefaultUnit;

    //        public Quantity<T> CreateMeasurement(double value, Unit<Quantity<T>> unit) {
    //            return new Quantity<T>(value, unit);
    //        }
    //    }
    //}
}