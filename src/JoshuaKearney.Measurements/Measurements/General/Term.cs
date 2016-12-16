using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public abstract class Term<TSelf, T1, T2> : Measurement<TSelf>, IDividableMeasurement<T2, T1>
           where TSelf : Term<TSelf, T1, T2>
           where T1 : IMeasurement<T1>
           where T2 : IMeasurement<T2> {
        public abstract MeasurementSupplier<T1> Item1Provider { get; }

        public abstract MeasurementSupplier<T2> Item2Provider { get; }

        protected Term() {
        }

        protected Term(IMeasurement<T1> item1, IMeasurement<T2> item2, MeasurementSupplier<TSelf> provider) : base(
            item1.ToDouble(item1.MeasurementSupplier.DefaultUnit) * item2.ToDouble(item2.MeasurementSupplier.DefaultUnit),
            item1.MeasurementSupplier.DefaultUnit.MultiplyToTermUnit(item2.MeasurementSupplier.DefaultUnit).ToTermUnit(provider)
        ) { }
    
        protected Term(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        public TNew Select<TNew>(Func<T1, T2, TNew> selector) {
            Validate.NonNull(selector, nameof(selector));

            return selector(this.Divide(this.Item2Provider.DefaultUnit), this.Item2Provider.DefaultUnit);
        }

        public Term<T, E> Select<T, E>(Func<IMeasurement<T1>, T> firstSelect, Func<IMeasurement<T2>, E> secondSelect)
                where T : class, IMeasurement<T>
                where E : class, IMeasurement<E> {

            Validate.NonNull(firstSelect, nameof(firstSelect));
            Validate.NonNull(secondSelect, nameof(secondSelect));

            var ret1 = firstSelect(
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

            return this.ToDouble(unit.ToTermUnit(this.MeasurementSupplier));
        }

        public string ToString(Unit<T1> item1Def, Unit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item1Def, nameof(item2Def));

            return this.ToString(item1Def.MultiplyToTermUnit(item2Def));
        }

        public string ToString(Unit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToString(unit.ToTermUnit(this.MeasurementSupplier));
        }

        public Term<T1, T2> ToTerm() => new Term<T1, T2>(
            this.ToDouble(this.MeasurementSupplier.DefaultUnit),
            this.MeasurementSupplier.DefaultUnit.ToTermUnit<TSelf, T1, T2>(),
            Item1Provider,
            Item2Provider
        );

        public static implicit operator Term<T1, T2>(Term<TSelf, T1, T2> term) {
            return term.ToTerm();
        }

        protected T1 DivideToFirst(IMeasurement<T2> that) {
            Validate.NonNull(that, nameof(that));

            return Item1Provider.CreateMeasurement(
                this.Value / that.ToDouble(that.MeasurementSupplier.DefaultUnit),
                Item1Provider.DefaultUnit
            );
        }

        protected T2 DivideToSecond(IMeasurement<T1> that) {
            Validate.NonNull(that, nameof(that));

            return Item2Provider.CreateMeasurement(
                this.ToDouble(this.MeasurementSupplier.DefaultUnit) / that.ToDouble(that.MeasurementSupplier.DefaultUnit),
                Item2Provider.DefaultUnit
            );
        }

        T1 IDividableMeasurement<T2, T1>.Divide(T2 measurement2) => this.DivideToFirst(measurement2);

        public T1 Divide(IMeasurement<T2> measurement2) {
            return this.DivideToFirst(measurement2);
        }
    }

    public sealed partial class Term<T1, T2> : Term<Term<T1, T2>, T1, T2>
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2> {

        internal Term(double amount, Unit<Term<T1, T2>> unit, MeasurementSupplier<T1> t1Prov, MeasurementSupplier<T2> t2Prov) : base(amount, unit) {
            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementSupplier = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term() { }

        public Term(IMeasurement<T1> item1, IMeasurement<T2> item2) : base(item1, item2, GetProvider(item1.MeasurementSupplier, item2.MeasurementSupplier)) {
            this.Item1Provider = item1.MeasurementSupplier;
            this.Item2Provider = item2.MeasurementSupplier;
            this.MeasurementSupplier = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public override MeasurementSupplier<Term<T1, T2>> MeasurementSupplier { get; }

        public override MeasurementSupplier<T1> Item1Provider { get; }

        public override MeasurementSupplier<T2> Item2Provider { get; }

        public new T1 DivideToFirst(IMeasurement<T2> measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return base.DivideToFirst(measurement2);
        }

        public new T2 DivideToSecond(IMeasurement<T1> first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        private static MeasurementSupplier<Term<T1, T2>> provider;

        public static MeasurementSupplier<Term<T1, T2>> GetProvider(MeasurementSupplier<T1> numProvider, MeasurementSupplier<T2> denomProvider) {
            if (provider == null) {
                provider = new MeasurementSupplier<Term<T1, T2>>((value, unit) => new Term<T1, T2>(value, unit, numProvider, denomProvider));
            }

            return provider;
        }
    }
}