using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements.CsvConverters {
    public class CsvRatioConverter<TNumerator, TDenominator> : CsvMeasurementConverter<Ratio<TNumerator, TDenominator>>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        public CsvRatioConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider) 
            : base(Ratio<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider)) {
        }

        public CsvRatioConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider, params Operator[] operators)
           : base(Ratio<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider), operators) {
        }

        public CsvRatioConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider, IEnumerable<Operator> operators)
           : base(Ratio<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider), operators) {
        }
    }
}
