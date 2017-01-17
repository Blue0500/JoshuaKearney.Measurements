namespace JoshuaKearney.Measurements.OldParser {

    internal class DoubleMeasurementToken : MeasurementToken {

        public DoubleMeasurementToken(DoubleMeasurement measurement) : base(measurement) {
        }

        public double DoubleValue => (this.MeasurementValue as DoubleMeasurement).ToDouble();
    }
}