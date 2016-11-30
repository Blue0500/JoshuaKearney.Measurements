using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class MeasurementExtensions {
        public static TNew Select<TSelf, T1, T2, TNew>(this TSelf self, Func<T1, T2, TNew> selector)
            where TSelf : Term<TSelf, T1, T2>
            where T1 : Measurement<T1>
            where T2 : Measurement<T2> {

            T2 oneItem2 = ((TSelf)self).Item2Provider.CreateMeasurementWithDefaultUnits(1);
            return selector(self.Divide(oneItem2), oneItem2);
        }

        public static T1 Divide<TSelf, T1, T2>(this Term<TSelf, T1, T2> term, T2 that)
            where TSelf : Term<TSelf, T1, T2>
            where T1 : Measurement<T1>
            where T2 : Measurement<T2> {

            Validate.NonNull(that, nameof(that));
            return ((TSelf)term).Item1Provider.CreateMeasurementWithDefaultUnits(term.DefaultUnits / that.DefaultUnits);
        }
    }

    public abstract class Term<TSelf, T1, T2> : Measurement<TSelf>, IDividableMeasurement<T2, T1>
           where TSelf : Term<TSelf, T1, T2>
           where T1 : Measurement<T1>
           where T2 : Measurement<T2> {
        public abstract Lazy<IMeasurementProvider<T1>> Item1Provider { get; }

        public abstract Lazy<IMeasurementProvider<T2>> Item2Provider { get; }

        protected Term() {
        }

        protected Term(T1 item1, T2 item2, Lazy<IMeasurementProvider<TSelf>> provider) : this(
            item1.DefaultUnits * item2.DefaultUnits,
            item1.MeasurementProvider.Value.DefaultUnit,
            item2.MeasurementProvider.Value.DefaultUnit, 
            provider
        ) { }

        protected Term(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected Term(double amount, Unit<T1> item1Def, Unit<T2> item2Def, Lazy<IMeasurementProvider<TSelf>> provider) : base(amount, item1Def.MultiplyToTermUnit(item2Def).ToTermUnit(provider)) {
        }

        public TNew Select<TNew>(Func<T1, T2, TNew> selector) {
            T2 oneItem2 = Item2Provider.CreateMeasurementWithDefaultUnits(1);

            return selector(this.Divide(oneItem2), oneItem2);
        }

        public Term<T, E> Select<T, E>(Func<T1, T> firstSelect, Func<T2, E> secondSelect)
                where T : Measurement<T>
                where E : Measurement<E> {
            Validate.NonNull(firstSelect, nameof(firstSelect));
            Validate.NonNull(secondSelect, nameof(secondSelect));

            T ret1 = firstSelect(Item1Provider.CreateMeasurementWithDefaultUnits(this.ToDouble(this.MeasurementProvider.GetDefaultUnit())));
            E ret2 = secondSelect(Item2Provider.CreateMeasurementWithDefaultUnits(1));

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

        public Term<T1, T2> ToTerm() => new Term<T1, T2>(
            this.DefaultUnits,
            this.MeasurementProvider.Value.DefaultUnit.ToTermUnit<TSelf, T1, T2>(),
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

        T1 IDividableMeasurement<T2, T1>.Divide(T2 measurement2) {
            return this.di
        }
    }

    public sealed partial class Term<T1, T2> : Term<Term<T1, T2>, T1, T2>
            where T1 : Measurement<T1>
            where T2 : Measurement<T2> {

        public Term(double amount, Unit<Term<T1, T2>> unit, Lazy<IMeasurementProvider<T1>> t1Prov, Lazy<IMeasurementProvider<T2>> t2Prov) : base(amount, unit) {
            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term(T1 item1, T2 item2) : base(item1, item2, GetProvider(item1.MeasurementProvider, item2.MeasurementProvider)) {
            this.Item1Provider = item1.MeasurementProvider;
            this.Item2Provider = item2.MeasurementProvider;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public override Lazy<IMeasurementProvider<Term<T1, T2>>> MeasurementProvider { get; }

        public override Lazy<IMeasurementProvider<T1>> Item1Provider { get; }

        public override Lazy<IMeasurementProvider<T2>> Item2Provider { get; }

        public new T1 DivideToFirst(T2 measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return base.DivideToFirst(measurement2);
        }

        public new T2 DivideToSecond(T1 first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        private static Lazy<IMeasurementProvider<Term<T1, T2>>> provider;

        public static Lazy<IMeasurementProvider<Term<T1, T2>>> GetProvider(Lazy<IMeasurementProvider<T1>> numProvider, Lazy<IMeasurementProvider<T2>> denomProvider) {
            if (provider == null) {
                provider = new Lazy<IMeasurementProvider<Term<T1, T2>>>(() => new TermProvider(numProvider, denomProvider));
            }

            return provider;
        }

        private class TermProvider : IMeasurementProvider<Term<T1, T2>>, IComplexMeasurementProvider<T1, T2> {
            private readonly Lazy<IMeasurementProvider<T1>> t1Prov;
            private readonly Lazy<IMeasurementProvider<T2>> t2Prov;

            private Lazy<Unit<Term<T1, T2>>> defaultUnit;

            public TermProvider(Lazy<IMeasurementProvider<T1>> t1Prov, Lazy<IMeasurementProvider<T2>> t2Prov) {
                this.t1Prov = t1Prov;
                this.t2Prov = t2Prov;
                this.defaultUnit = new Lazy<Unit<Term<T1, T2>>>(() => t1Prov.GetDefaultUnit().MultiplyToTermUnit(t2Prov.GetDefaultUnit()));
            }

            public IEnumerable<Unit<Term<T1, T2>>> AllUnits => new Unit<Term<T1, T2>>[] { };

            public Lazy<IMeasurementProvider<T1>> Component1Provider => t1Prov;

            public Lazy<IMeasurementProvider<T2>> Component2Provider => t2Prov;

            public Unit<Term<T1, T2>> DefaultUnit => defaultUnit.Value;

            public Term<T1, T2> CreateMeasurement(double value, Unit<Term<T1, T2>> unit) {
                return new Term<T1, T2>(value, unit, t1Prov, t2Prov);
            }
        }
    }
}