using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Csv {
    public class TermConverter<TNumerator, TDenominator> : MeasurementConverter<Term<TNumerator, TDenominator>>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        public TermConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider)
            : base(Term<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider)) {
        }
    }
}
