using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Quantity<TSelf, T> : Measurement<TSelf> where TSelf : Quantity<TSelf, T> {
        public static IMeasurementProvider<Quantity<TSelf, T>> Provider { get; } = new QuantityProvider();

        public override IMeasurementProvider<Quantity<TSelf, T>> MeasurementProvider => Provider;

        public Quantity(double amount) : this(amount, Units.DefaultUnit) {
        }

        private Quantity(double amount, Unit<Quantity<TSelf, T>> unit) : base(amount, unit) {
        }

        private static class Units {
            public static Unit<Quantity<TSelf, T>> DefaultUnit { get; } = new Unit<Quantity<TSelf, T>>("", "", 1);
        }

        private class QuantityProvider : IMeasurementProvider<Quantity<TSelf, T>> {
            public IEnumerable<Unit<Quantity<TSelf, T>>> BaseUnits { get; } = new Unit<Quantity<TSelf, T>>[] { };

            public Unit<Quantity<TSelf, T>> DefaultUnit { get; } = Units.DefaultUnit;

            public Quantity<TSelf, T> CreateMeasurement(double value, Unit<Quantity<TSelf, T>> unit) {
                return new Quantity<TSelf, T>(value, unit);
            }
        }
    }
}