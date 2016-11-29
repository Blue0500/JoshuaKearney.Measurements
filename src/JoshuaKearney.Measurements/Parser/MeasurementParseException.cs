using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    public class MeasurementParseException : Exception {
        public string ParserInput { get; }

        public string IntermediateResult { get; }

        public Type ResultType { get; }

        public MeasurementParseException(string message, string input, string progress, Type type) : base(message) {
            this.ParserInput = input;
            this.IntermediateResult = progress;
            this.ResultType = type;
        }

        public override string ToString() {
            return base.ToString();
        }
    }

    public class MeasurementParseException<T> : MeasurementParseException {
        public MeasurementParseException(string message, string input, string progress) : base(message, input, progress, typeof(T)) {
        }
    }
}
