using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static partial class MeasurementExtensions {

        public static double SimplifyToDouble<TSelf, T>(this Ratio<TSelf, T, T> measurement)
            where T : Measurement<T>
            where TSelf : Ratio<TSelf, T, T> {
            return measurement.Select((x, y) => x.Divide(y));
        }
    }

    public abstract class Ratio<TSelf, TNumerator, TDenominator> :
        Measurement<TSelf>,
        IMultipliableMeasurement<TDenominator, TNumerator>

        where TSelf : Ratio<TSelf, TNumerator, TDenominator>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        protected Ratio() {
        }

        protected Ratio(TNumerator numerator, TDenominator denominator, MeasurementProvider<TSelf> provider) : this(
            numerator.DefaultUnits / denominator.DefaultUnits,
            numerator.MeasurementProvider.DefaultUnit,
            denominator.MeasurementProvider.DefaultUnit,
            provider
        ) { }

        protected Ratio(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected Ratio(double amount, Unit<TNumerator> numDef, Unit<TDenominator> denomDef, MeasurementProvider<TSelf> provider) : this(
            amount,
            numDef.DivideToRatioUnit(denomDef).ToRatioUnit(provider)
        ) { }

        public abstract MeasurementProvider<TDenominator> DenominatorProvider { get; }

        public abstract MeasurementProvider<TNumerator> NumeratorProvider { get; }


        public static implicit operator Ratio<TNumerator, TDenominator>(Ratio<TSelf, TNumerator, TDenominator> ratio) {
            return ratio.ToRatio();
        }

        public TDenominator Divide<E, F>(Ratio<E, TNumerator, F> that)
                where F : Term<F, TDenominator, TDenominator>
                where E : Ratio<E, TNumerator, F> {
            return this.DenominatorProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / that.DefaultUnits);
        }

        public Ratio<TThatDenom, TDenominator> DivideToRatio<TThatDenom, E>(Ratio<E, TNumerator, TThatDenom> that)
                where TThatDenom : Measurement<TThatDenom>
                where E : Ratio<E, TNumerator, TThatDenom> {
            return that.DenominatorProvider.CreateMeasurement(this.DefaultUnits, that.DenominatorProvider.DefaultUnit).DivideToRatio(
                this.DenominatorProvider.CreateMeasurementWithDefaultUnits(that.DefaultUnits)
            );
        }

        public double Multiply<E>(Ratio<E, TDenominator, TNumerator> that)
                        where E : Ratio<E, TDenominator, TNumerator> {
            return this.DefaultUnits * that.DefaultUnits;
        }

        public TNumerator Multiply(TDenominator denominator) {
            Validate.NonNull(denominator, nameof(denominator));

            return NumeratorProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits * denominator.DefaultUnits);
        }

        public new Ratio<TDenominator, TNumerator> Reciprocal() {
            return new Ratio<TDenominator, TNumerator>(
                1 / this.DefaultUnits,
                this.DenominatorProvider.DefaultUnit.DivideToRatioUnit(this.NumeratorProvider.DefaultUnit),
                this.DenominatorProvider,
                this.NumeratorProvider
            );
        }

        public double ToDouble(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(numDef.DivideToRatioUnit(denomDef).ToRatioUnit(this.MeasurementProvider));
        }

        public new Ratio<TNumerator, TDenominator> ToRatio() => new Ratio<TNumerator, TDenominator>(
            this.DefaultUnits,
            this.MeasurementProvider.DefaultUnit.ToRatioUnit<TSelf, TNumerator, TDenominator>(),
            NumeratorProvider,
            DenominatorProvider
        );

        public TNew Select<TNew>(Func<TNumerator, TDenominator, TNew> selector) {
            return selector(this.Multiply(DenominatorProvider.DefaultUnit), DenominatorProvider.DefaultUnit);
        }

        public Ratio<T, E> Select<T, E>(Func<TNumerator, T> numSelector, Func<TDenominator, E> denomSelector)
                 where T : Measurement<T>
                 where E : Measurement<E> {
            Validate.NonNull(numSelector, nameof(numSelector));
            Validate.NonNull(denomSelector, nameof(denomSelector));

            T ret1 = numSelector(this.Multiply(this.DenominatorProvider.DefaultUnit));
            E ret2 = denomSelector(DenominatorProvider.DefaultUnit);

            return new Ratio<T, E>(ret1, ret2);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="numDef">The numerator definition.</param>
        /// <param name="denomDef">The denominator definition.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToString(numDef.DivideToRatioUnit(denomDef).ToRatioUnit(this.MeasurementProvider));
        }
    }

    public sealed class Ratio<TNumerator, TDenominator> : Ratio<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator>
            where TNumerator : Measurement<TNumerator>
            where TDenominator : Measurement<TDenominator> {

        public Ratio(double amount, Unit<Ratio<TNumerator, TDenominator>> unit, MeasurementProvider<TNumerator> t1Prov, MeasurementProvider<TDenominator> t2Prov) : base(amount, unit) {
            this.NumeratorProvider = t1Prov;
            this.DenominatorProvider = t2Prov;
            this.MeasurementProvider = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public Ratio(TNumerator item1, TDenominator item2) : base(item1, item2, GetProvider(item1.MeasurementProvider, item2.MeasurementProvider)) {
            this.NumeratorProvider = item1.MeasurementProvider;
            this.DenominatorProvider = item2.MeasurementProvider;
            this.MeasurementProvider = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public override MeasurementProvider<Ratio<TNumerator, TDenominator>> MeasurementProvider { get; }

        public override MeasurementProvider<TDenominator> DenominatorProvider { get; }

        public override MeasurementProvider<TNumerator> NumeratorProvider { get; }


        private static MeasurementProvider<Ratio<TNumerator, TDenominator>> provider;

        public static MeasurementProvider<Ratio<TNumerator, TDenominator>> GetProvider(MeasurementProvider<TNumerator> numProvider, MeasurementProvider<TDenominator> denomProvider) {
            if (provider == null) {
                provider = new RatioProvider(numProvider, denomProvider);
            }

            return provider;
        }

        private class RatioProvider : ComplexMeasurementProvider<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator> {
            public RatioProvider(MeasurementProvider<TNumerator> t1Prov, MeasurementProvider<TDenominator> t2Prov) {
                this.Component1Provider = t1Prov;
                this.Component2Provider = t2Prov;
                this.LazyDefaultUnit = new Lazy<Unit<Ratio<TNumerator, TDenominator>>>(() => t1Prov.DefaultUnit.DivideToRatioUnit(t2Prov.DefaultUnit));
            }

            public IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> AllUnits => new Unit<Ratio<TNumerator, TDenominator>>[] { };

            public override MeasurementProvider<TNumerator> Component1Provider { get; }

            public override MeasurementProvider<TDenominator> Component2Provider { get; }

            protected override Lazy<Unit<Ratio<TNumerator, TDenominator>>> LazyDefaultUnit { get; }

            protected override Lazy<IEnumerable<Unit<Ratio<TNumerator, TDenominator>>>> LazyParsableUnits { get; } = 
                new Lazy<IEnumerable<Unit<Ratio<TNumerator, TDenominator>>>>(() => new Unit<Ratio<TNumerator, TDenominator>>[] { });

            public override Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                return new Ratio<TNumerator, TDenominator>(value, unit, Component1Provider, Component2Provider);
            }
        }
    }
}