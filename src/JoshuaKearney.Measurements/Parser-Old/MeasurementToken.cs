using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {

    internal class MeasurementToken : Token {
        public object MeasurementValue { get; }

        public MeasurementToken(object measurement) : base(measurement.ToString()) {
            this.MeasurementValue = measurement;
        }
    }
}