using System;
using System.Collections.Generic;
using System.Text;

namespace JoshuaKearney.Measurements {
    public interface IAddableMeasurement<TSelf, TOther, TResult> : IMeasurement<TSelf> 
        where TOther : IMeasurement<TOther>
        where TResult : IMeasurement<TResult>
        where TSelf : IMeasurement<TSelf> {

        TResult Add(IMeasurement<TOther> other);
    }

    public interface ISubtractableMeasurement<TSelf, TOther, TResult> : IMeasurement<TSelf>
        where TOther : IMeasurement<TOther>
        where TResult : IMeasurement<TResult>
        where TSelf : IMeasurement<TSelf> {

        TResult Subtract(IMeasurement<TOther> other);
    }

    public static partial class MeasurementExtensions {
        public static T Add<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
            where T : IAddableMeasurement<T, T, T> {

            return measurement1.ToMeasurement().Add(measurement2);
        }

        public static T Subtract<T>(this IMeasurement<T> measurement1, IMeasurement<T> measurement2)
           where T : ISubtractableMeasurement<T, T, T> {

            return measurement1.ToMeasurement().Subtract(measurement2);
        }
    }
}