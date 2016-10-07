using System;

namespace JoshuaKearney.Measurements.Measurements.Chemistry {

    public class Molality : Ratio<Molality, ChemicalAmount, Mass> {
        public override IMeasurementProvider<Molality> MeasurementProvider {
            get {
                throw new NotImplementedException();
            }
        }

        protected override IMeasurementProvider<Mass> DenominatorProvider {
            get {
                throw new NotImplementedException();
            }
        }

        protected override IMeasurementProvider<ChemicalAmount> NumeratorProvider {
            get {
                throw new NotImplementedException();
            }
        }
    }
}