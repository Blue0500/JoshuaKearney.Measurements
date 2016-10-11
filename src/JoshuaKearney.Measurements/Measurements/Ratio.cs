using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class Ratio<TSelf, TNumerator, TDenominator> :
        Measurement<TSelf>,
        IMultipliableMeasurement<TDenominator, TNumerator>

        where TSelf : Ratio<TSelf, TNumerator, TDenominator>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        protected Ratio() {
        }

        protected Ratio(TNumerator numerator, TDenominator denominator) : this(
            numerator.DefaultUnits / denominator.DefaultUnits,
            numerator.MeasurementProvider.DefaultUnit.DivideToRatio(denominator.MeasurementProvider.DefaultUnit).Cast<Ratio<TNumerator, TDenominator>, TSelf>()
        ) { }

        protected Ratio(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected Ratio(double amount, Unit<TNumerator> numDef, Unit<TDenominator> denomDef) : this(
            amount,
            numDef.DivideToRatio(denomDef).Cast<Ratio<TNumerator, TDenominator>, TSelf>()
        ) { }

        protected abstract IMeasurementProvider<TDenominator> DenominatorProvider { get; }

        protected abstract IMeasurementProvider<TNumerator> NumeratorProvider { get; }

        public static implicit operator Ratio<TNumerator, TDenominator>(Ratio<TSelf, TNumerator, TDenominator> ratio) {
            return ratio.ToRatio();
        }

        public static TNumerator operator *(Ratio<TSelf, TNumerator, TDenominator> ratio, TDenominator denominator) {
            if (ratio == null || denominator == null) {
                return null;
            }

            return ratio.Multiply(denominator);
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
                this.DefaultUnits,
                this.DenominatorProvider.DefaultUnit.DivideToRatio(this.NumeratorProvider.DefaultUnit),
                this.DenominatorProvider,
                this.NumeratorProvider
            );
        }

        public double ToDouble(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(numDef.DivideToRatio(denomDef).Cast<Ratio<TNumerator, TDenominator>, TSelf>());
        }

        public new Ratio<TNumerator, TDenominator> ToRatio() => new Ratio<TNumerator, TDenominator>(
            this.DefaultUnits,
            this.MeasurementProvider.DefaultUnit.Cast<TSelf, Ratio<TNumerator, TDenominator>>(),
            NumeratorProvider,
            DenominatorProvider
        );

        public TNew Reduce<TNew>(Func<TNumerator, TDenominator, TNew> reducer) {
            var oneDenom = DenominatorProvider.CreateMeasurementWithDefaultUnits(1);
            return reducer(this.Multiply(oneDenom), oneDenom);
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

            return this.ToString(numDef.DivideToRatio(denomDef).Cast<Ratio<TNumerator, TDenominator>, TSelf>());
        }
    }

    public sealed class Ratio<TNumerator, TDenominator> : Ratio<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator>
            where TNumerator : Measurement<TNumerator>
            where TDenominator : Measurement<TDenominator> {

        public Ratio(double amount, Unit<Ratio<TNumerator, TDenominator>> unit, IMeasurementProvider<TNumerator> t1Prov, IMeasurementProvider<TDenominator> t2Prov) : base(amount, unit) {
            this.NumeratorProvider = t1Prov;
            this.DenominatorProvider = t2Prov;
            this.MeasurementProvider = new RatioProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public Ratio(TNumerator item1, TDenominator item2) : base(item1, item2) {
            this.NumeratorProvider = item1.MeasurementProvider;
            this.DenominatorProvider = item2.MeasurementProvider;
            this.MeasurementProvider = new RatioProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public override IMeasurementProvider<Ratio<TNumerator, TDenominator>> MeasurementProvider { get; }

        protected override IMeasurementProvider<TDenominator> DenominatorProvider { get; }

        protected override IMeasurementProvider<TNumerator> NumeratorProvider { get; }

        public Ratio<T, E> Simplify<T, E>(Func<TNumerator, T> numConv, Func<TDenominator, E> denomConv)
                where T : Measurement<T>
                where E : Measurement<E> {
            Validate.NonNull(numConv, nameof(numConv));
            Validate.NonNull(denomConv, nameof(denomConv));

            T ret1 = numConv(NumeratorProvider.CreateMeasurementWithDefaultUnits(this.ToDouble(this.MeasurementProvider.DefaultUnit)));
            E ret2 = denomConv(DenominatorProvider.CreateMeasurementWithDefaultUnits(1));

            return new Ratio<T, E>(ret1, ret2);
        }

        private class RatioProvider : IMeasurementProvider<Ratio<TNumerator, TDenominator>> {
            private readonly IMeasurementProvider<TDenominator> denomProv;
            private readonly IMeasurementProvider<TNumerator> numProv;

            public RatioProvider(IMeasurementProvider<TNumerator> t1Prov, IMeasurementProvider<TDenominator> t2Prov) {
                this.numProv = t1Prov;
                this.denomProv = t2Prov;
                this.DefaultUnit = t1Prov.DefaultUnit.DivideToRatio(t2Prov.DefaultUnit);
            }

            public IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> AllUnits => new Unit<Ratio<TNumerator, TDenominator>>[] { };

            public Unit<Ratio<TNumerator, TDenominator>> DefaultUnit { get; }

            public Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                return new Ratio<TNumerator, TDenominator>(value, unit, numProv, denomProv);
            }
        }
    }
}