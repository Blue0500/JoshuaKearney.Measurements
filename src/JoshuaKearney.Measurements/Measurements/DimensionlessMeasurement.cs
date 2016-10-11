using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public abstract class DimensionlessMeasurement<T> : Measurement<T> where T : Measurement<T> {

        public double ToDouble() => this.ToDouble(this.MeasurementProvider.DefaultUnit);

        public Frequency Divide(Time measurement2) {
            return new Frequency(this, measurement2);
        }

        public static implicit operator double(DimensionlessMeasurement<T> measurement) {
            return measurement?.ToDouble() ?? 0;
        }

        private static class Units {
            public static Unit<DoubleMeasurement> DefaultUnit { get; } = new Unit<DoubleMeasurement>("", "", 1);
        }
    }
}