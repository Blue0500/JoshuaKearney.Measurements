using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    // public delegate T MeasurementSupplier<T>(double value, Unit<T> unit) where T : IMeasurement<T>;

    public class MeasurementSupplier<T> where T : IMeasurement<T> {
        private Func<double, Unit<T>, T> createMeasurement;

        public MeasurementSupplier(Func<double, Unit<T>, T> func) {
            this.createMeasurement = func;

            this.DefaultUnit = new Unit<T>("", 1, this);
            this.nan = new Lazy<T>(() => this.CreateMeasurement(double.NaN, this.DefaultUnit));
            this.negInf = new Lazy<T>(() => this.CreateMeasurement(double.NegativeInfinity, this.DefaultUnit));
            this.posInf = new Lazy<T>(() => this.CreateMeasurement(double.PositiveInfinity, this.DefaultUnit));
            this.minValue = new Lazy<T>(() => this.CreateMeasurement(double.MinValue, this.DefaultUnit));
            this.maxValue = new Lazy<T>(() => this.CreateMeasurement(double.MaxValue, this.DefaultUnit));
            this.epsilon = new Lazy<T>(() => this.CreateMeasurement(double.Epsilon, this.DefaultUnit));
        }

        public T CreateMeasurement(double value, Unit<T> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.createMeasurement(value, unit);
        }

        public Unit<T> DefaultUnit { get; }

        private Lazy<T> nan, posInf, negInf, minValue, maxValue, epsilon;

        public T NaN => nan.Value;

        public T PositiveInfinity => posInf.Value;

        public T NegativeInfinity => negInf.Value;

        public T MinValue => minValue.Value;

        public T MaxValue => maxValue.Value;

        public T Epsilon => epsilon.Value;
    }
}