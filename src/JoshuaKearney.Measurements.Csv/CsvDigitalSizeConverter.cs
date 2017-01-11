using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.CsvConverters {
    public sealed class CsvDigitalSizeConverter : CsvMeasurementConverter<DigitalSize> {
        public CsvDigitalSizeConverter() : base(DigitalSize.Provider) {
        }
    }
}