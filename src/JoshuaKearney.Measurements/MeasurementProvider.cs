using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.NewParser;

namespace JoshuaKearney.Measurements {

    public abstract class MeasurementProvider<T> where T : Measurement<T> {
        private IEnumerable<Unit<T>> parsableUnits;
        private IEnumerable<ParsingOperator> operators;

        public abstract T CreateMeasurement(double value, Unit<T> unit);

        protected virtual IEnumerable<Unit<T>> GetParsableUnits() => new Unit<T>[] { };

        protected virtual IEnumerable<ParsingOperator> GetOperators() => new ParsingOperator[] { };

        public MeasurementProvider<T> AppendParsableUnits(params Unit<T>[] units) {
            Validate.NonNull(units, nameof(units));

            IEnumerable<Unit<T>> par = this.GetParsableUnits();
            return new ExtendedMeasurementProvider<T>(
                this.CreateMeasurement,
                () => par.Concat(units)
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

        public IEnumerable<ParsingOperator> ParseOperators {
            get {
                if(this.operators == null) {
                    this.operators = this.GetOperators() ?? new ParsingOperator[] { };
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
                () => this.GetParsableUnits().Concat(units)
            );
        }

        public abstract MeasurementProvider<TComp1> Component1Provider { get; }

        public abstract MeasurementProvider<TComp2> Component2Provider { get; }
    }
}