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

        protected Ratio(TNumerator numerator, TDenominator denominator, Lazy<IMeasurementProvider<TSelf>> provider) : this(
            numerator.DefaultUnits / denominator.DefaultUnits,
            numerator.MeasurementProvider.Value.DefaultUnit,
            denominator.MeasurementProvider.Value.DefaultUnit,
            provider
        ) { }

        protected Ratio(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected Ratio(double amount, Unit<TNumerator> numDef, Unit<TDenominator> denomDef, Lazy<IMeasurementProvider<TSelf>> provider) : this(
            amount,
            numDef.DivideToRatioUnit(denomDef).ToRatioUnit(provider)
        ) { }

        public abstract Lazy<IMeasurementProvider<TDenominator>> DenominatorProvider { get; }

        public abstract Lazy<IMeasurementProvider<TNumerator>> NumeratorProvider { get; }


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
            return that.DenominatorProvider.Value.CreateMeasurement(this.DefaultUnits, that.DenominatorProvider.Value.DefaultUnit).DivideToRatio(
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
                this.DenominatorProvider.GetDefaultUnit().DivideToRatioUnit(this.NumeratorProvider.GetDefaultUnit()),
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
            this.MeasurementProvider.Value.DefaultUnit.ToRatioUnit<TSelf, TNumerator, TDenominator>(),
            NumeratorProvider,
            DenominatorProvider
        );

        public TNew Select<TNew>(Func<TNumerator, TDenominator, TNew> selector) {
            var oneDenom = DenominatorProvider.CreateMeasurementWithDefaultUnits(1);
            return selector(this.Multiply(oneDenom), oneDenom);
        }

        public Ratio<T, E> Select<T, E>(Func<TNumerator, T> numSelector, Func<TDenominator, E> denomSelector)
                 where T : Measurement<T>
                 where E : Measurement<E> {
            Validate.NonNull(numSelector, nameof(numSelector));
            Validate.NonNull(denomSelector, nameof(denomSelector));

            T ret1 = numSelector(NumeratorProvider.CreateMeasurementWithDefaultUnits(this.ToDouble(this.MeasurementProvider.Value.DefaultUnit)));
            E ret2 = denomSelector(DenominatorProvider.CreateMeasurementWithDefaultUnits(1));

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

        public Ratio(double amount, Unit<Ratio<TNumerator, TDenominator>> unit, Lazy<IMeasurementProvider<TNumerator>> t1Prov, Lazy<IMeasurementProvider<TDenominator>> t2Prov) : base(amount, unit) {
            this.NumeratorProvider = t1Prov;
            this.DenominatorProvider = t2Prov;
            this.MeasurementProvider = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public Ratio(TNumerator item1, TDenominator item2) : base(item1, item2, GetProvider(item1.MeasurementProvider, item2.MeasurementProvider)) {
            this.NumeratorProvider = item1.MeasurementProvider;
            this.DenominatorProvider = item2.MeasurementProvider;
            this.MeasurementProvider = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public override Lazy<IMeasurementProvider<Ratio<TNumerator, TDenominator>>> MeasurementProvider { get; }

        public override Lazy<IMeasurementProvider<TDenominator>> DenominatorProvider { get; }

        public override Lazy<IMeasurementProvider<TNumerator>> NumeratorProvider { get; }


        private static Lazy<IMeasurementProvider<Ratio<TNumerator, TDenominator>>> provider;

        public static Lazy<IMeasurementProvider<Ratio<TNumerator, TDenominator>>> GetProvider(Lazy<IMeasurementProvider<TNumerator>> numProvider, Lazy<IMeasurementProvider<TDenominator>> denomProvider) {
            if (provider == null) {
                provider = new Lazy<IMeasurementProvider<Ratio<TNumerator, TDenominator>>>(() => new RatioProvider(numProvider, denomProvider));
            }

            return provider;
        }

        private class RatioProvider : IMeasurementProvider<Ratio<TNumerator, TDenominator>>, IComplexMeasurementProvider<TNumerator, TDenominator> {
            private readonly Lazy<IMeasurementProvider<TDenominator>> denomProv;
            private readonly Lazy<IMeasurementProvider<TNumerator>> numProv;

            public RatioProvider(Lazy<IMeasurementProvider<TNumerator>> t1Prov, Lazy<IMeasurementProvider<TDenominator>> t2Prov) {
                this.numProv = t1Prov;
                this.denomProv = t2Prov;
                this.DefaultUnit = numProv.Value.DefaultUnit.DivideToRatioUnit(denomProv.Value.DefaultUnit);
            }

            public IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> AllUnits => new Unit<Ratio<TNumerator, TDenominator>>[] { };

            public Lazy<IMeasurementProvider<TNumerator>> Component1Provider => numProv;

            public Lazy<IMeasurementProvider<TDenominator>> Component2Provider => denomProv;

            public Unit<Ratio<TNumerator, TDenominator>> DefaultUnit { get; }

            public Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                return new Ratio<TNumerator, TDenominator>(value, unit, numProv, denomProv);
            }
        }
    }
}