using System;
using System.Collections;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class MeasurementProvider<T> where T : Measurement<T> {
        public abstract T CreateMeasurement(double value, Unit<T> unit);

        protected abstract Lazy<Unit<T>> LazyDefaultUnit { get; }

        protected abstract Lazy<IEnumerable<Unit<T>>> LazyParsableUnits { get; }

        public Unit<T> DefaultUnit => LazyDefaultUnit.Value;

        public IEnumerable<Unit<T>> ParsableUnits => LazyParsableUnits.Value;

        public T CreateMeasurementWithDefaultUnits(double value) {
            return this.CreateMeasurement(value, DefaultUnit);
        }
    }

    public abstract class ComplexMeasurementProvider<T, TComp1, TComp2> : MeasurementProvider<T> 
        where T : Measurement<T> 
        where TComp1 : Measurement<TComp1> 
        where  TComp2 : Measurement<TComp2> {

        public abstract MeasurementProvider<TComp1> Component1Provider { get; }

        public abstract MeasurementProvider<TComp2> Component2Provider { get; }
    }
}