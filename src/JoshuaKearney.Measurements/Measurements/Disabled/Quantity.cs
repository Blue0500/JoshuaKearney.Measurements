//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements {
//    //public static class Quantity {
//    //    public sealed class QuantityUnit {
//    //        /// <summary>
//    //        /// Gets the name of this unit.
//    //        /// </summary>
//    //        public string Name { get; }

//    //        /// <summary>
//    //        /// Gets the symbol of this unit.
//    //        /// </summary>
//    //        public string Symbol { get; }

//    //        /// <summary>
//    //        /// Gets the units per default unit for this measurement. Ex: 3.2808399 ft/m (meter is the default unit for length)
//    //        /// </summary>
//    //        public double UnitsPerDefault { get; }

//    //        /// <summary>
//    //        /// Initializes a new instance of the <see cref="Unit{T}"/> class.
//    //        /// </summary>
//    //        /// <param name="name">The name.</param>
//    //        /// <param name="symbol">The symbol.</param>
//    //        /// <param name="unitsPerDefault">The units per default unit for this measurement.</param>
//    //        public QuantityUnit(string name, string symbol, double unitsPerDefault) {
//    //            this.Name = name;
//    //            this.Symbol = symbol;
//    //            this.UnitsPerDefault = unitsPerDefault;
//    //        }

//    //        /// <summary>
//    //        /// Returns a <see cref="System.String" /> that represents this instance.
//    //        /// </summary>
//    //        /// <returns>
//    //        /// A <see cref="System.String" /> that represents this instance.
//    //        /// </returns>
//    //        public override string ToString() {
//    //            return this.Symbol;
//    //        }
//    //    }

//    //    public sealed class QuantityUnit<T> : Unit<Quantity<T>> {
//    //        public QuantityUnit(string name, string symbol, double unitsPerDefault) : base(name, symbol, unitsPerDefault) {
//    //        }

//    //        public static implicit operator QuantityUnit<T>(QuantityUnit unit) {
//    //            return new QuantityUnit<T>(unit.Name, unit.Symbol, unit.UnitsPerDefault);
//    //        }
//    //    }
//    //}

//    public sealed class Quantity<T> : Measurement<Quantity<T>> {
//        public static IMeasurementSupplier<Quantity<T>> Provider { get; } = new QuantityProvider();

//        public override IMeasurementSupplier<Quantity<T>> MeasurementSupplier => Provider;

//        public Quantity(double amount) {
//        }

//        public Quantity(double amount, Unit<Quantity<T>> unit) : base(amount, unit) {
//        }

//        public double ToDouble() {
//            return this.ToDouble(Units.DefaultUnit);
//        }

//        public static class Units {
//            internal static Unit<Quantity<T>> DefaultUnit { get; } = new Unit<Quantity<T>>(typeof(T).Name.ToLower(), $"{typeof(T).Name.ToLower()}(s)", 1);

//            public static Unit<Quantity<T>> Gross { get; } = new Unit<Quantity<T>>("gross", "gross", 1d / 144d);

//            public static Unit<Quantity<T>> Dozen { get; } = new Unit<Quantity<T>>("dozen", "dozen", 1d / 12d);

//            public static Unit<Quantity<T>> BakersDozen { get; } = new Unit<Quantity<T>>("b. dozen", "b. dozen", 1d / 13d);
//        }

//        private class QuantityProvider : IMeasurementSupplier<Quantity<T>> {
//            public IEnumerable<Unit<Quantity<T>>> AllUnits { get; } = new Unit<Quantity<T>>[] { Units.Dozen, Units.BakersDozen, Units.Gross };

//            public Unit<Quantity<T>> DefaultUnit => Units.DefaultUnit;

//            public Quantity<T> CreateMeasurement(double value, Unit<Quantity<T>> unit) {
//                return new Quantity<T>(value, unit);
//            }
//        }
//    }
//}