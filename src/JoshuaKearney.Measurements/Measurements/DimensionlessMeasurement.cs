using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Measurements {
    public abstract class DimensionlessMeasurement<T> :
        Measurement<T>, 
        IDividableMeasurement<Time, Frequency> 
        where T : DimensionlessMeasurement<T> {

        public Frequency Divide(Time measurement2) {
            return new Frequency(this.ToDouble(), measurement2);
        }

        public abstract double ToDouble();

        public static implicit operator double(DimensionlessMeasurement<T> measurement) {
            return measurement.ToDouble();
        }
    }
}
