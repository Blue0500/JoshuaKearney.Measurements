using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Csv {
    public class RatioConverter<TNumerator, TDenominator> : MeasurementConverter<Ratio<TNumerator, TDenominator>>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        public RatioConverter(IMeasurementProvider<TNumerator> numProvider, IMeasurementProvider<TDenominator> denomProvider) 
            : base(Ratio<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider)) {
        }
    }
}
