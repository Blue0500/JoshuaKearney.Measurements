using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Csv {
    public sealed class VolumeConverter : MeasurementConverter<Volume> {
        public VolumeConverter() : base(Volume.Provider) {
        }
    }
}