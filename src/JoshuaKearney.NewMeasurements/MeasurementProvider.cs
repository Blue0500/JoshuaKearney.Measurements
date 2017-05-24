using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {

    public abstract class MeasurementProvider<T> where T : IMeasurement<T> {
        private readonly Lazy<Unit<T>> defaultUnit;
        private readonly IEnumerable<Operator> defaultOperators = new Operator[] {
            Operator.CreateMultiplication<T, DoubleMeasurement, T>((x, y) => x.Multiply(y)),
            Operator.CreateDivision<T, DoubleMeasurement, T>((x, y) => x.Divide(y)),
            Operator.CreateDivision<T, T, DoubleMeasurement>((x, y) => x.Divide(y)),
            Operator.CreateAddition<T, T, T>((x, y) => x.Add(y)),
            Operator.CreateSubtraction<T, T, T>((x, y) => x.Subtract(y))
        };
        private Lazy<T> nan, posInf, negInf, max, min, zero;

        public abstract T CreateMeasurement(double value, Unit<T> unit);

        public virtual IEnumerable<Unit<T>> ParsableUnits { get; } = Enumerable.Empty<Unit<T>>();

        public virtual IEnumerable<Operator> ParseOperators => this.defaultOperators;

        public Unit<T> DefaultUnit => this.defaultUnit.Value;

        public MeasurementProvider<T> AppendParsableUnits(params Unit<T>[] units) {
            Validate.NonNull(units, nameof(units));

            return new ExtendedMeasurementProvider<T>(
                this.CreateMeasurement,
                () => this.ParsableUnits.Concat(units),
                () => this.ParseOperators
            );
        }

        public MeasurementProvider<T> AppendOperators(params Operator[] operators) {
            Validate.NonNull(operators, nameof(operators));

            return new ExtendedMeasurementProvider<T>(
                this.CreateMeasurement,
                () => this.ParsableUnits,
                () => this.ParseOperators.Concat(operators)
            );
        }

        public MeasurementProvider() {
            this.defaultUnit = new Lazy<Unit<T>>(() => new Unit<T>($"DefaultUnit<{typeof(T).Name}>", 1, this));
            this.nan = new Lazy<T>(() => this.CreateMeasurement(double.NaN, this.defaultUnit.Value));
            this.posInf = new Lazy<T>(() => this.CreateMeasurement(double.PositiveInfinity, this.defaultUnit.Value));
            this.negInf = new Lazy<T>(() => this.CreateMeasurement(double.NegativeInfinity, this.defaultUnit.Value));
            this.max = new Lazy<T>(() => this.CreateMeasurement(double.MinValue, this.defaultUnit.Value));
            this.min = new Lazy<T>(() => this.CreateMeasurement(double.MaxValue, this.defaultUnit.Value));
            this.zero = new Lazy<T>(() => this.CreateMeasurement(0d, this.defaultUnit.Value));
        }

        public T NaN => this.nan.Value;

        public T PositiveInfinity => this.posInf.Value;

        public T NegativeInfinity => this.negInf.Value;

        public T MaxValue => this.max.Value;

        public T MinValue => this.min.Value;

        public T Zero => this.zero.Value;
    }

    public abstract class CompoundMeasurementProvider<T, TComp1, TComp2> : MeasurementProvider<T>
        where T : IMeasurement<T>
        where TComp1 : IMeasurement<TComp1>
        where TComp2 : IMeasurement<TComp2> {

        public new CompoundMeasurementProvider<T, TComp1, TComp2> AppendParsableUnits(params Unit<T>[] units) {
            Validate.NonNull(units, nameof(units));

            return new ExtendedComplexMeasurementProvider<T, TComp1, TComp2>(
                this.Component1Provider,
                this.Component2Provider,
                this.CreateMeasurement,
                () => this.ParsableUnits.Concat(units),
                () => this.ParseOperators
            );
        }

        public new CompoundMeasurementProvider<T, TComp1, TComp2> AppendOperators(params Operator[] operators) {
            Validate.NonNull(operators, nameof(operators));

            return new ExtendedComplexMeasurementProvider<T, TComp1, TComp2>(
                this.Component1Provider,
                this.Component2Provider,
                this.CreateMeasurement,
                () => this.ParsableUnits,
                () => this.ParseOperators.Concat(operators)
            );
        }

        public abstract MeasurementProvider<TComp1> Component1Provider { get; }

        public abstract MeasurementProvider<TComp2> Component2Provider { get; }
    }
}