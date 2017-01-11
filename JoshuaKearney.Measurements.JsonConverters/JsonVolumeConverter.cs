using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.JsonConverters {
    public class JsonVolumeConverter : JsonMeasurementConverter<Volume> {
        public JsonVolumeConverter() : base(Volume.Provider) { }
    }
}
