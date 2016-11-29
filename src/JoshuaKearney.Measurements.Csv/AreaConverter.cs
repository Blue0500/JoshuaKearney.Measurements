using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Csv {
    public sealed class AreaConverter : MeasurementConverter<Area> {
        public AreaConverter() : base(Area.Provider) {
        }
    }
}
