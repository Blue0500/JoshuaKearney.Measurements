using System;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {

    public abstract class Term<TSelf, T1, T2> : Measurement<TSelf>
        where TSelf : Term<TSelf, T1, T2>
        where T1 : IMeasurement<T1>
        where T2 : IMeasurement<T2> {

        public abstract MeasurementProvider<T1> Item1Provider { get; }

        public abstract MeasurementProvider<T2> Item2Provider { get; }

        public T1 Divide(IMeasurement<T2> other) {
            Validate.NonNull(other, nameof(other));
            return this.DivideToFirst(other);
        }

        protected Term(IMeasurement<T1> item1, IMeasurement<T2> item2, MeasurementProvider<TSelf> provider) : base(
            item1.ToDouble(item1.MeasurementProvider.DefaultUnit) * item2.ToDouble(item2.MeasurementProvider.DefaultUnit),
            item1.MeasurementProvider.DefaultUnit.MultiplyToTermUnit(item2.MeasurementProvider.DefaultUnit).ToTermUnit(provider)
        ) { }

        protected Term(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        public TNew Select<TNew>(Func<T1, T2, TNew> selector) {
            Validate.NonNull(selector, nameof(selector));

            return selector(this.Divide(this.Item2Provider.DefaultUnit), this.Item2Provider.DefaultUnit.ToMeasurement());
        }

        public Term<T, E> Select<T, E>(Func<T1, T> firstSelect, Func<T2, E> secondSelect)
                where T : IMeasurement<T>
                where E : IMeasurement<E> {
            Validate.NonNull(firstSelect, nameof(firstSelect));
            Validate.NonNull(secondSelect, nameof(secondSelect));

            T ret1 = firstSelect(
                this.Divide(this.Item2Provider.DefaultUnit)             
            );

            E ret2 = secondSelect(this.Item2Provider.DefaultUnit.ToMeasurement());

            return new Term<T, E>(ret1, ret2);
        }

        public double ToDouble(Unit<T1> item1Def, Unit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item2Def, nameof(item2Def));

            return this.ToDouble(item1Def.MultiplyToTermUnit(item2Def));
        }

        public double ToDouble(Unit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit.ToTermUnit(this.MeasurementProvider));
        }

        public string ToString(Unit<T1> item1Def, Unit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item1Def, nameof(item2Def));

            return this.ToString(item1Def.MultiplyToTermUnit(item2Def));
        }

        public string ToString(Unit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToString(unit.ToTermUnit(this.MeasurementProvider));
        }

        public Term<T1, T2> ToTerm() {
            return new Term<T1, T2>(
                this.ToDouble(this.MeasurementProvider.DefaultUnit),
                this.MeasurementProvider.DefaultUnit.ToTermUnit<TSelf, T1, T2>(),
                this.Item1Provider,
                this.Item2Provider
            );
        }

        public static implicit operator Term<T1, T2>(Term<TSelf, T1, T2> term) {
            if (term == null) {
                return null;
            }

            return term.ToTerm();
        }

        protected T1 DivideToFirst(IMeasurement<T2> that) {
            Validate.NonNull(that, nameof(that));

            return this.Item1Provider.CreateMeasurement(
                this.Value / that.ToDouble(that.MeasurementProvider.DefaultUnit),
                this.Item1Provider.DefaultUnit
            );
        }

        protected T2 DivideToSecond(IMeasurement<T1> that) {
            Validate.NonNull(that, nameof(that));

            return this.Item2Provider.CreateMeasurement(
                this.ToDouble(this.MeasurementProvider.DefaultUnit) / that.ToDouble(that.MeasurementProvider.DefaultUnit),
                this.Item2Provider.DefaultUnit
            );
        }
    }        
}