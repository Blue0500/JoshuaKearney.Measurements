using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {

    public abstract class MeasurementProvider<T> where T : Measurement<T> {
        private static readonly IEnumerable<Operator> defaultOperators = new[] {
            Operator.CreateMultiplication<T, DoubleMeasurement, T>((x, y) => x.Multiply(y)),
            Operator.CreateDivision<T, DoubleMeasurement, T>((x, y) => x.Multiply(y)),
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
            this.DefaultUnit = new Unit<T>("", 1, this);
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

        public Unit<T> DefaultUnit { get; }

        public T NaN => this.CreateMeasurement(double.NaN, DefaultUnit);

        public T PositiveInfinity => this.CreateMeasurement(double.PositiveInfinity, DefaultUnit);

        public T NegativeInfinity => this.CreateMeasurement(double.NegativeInfinity, DefaultUnit);

        public T MaxValue => this.CreateMeasurement(double.MaxValue, DefaultUnit);

        public T MinValue => this.CreateMeasurement(double.MinValue, DefaultUnit);
    }

    public abstract class CompoundMeasurementProvider<T, TComp1, TComp2> : MeasurementProvider<T>
        where T : Measurement<T>
        where TComp1 : Measurement<TComp1>
        where TComp2 : Measurement<TComp2> {

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