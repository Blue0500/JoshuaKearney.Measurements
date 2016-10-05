using System;

namespace JoshuaKearney.Measurements {

    public class Molarity : Ratio<Molarity, Mass, ChemicalAmount> {

        public override IMeasurementProvider<Molarity> MeasurementProvider {
            get {
                throw new NotImplementedException();
            }
        }

        protected override IMeasurementProvider<ChemicalAmount> DenominatorProvider {
            get {
                throw new NotImplementedException();
            }
        }

        protected override IMeasurementProvider<Mass> NumeratorProvider {
            get {
                throw new NotImplementedException();
            }
        }
    }
}