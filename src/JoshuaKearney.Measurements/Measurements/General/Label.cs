using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static class Label {

        public static class Units {
            public static LabelUnit Gross { get; } = new LabelUnit("gross", "gross", 1d / 144d);

            public static LabelUnit Dozen { get; } = new LabelUnit("dozen", "dozen", 1d / 12d);

            public static LabelUnit BakersDozen { get; } = new LabelUnit("b. dozen", "b. dozen", 1d / 13d);
        }

        public sealed class LabelUnit {

            /// <summary>
            /// Gets the name of this unit.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the symbol of this unit.
            /// </summary>
            public string Symbol { get; }

            /// <summary>
            /// Gets the units per default unit for this measurement. Ex: 3.2808399 ft/m (meter is the default unit for length)
            /// </summary>
            public double UnitsPerDefault { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Unit{T}"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="symbol">The symbol.</param>
            /// <param name="unitsPerDefault">The units per default unit for this measurement.</param>
            public LabelUnit(string name, string symbol, double unitsPerDefault) {
                this.Name = name;
                this.Symbol = symbol;
                this.UnitsPerDefault = unitsPerDefault;
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString() {
                return this.Symbol;
            }
        }

        public sealed class LabelUnit<T> : Unit<Label<T>> {

            public LabelUnit(string name, string symbol, double unitsPerDefault) : base(name, symbol, unitsPerDefault) {
            }

            public static implicit operator LabelUnit<T>(LabelUnit unit) {
                return new LabelUnit<T>(unit.Name, unit.Symbol, unit.UnitsPerDefault);
            }
        }
    }

    public sealed class Label<T> : Measurement<Label<T>> {
        public static IMeasurementProvider<Label<T>> Provider { get; } = new LabelProvider();

        public override IMeasurementProvider<Label<T>> MeasurementProvider => Provider;

        public Label(double amount) {
        }

        public Label(double amount, Label.LabelUnit<T> unit) : this(amount, (Unit<Label<T>>)unit) {
        }

        private Label(double amount, Unit<Label<T>> unit) : base(amount, unit) {
        }

        private static class Units {
            public static Unit<Label<T>> DefaultUnit { get; } = new Unit<Label<T>>(typeof(T).Name.ToLower(), $"{typeof(T).Name.ToLower()}(s)", 1);
        }

        private class LabelProvider : IMeasurementProvider<Label<T>> {
            public IEnumerable<Unit<Label<T>>> AllUnits { get; } = new Unit<Label<T>>[] { };

            public Unit<Label<T>> DefaultUnit => Units.DefaultUnit;

            public Label<T> CreateMeasurement(double value, Unit<Label<T>> unit) {
                return new Label<T>(value, unit);
            }
        }
    }
}