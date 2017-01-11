using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.JsonConverters {
    public class JsonAreaConverter : JsonMeasurementConverter<Area> {
        public JsonAreaConverter() : base(Area.Provider) {
        }
    }
}
