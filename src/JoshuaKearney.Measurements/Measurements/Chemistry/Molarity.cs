using System;

namespace JoshuaKearney.Measurements {

    public class Molarity : Ratio<Molarity, ChemicalAmount, Volume> {

        public override IMeasurementProvider<Molarity> MeasurementProvider {
            get {
                throw new NotImplementedException();
            }
        }

        protected override IMeasurementProvider<Volume> DenominatorProvider {
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