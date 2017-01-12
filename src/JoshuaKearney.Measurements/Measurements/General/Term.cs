using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public abstract class Term<TSelf, T1, T2> : Measurement<TSelf>
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

        protected Term(T1 item1, T2 item2, MeasurementProvider<TSelf> provider) : base(
            item1.ToDouble(item1.MeasurementProvider.DefaultUnit) * item2.ToDouble(item2.MeasurementProvider.DefaultUnit),
            item1.MeasurementProvider.DefaultUnit.MultiplyToTermUnit(item2.MeasurementProvider.DefaultUnit).ToTermUnit(provider)
        ) { }
        //    item1.ToDouble(item1.MeasurementProvider.DefaultUnit) * item2.ToDouble(item2.MeasurementProvider.DefaultUnit),
        //    item1.MeasurementProvider.DefaultUnit,
        //    item2.MeasurementProvider.DefaultUnit, 
        //    provider
        //) { }

        protected Term(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        //protected Term(double amount, Unit<T1> item1Def, Unit<T2> item2Def, MeasurementProvider<TSelf> provider) : base(amount, item1Def.MultiplyToTermUnit(item2Def).ToTermUnit(provider)) {
        //}

        public TNew Select<TNew>(Func<T1, T2, TNew> selector) {
            Validate.NonNull(selector, nameof(selector));

            return selector(this.Divide(this.Item2Provider.DefaultUnit), this.Item2Provider.DefaultUnit);
        }

        public Term<T, E> Select<T, E>(Func<T1, T> firstSelect, Func<T2, E> secondSelect)
                where T : Measurement<T>
                where E : Measurement<E> {
            Validate.NonNull(firstSelect, nameof(firstSelect));
            Validate.NonNull(secondSelect, nameof(secondSelect));

            T ret1 = firstSelect(
                this.Divide(this.Item2Provider.DefaultUnit)             
            );

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
            this.ToDouble(this.MeasurementProvider.DefaultUnit),
            this.MeasurementProvider.DefaultUnit.ToTermUnit<TSelf, T1, T2>(),
            Item1Provider,
            Item2Provider
        );

        public static implicit operator Term<T1, T2>(Term<TSelf, T1, T2> term) {
            return term.ToTerm();
        }

        protected T1 DivideToFirst(T2 that) {
            Validate.NonNull(that, nameof(that));

            return Item1Provider.CreateMeasurement(
                this.Value / that.ToDouble(that.MeasurementProvider.DefaultUnit),
                Item1Provider.DefaultUnit
            );
        }

        protected T2 DivideToSecond(T1 that) {
            Validate.NonNull(that, nameof(that));

            return Item2Provider.CreateMeasurement(
                this.ToDouble(this.MeasurementProvider.DefaultUnit) / that.ToDouble(that.MeasurementProvider.DefaultUnit),
                Item2Provider.DefaultUnit
            );
        }
    }

    public sealed partial class Term<T1, T2> : Term<Term<T1, T2>, T1, T2>
            where T1 : Measurement<T1>
            where T2 : Measurement<T2> {

        internal Term(double amount, Unit<Term<T1, T2>> unit, MeasurementProvider<T1> t1Prov, MeasurementProvider<T2> t2Prov) : base(amount, unit) {
            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term() { }

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

        private class TermProvider : CompoundMeasurementProvider<Term<T1, T2>, T1, T2> {
            public TermProvider(MeasurementProvider<T1> t1Prov, MeasurementProvider<T2> t2Prov) {
                this.Component1Provider = t1Prov;
                this.Component2Provider = t2Prov;
            }

            public override MeasurementProvider<T1> Component1Provider { get; }

            public override MeasurementProvider<T2> Component2Provider { get; }                

            public override Term<T1, T2> CreateMeasurement(double value, Unit<Term<T1, T2>> unit) {
                return new Term<T1, T2>(value, unit, Component1Provider, Component2Provider);
            }

            protected override IEnumerable<Unit<Term<T1, T2>>> GetParsableUnits() => 
                new[] { Component1Provider.ParsableUnits.First().MultiplyToTermUnit(Component2Provider.ParsableUnits.First()) };
        }
    }
}