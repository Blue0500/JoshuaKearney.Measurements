using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class StandardMeasure<T> where T : Measurement<T>, new() {
        public double Value { get; set; }
        public UnitDefinition<T> Unit { get; set; }

        public StandardMeasure(double value, UnitDefinition<T> unit) {
            this.Value = value;
            this.Unit = unit;
        }

        public override string ToString() {
            return this.Value + " " + this.Unit.Symbol;
        }
    }
}