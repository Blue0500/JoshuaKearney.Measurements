using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    public class MeasurementParseException : Exception {
        public string ParserInput { get; }

        public Type ResultType { get; }

        public MeasurementParseException(string message, string input, Type type) : base(message) {
            this.ParserInput = input;
            this.ResultType = type;
        }

        public override string ToString() {
            return base.ToString();
        }
    }

    public class MeasurementParseException<T> : MeasurementParseException {
        public MeasurementParseException(string message, string input) : base(message, input, typeof(T)) {
        }
    }
}
