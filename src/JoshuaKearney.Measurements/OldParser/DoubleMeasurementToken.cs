namespace JoshuaKearney.Measurements.Parser {

    internal class DoubleMeasurementToken : MeasurementToken {

        public DoubleMeasurementToken(DoubleMeasurement measurement) : base(measurement) {
        }

        public double DoubleValue => (this.MeasurementValue as DoubleMeasurement).ToDouble();
    }
}