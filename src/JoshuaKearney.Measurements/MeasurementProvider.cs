using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public abstract class MeasurementProvider<T> where T : Measurement<T> {
        private IEnumerable<Unit<T>> parsableUnits;

        public abstract T CreateMeasurement(double value, Unit<T> unit);

        protected abstract IEnumerable<Unit<T>> GetParsableUnits();

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

        public Unit<T> DefaultUnit { get; }
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

        //public CompoundMeasurementProvider<T, TComp1, TComp2> AppendComponent1ParsableUnits(params Unit<TComp1>[] units) {
        //    Validate.NonNull(units, nameof(units));

        //    return new ExtendedComplexMeasurementProvider<T, TComp1, TComp2>(
        //        this.Component1Provider.AppendParsableUnits(units),
        //        this.Component2Provider,
        //        this.CreateMeasurement,
        //        () => this.GetParsableUnits()
        //    );
        //}

        //public CompoundMeasurementProvider<T, TComp1, TComp2> AppendComponent2ParsableUnits(params Unit<TComp2>[] units) {
        //    Validate.NonNull(units, nameof(units));

        //    return new ExtendedComplexMeasurementProvider<T, TComp1, TComp2>(
        //        this.Component1Provider,
        //        this.Component2Provider.AppendParsableUnits(units),
        //        this.CreateMeasurement,
        //        () => this.GetParsableUnits()
        //    );
        //}

        //public CompoundMeasurementProvider<T, TComp1, TComp2> Select(Func<MeasurementProvider<TComp1>, MeasurementProvider<TComp1>> compProvider1Select, Func<MeasurementProvider<TComp2>, MeasurementProvider<TComp2>> compProvider2Select) {
        //    return new ExtendedComplexMeasurementProvider<T, TComp1, TComp2>(
        //        compProvider1Select(this.Component1Provider),
        //        compProvider2Select(this.Component2Provider),
        //        this.CreateMeasurement,
        //        () => this.GetParsableUnits()
        //    );
        //}

        public abstract MeasurementProvider<TComp1> Component1Provider { get; }

        public abstract MeasurementProvider<TComp2> Component2Provider { get; }
    }
}