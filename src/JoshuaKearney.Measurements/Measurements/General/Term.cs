using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class Term<TSelf, T1, T2> : Measurement<TSelf>, IDividableMeasurement<T2, T1>
           where TSelf : Term<TSelf, T1, T2>
           where T1 : Measurement<T1>
           where T2 : Measurement<T2> {
        public abstract MeasurementProvider<T1> Item1Provider { get; }

        public abstract MeasurementProvider<T2> Item2Provider { get; }

        public T1 Divide(T2 other) {
            return this.DivideToFirst(other);
        }

        protected Term() {
        }

        protected Term(T1 item1, T2 item2, MeasurementProvider<TSelf> provider) : this(
            item1.DefaultUnits * item2.DefaultUnits,
            item1.MeasurementProvider.DefaultUnit,
            item2.MeasurementProvider.DefaultUnit, 
            provider
        ) { }

        protected Term(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected Term(double amount, Unit<T1> item1Def, Unit<T2> item2Def, MeasurementProvider<TSelf> provider) : base(amount, item1Def.MultiplyToTermUnit(item2Def).ToTermUnit(provider)) {
        }

        public TNew Select<TNew>(Func<T1, T2, TNew> selector) {
            return selector(this.Divide(this.Item2Provider.DefaultUnit), this.Item2Provider.DefaultUnit);
        }

        public Term<T, E> Select<T, E>(Func<T1, T> firstSelect, Func<T2, E> secondSelect)
                where T : Measurement<T>
                where E : Measurement<E> {
            Validate.NonNull(firstSelect, nameof(firstSelect));
            Validate.NonNull(secondSelect, nameof(secondSelect));

            T ret1 = firstSelect(Item1Provider.CreateMeasurementWithDefaultUnits(this.ToDouble(this.MeasurementProvider.DefaultUnit)));
            E ret2 = secondSelect(Item2Provider.DefaultUnit);

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
            this.MeasurementProvider.DefaultUnit.ToTermUnit<TSelf, T1, T2>(),
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

        public Term(double amount, Unit<Term<T1, T2>> unit, MeasurementProvider<T1> t1Prov, MeasurementProvider<T2> t2Prov) : base(amount, unit) {
            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term(T1 item1, T2 item2) : base(item1, item2, GetProvider(item1.MeasurementProvider, item2.MeasurementProvider)) {
            this.Item1Provider = item1.MeasurementProvider;
            this.Item2Provider = item2.MeasurementProvider;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public override MeasurementProvider<Term<T1, T2>> MeasurementProvider { get; }

        public override MeasurementProvider<T1> Item1Provider { get; }

        public override MeasurementProvider<T2> Item2Provider { get; }

        public new T1 DivideToFirst(T2 measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return base.DivideToFirst(measurement2);
        }

        public new T2 DivideToSecond(T1 first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        private static MeasurementProvider<Term<T1, T2>> provider;

        public static MeasurementProvider<Term<T1, T2>> GetProvider(MeasurementProvider<T1> numProvider, MeasurementProvider<T2> denomProvider) {
            if (provider == null) {
                provider = new TermProvider(numProvider, denomProvider);
            }

            return provider;
        }

        private class TermProvider : ComplexMeasurementProvider<Term<T1, T2>, T1, T2> {
            public TermProvider(MeasurementProvider<T1> t1Prov, MeasurementProvider<T2> t2Prov) {
                this.Component1Provider = t1Prov;
                this.Component2Provider = t2Prov;
                this.LazyDefaultUnit = new Lazy<Unit<Term<T1, T2>>>(() => t1Prov.DefaultUnit.MultiplyToTermUnit(t2Prov.DefaultUnit));
            }

            public override MeasurementProvider<T1> Component1Provider { get; }

            public override MeasurementProvider<T2> Component2Provider { get; }

            protected override Lazy<Unit<Term<T1, T2>>> LazyDefaultUnit { get; }

            protected override Lazy<IEnumerable<Unit<Term<T1, T2>>>> LazyParsableUnits { get; }
                = new Lazy<IEnumerable<Unit<Term<T1, T2>>>>(() => new Unit<Term<T1, T2>>[] { });

            public override Term<T1, T2> CreateMeasurement(double value, Unit<Term<T1, T2>> unit) {
                return new Term<T1, T2>(value, unit, Component1Provider, Component2Provider);
            }
        }
    }
}