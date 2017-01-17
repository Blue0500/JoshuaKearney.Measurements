using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements.CsvConverters {
    public class CsvTermConverter<TNumerator, TDenominator> : CsvMeasurementConverter<Term<TNumerator, TDenominator>>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        public CsvTermConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider)
            : base(Term<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider)) {
        }

        public CsvTermConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider, params Operator[] operators)
           : base(Term<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider), operators) {
        }

        public CsvTermConverter(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider, IEnumerable<Operator> operators)
           : base(Term<TNumerator, TDenominator>.GetProvider(numProvider, denomProvider), operators) {
        }
    }
}