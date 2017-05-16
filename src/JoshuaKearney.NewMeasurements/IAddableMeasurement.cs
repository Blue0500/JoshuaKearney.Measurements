using System;
using System.Collections.Generic;
using System.Text;

namespace JoshuaKearney.Measurements {
    public interface IAddableMeasurement<TSelf, TResult> : IMeasurement<TSelf> 
        where TSelf : IMeasurement<TSelf> {

        TResult Add(IMeasurement<TSelf> other);
        TResult Subtract(IMeasurement<TSelf> other);
    }

    public static partial class MeasurementExtensions {
        public static T Add<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
            where T : IAddableMeasurement<T, T> {

            return measurement1.ToMeasurement().Add(measurement2);
        }

        public static T Subtract<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
           where T : IAddableMeasurement<T, T> {

            return measurement1.ToMeasurement().Add(measurement2);
        }
    }
}