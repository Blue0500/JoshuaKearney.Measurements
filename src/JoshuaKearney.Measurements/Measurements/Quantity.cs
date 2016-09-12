//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JoshuaKearney.Measurements {
//    public abstract class Quantity<TSelf> : Measurement<TSelf> where TSelf : Quantity<TSelf> {
//        private static Unit<TSelf> DefaultUnit { get; } = new Unit<TSelf>("", "", 1);

//        protected sealed class QuantityProvider : IMeasurementProvider<TSelf> {
//            private readonly Func<double, TSelf> makeTSelf;
//            private readonly Unit<TSelf> unit;

//            public QuantityProvider(Func<double, TSelf> createTSelf, string unitName, string unitSymbol) {
//                this.makeTSelf = createTSelf;
//                this.unit = new Unit<TSelf>(unitName, unitSymbol, 1);
//            }

//            public IEnumerable<Unit<TSelf>> BaseUnits => new Unit<TSelf>[] { };

//            public Unit<TSelf> DefaultUnit => unit;

//            public TSelf CreateMeasurement(double value, Unit<TSelf> unit) {
//                return this.makeTSelf(value);
//            }
//        }

//        public Quantity(double value) : base(value, DefaultUnit) {
//        }

//        public double ToDouble() {
//            return this.DefaultUnits;
//        }
//    }

//    public sealed class Label<T> : Measurement<Label<T>> {
//        public static IMeasurementProvider<Label<T>> Provider { get; } = new QuantityProvider();

//        public override IMeasurementProvider<Label<T>> MeasurementProvider => Provider;

//        public Label(double amount) : this(amount, Units.DefaultUnit) {
//        }

//        private Label(double amount, Unit<Label<T>> unit) : base(amount, unit) {
//        }

//        private static class Units {
//            public static Unit<Label<T>> DefaultUnit { get; } = new Unit<Label<T>>("", "", 1);
//        }

//        private class QuantityProvider : IMeasurementProvider<Label<T>> {
//            public IEnumerable<Unit<Label<T>>> BaseUnits { get; } = new Unit<Label<T>>[] { };

//            public Unit<Label<T>> DefaultUnit { get; } = Units.DefaultUnit;

//            public Label<T> CreateMeasurement(double value, Unit<Label<T>> unit) {
//                return new Label<T>(value, unit);
//            }
//        }
//    }
//}