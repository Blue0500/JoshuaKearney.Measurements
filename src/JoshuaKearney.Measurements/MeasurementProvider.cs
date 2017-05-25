using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {

    public abstract class MeasurementProvider<T> where T : IMeasurement<T> {
        private static readonly IEnumerable<Operator> defaultOperators = new[] {
            Operator.CreateMultiplication<T, DoubleMeasurement, T>((x, y) => x.Multiply(y)),
            Operator.CreateDivision<T, DoubleMeasurement, T>((x, y) => x.Divide(y)),
            Operator.CreateDivision<T, T, DoubleMeasurement>((x, y) => x.Divide(y)),
            Operator.CreateAddition<T, T, T>((x, y) => x.Add(y)),
            Operator.CreateSubtraction<T, T, T>((x, y) => x.Subtract(y))
        };

        private IEnumerable<Unit<T>> parsableUnits;
        private IEnumerable<Operator> operators = null;

        public abstract T CreateMeasurement(double value, Unit<T> unit);

        protected abstract IEnumerable<Unit<T>> GetParsableUnits();

        protected abstract IEnumerable<Operator> GetOperators();

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
            this.defUnit = new Lazy<Unit<T>>(() => new Unit<T>("", 1, this));
            this.nan = new Lazy<T>(() => this.CreateMeasurement(double.NaN, this.DefaultUnit));
            this.posInf = new Lazy<T>(() => this.CreateMeasurement(double.PositiveInfinity, this.DefaultUnit));
            this.negInf = new Lazy<T>(() => this.CreateMeasurement(double.NegativeInfinity, this.DefaultUnit));
            this.minVal = new Lazy<T>(() => this.CreateMeasurement(double.MinValue, this.DefaultUnit));
            this.maxVal = new Lazy<T>(() => this.CreateMeasurement(double.MaxValue, this.DefaultUnit));
            this.zero = new Lazy<T>(() => this.CreateMeasurement(0d, this.DefaultUnit));
        }

        public IEnumerable<Unit<T>> ParsableUnits {
            get {
                if (this.parsableUnits == null) {
                    this.parsableUnits = this.GetParsableUnits().Where(x => x != null);
                }

                if (this.parsableUnits.Count() <= 0) {
                    throw new Exception($"Invalid measurement provider for type '{typeof(T).ToString()}': There are no parsable units");
                }
                else {
                    return this.parsableUnits;
                }
            }
        }

        public IEnumerable<Operator> ParseOperators {
            get {
                if(this.operators == null) {
                    this.operators = (this.GetOperators() ?? new Operator[] { }).Concat(defaultOperators);
                }

                return this.operators;
            }
        }


        private readonly Lazy<T> nan, posInf, negInf, maxVal, minVal, zero;
        private readonly Lazy<Unit<T>> defUnit;

        public Unit<T> DefaultUnit => this.defUnit.Value;

        public T NaN => this.nan.Value;

        public T PositiveInfinity => this.posInf.Value;

        public T NegativeInfinity => this.negInf.Value;

        public T MaxValue => this.maxVal.Value;

        public T MinValue => this.minVal.Value;

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