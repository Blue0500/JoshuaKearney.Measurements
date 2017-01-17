using System;

namespace JoshuaKearney.Measurements.OldParser {

    internal class MeasurementToken : Token, IEquatable<MeasurementToken> {
        public object MeasurementValue { get; }

        public MeasurementToken(object measurement) : base(measurement.ToString()) {
            this.MeasurementValue = measurement;
        }

        public override bool Equals(object obj) {
            MeasurementToken tok = obj as MeasurementToken;

            if (tok == null) {
                return false;
            }
            else {
                return this.Equals(tok);
            }
        }

        public bool Equals(MeasurementToken other) {
            if (other == null) {
                return this == null;
            }
            else {
                return this.MeasurementValue.Equals(other.MeasurementValue);
            }
        }

        public override int GetHashCode() {
            return MeasurementValue.GetHashCode();
        }
    }
}