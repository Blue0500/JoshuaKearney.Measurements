using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public abstract class DimensionlessMeasurement<T> : Measurement<T> where T : DimensionlessMeasurement<T> {

        public double ToDouble() => this.ToDouble(this.MeasurementProvider.DefaultUnit);

        protected static Unit<T> DefaultUnit { get; } = new Unit<T>("", "", 1);

        public Frequency Divide(Time measurement2) {
            return new Frequency(this, measurement2);
        }

        public static implicit operator double(DimensionlessMeasurement<T> measurement) {
            return measurement?.ToDouble() ?? double.NaN;
        }
    }
}