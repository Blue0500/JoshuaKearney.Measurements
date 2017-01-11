using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.CsvConverters {
    public sealed class CsvMassConverter : CsvMeasurementConverter<Mass> {
        public CsvMassConverter() : base(Mass.Provider) {
        }
    }
}
