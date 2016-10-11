using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class Term<TSelf, T1, T2> : Measurement<TSelf>, IDividableMeasurement<T2, T1>
           where TSelf : Term<TSelf, T1, T2>
           where T1 : Measurement<T1>
           where T2 : Measurement<T2> {
        protected abstract IMeasurementProvider<T1> Item1Provider { get; }

        protected abstract IMeasurementProvider<T2> Item2Provider { get; }

        protected Term() {
        }

        protected Term(T1 item1, T2 item2) : base(
            item1.DefaultUnits * item2.DefaultUnits,
            item1.MeasurementProvider.DefaultUnit.MultiplyToTerm(item2.MeasurementProvider.DefaultUnit).Cast<Term<T1, T2>, TSelf>()
        ) { }

        protected Term(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected Term(double amount, Unit<T1> item1Def, Unit<T2> item2Def) : base(amount, item1Def.MultiplyToTerm(item2Def).Cast<Term<T1, T2>, TSelf>()) {
        }

        public static T1 operator /(Term<TSelf, T1, T2> term, T2 term2) {
            if (term == null || term2 == null) {
                return null;
            }

            return term.Divide(term2);
        }

        public TNew Reduce<TNew>(Func<T1, T2, TNew> reducer) {
            T2 oneItem2 = Item2Provider.CreateMeasurementWithDefaultUnits(1);

            return reducer(this.Divide(oneItem2), oneItem2);
        }

        public T1 Divide(T2 that) {
            Validate.NonNull(that, nameof(that));

            return this.DivideToFirst(that);
        }

        public double ToDouble(Unit<T1> item1Def, Unit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item2Def, nameof(item2Def));

            return this.ToDouble(item1Def.MultiplyToTerm(item2Def).Cast<Term<T1, T2>, TSelf>());
        }

        public double ToDouble(Unit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit.Cast<Term<T1, T2>, TSelf>());
        }

        public string ToString(Unit<T1> item1Def, Unit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item1Def, nameof(item2Def));

            return this.ToString(item1Def.MultiplyToTerm(item2Def).Cast<Term<T1, T2>, TSelf>());
        }

        public string ToString(Unit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));
            return this.ToString(unit.Cast<Term<T1, T2>, TSelf>());
        }

        public Term<T1, T2> ToTerm() => new Term<T1, T2>(
            this.DefaultUnits,
            this.MeasurementProvider.DefaultUnit.Cast<TSelf, Term<T1, T2>>(),
            Item1Provider,
            Item2Provider
        );

        public static implicit operator Term<T1, T2>(Term<TSelf, T1, T2> term) {
            return term.ToTerm();
        }

        protected T1 DivideToFirst(T2 that) {
            Validate.NonNull(that, nameof(that));

            return Item1Provider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / that.DefaultUnits);
        }

        protected T2 DivideToSecond(T1 that) {
            Validate.NonNull(that, nameof(that));

            return Item2Provider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / that.DefaultUnits);
        }
    }

    public sealed partial class Term<T1, T2> : Term<Term<T1, T2>, T1, T2>
            where T1 : Measurement<T1>
            where T2 : Measurement<T2> {

        public Term(double amount, Unit<Term<T1, T2>> unit, IMeasurementProvider<T1> t1Prov, IMeasurementProvider<T2> t2Prov) : base(amount, unit) {
            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementProvider = new TermProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term(T1 item1, T2 item2) : base(item1, item2) {
            this.Item1Provider = item1.MeasurementProvider;
            this.Item2Provider = item2.MeasurementProvider;
            this.MeasurementProvider = new TermProvider(this.Item1Provider, this.Item2Provider);
        }

        public override IMeasurementProvider<Term<T1, T2>> MeasurementProvider { get; }

        protected override IMeasurementProvider<T1> Item1Provider { get; }

        protected override IMeasurementProvider<T2> Item2Provider { get; }

        public new T1 DivideToFirst(T2 measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return base.DivideToFirst(measurement2);
        }

        public new T2 DivideToSecond(T1 first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        public Term<T, E> Simplify<T, E>(Func<T1, T> t1Conv, Func<T2, E> t2Conv)
                where T : Measurement<T>
                where E : Measurement<E> {
            Validate.NonNull(t1Conv, nameof(t1Conv));
            Validate.NonNull(t2Conv, nameof(t2Conv));

            T ret1 = t1Conv(Item1Provider.CreateMeasurementWithDefaultUnits(this.ToDouble(this.MeasurementProvider.DefaultUnit)));
            E ret2 = t2Conv(Item2Provider.CreateMeasurementWithDefaultUnits(1));

            return new Term<T, E>(ret1, ret2);
        }

        private class TermProvider : IMeasurementProvider<Term<T1, T2>> {
            private readonly IMeasurementProvider<T1> t1Prov;
            private readonly IMeasurementProvider<T2> t2Prov;

            public TermProvider(IMeasurementProvider<T1> t1Prov, IMeasurementProvider<T2> t2Prov) {
                this.t1Prov = t1Prov;
                this.t2Prov = t2Prov;
                this.DefaultUnit = t1Prov.DefaultUnit.MultiplyToTerm(t2Prov.DefaultUnit);
            }

            public IEnumerable<Unit<Term<T1, T2>>> AllUnits { get; } = new Unit<Term<T1, T2>>[] { };

            public Unit<Term<T1, T2>> DefaultUnit { get; }

            public Term<T1, T2> CreateMeasurement(double value, Unit<Term<T1, T2>> unit) {
                return new Term<T1, T2>(value, unit, t1Prov, t2Prov);
            }
        }
    }
}