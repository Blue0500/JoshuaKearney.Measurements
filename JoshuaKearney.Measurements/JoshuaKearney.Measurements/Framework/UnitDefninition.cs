using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class UnitDefinition<T> where T : Measurement<T>, new() {
        public string Symbol { get; set; }
        public Func<double, T> ToStandardUnits { get; set; }
        public Func<T, double> FromStandardUnits { get; set; }
        public MeasurementSystem MeasurementSystem { get; set; }

        public UnitDefinition(string symbol, Func<double, T> toSiUnits, Func<T, double> fromSiUnits, MeasurementSystem system) {
            this.Symbol = symbol;
            this.ToStandardUnits = toSiUnits;
            this.FromStandardUnits = fromSiUnits;
            this.MeasurementSystem = system;
        }

        public UnitDefinition() {
        }

        public static T operator *(UnitDefinition<T> unit, double num) {
            return unit.ToStandardUnits(num);
        }

        public static T operator *(double num, UnitDefinition<T> unit) {
            return unit.ToStandardUnits(num);
        }

        public override string ToString() {
            return this.Symbol;
        }
    }
}