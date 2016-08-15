using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class TermBase<TSelf, T1, T2> : Measurement<TSelf>, IDividableMeasurement<T2, T1>
        where TSelf : TermBase<TSelf, T1, T2>
        where T1 : Measurement<T1>
        where T2 : Measurement<T2> {
        protected abstract IMeasurementProvider<T1> Item1Provider { get; }

        protected abstract IMeasurementProvider<T2> Item2Provider { get; }

        protected TermBase() {
        }

        protected TermBase(T1 item1, T2 item2) : base(
            item1.DefaultUnits * item2.DefaultUnits,
            item1.MeasurementProvider.DefaultUnit.MultiplyToTerm(item2.MeasurementProvider.DefaultUnit).Cast<Term<T1, T2>, TSelf>()
        ) { }

        protected TermBase(double amount, IUnit<TSelf> unit) : base(amount, unit) {
        }

        protected TermBase(double amount, IUnit<T1> item1Def, IUnit<T2> item2Def) : base(amount, item1Def.MultiplyToTerm(item2Def).Cast<Term<T1, T2>, TSelf>()) {
        }

        public static T1 operator /(TermBase<TSelf, T1, T2> term, T2 term2) {
            if (term == null || term2 == null) {
                return null;
            }

            return term.Divide(term2);
        }

        public T1 Divide(T2 that) {
            Validate.NonNull(that, nameof(that));

            return this.DivideToFirst(that);
        }

        public double ToDouble(IUnit<T1> item1Def, IUnit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item2Def, nameof(item2Def));

            return this.ToDouble(item1Def.MultiplyToTerm(item2Def).Cast<Term<T1, T2>, TSelf>());
        }

        public double ToDouble(IUnit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit.Cast<Term<T1, T2>, TSelf>());
        }

        public string ToString(IUnit<T1> item1Def, IUnit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item1Def, nameof(item2Def));

            return this.ToString(item1Def.MultiplyToTerm(item2Def).Cast<Term<T1, T2>, TSelf>());
        }

        public string ToString(IUnit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));
            return this.ToString(unit.Cast<Term<T1, T2>, TSelf>());
        }

        public Term<T1, T2> ToTerm() => new Term<T1, T2>(
            this.DefaultUnits,
            this.MeasurementProvider.DefaultUnit.Cast<TSelf, Term<T1, T2>>(),
            Item1Provider,
            Item2Provider
        );

        protected T1 DivideToFirst(T2 that) {
            Validate.NonNull(that, nameof(that));

            return Item1Provider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / that.DefaultUnits);
        }

        protected T2 DivideToSecond(T1 that) {
            Validate.NonNull(that, nameof(that));

            return Item2Provider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / that.DefaultUnits);
        }
    }
}