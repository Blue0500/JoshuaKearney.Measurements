using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public abstract class Ratio<TSelf, TNumerator, TDenominator> :
        Measurement<TSelf>,
        IMultipliableMeasurement<TDenominator, TNumerator>

        where TSelf : Ratio<TSelf, TNumerator, TDenominator>
        where TNumerator : IMeasurement<TNumerator>
        where TDenominator : IMeasurement<TDenominator> {

        protected Ratio() {
        }

        protected Ratio(IMeasurement<TNumerator> numerator, IMeasurement<TDenominator> denominator, MeasurementSupplier<TSelf> provider) : this(
            numerator.ToDouble(numerator.MeasurementSupplier.DefaultUnit) / denominator.ToDouble(denominator.MeasurementSupplier.DefaultUnit),
            numerator.MeasurementSupplier.DefaultUnit.DivideToRatioUnit(denominator.MeasurementSupplier.DefaultUnit).ToRatioUnit(provider)
        ) { }
        
        protected Ratio(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }
        
        public abstract MeasurementSupplier<TDenominator> DenominatorProvider { get; }

        public abstract MeasurementSupplier<TNumerator> NumeratorProvider { get; }


        public static implicit operator Ratio<TNumerator, TDenominator>(Ratio<TSelf, TNumerator, TDenominator> ratio) {
            return ratio.ToRatio();
        }

        public TDenominator Divide<E, F>(Ratio<E, TNumerator, F> that)
            where F : Term<F, TDenominator, TDenominator>
            where E : Ratio<E, TNumerator, F> {

            return this.DenominatorProvider.CreateMeasurement(
                this.Value / that.Value,
                DenominatorProvider.DefaultUnit
            );
        }

        public Ratio<TThatDenom, TDenominator> DivideToRatio<TThatDenom, E>(Ratio<E, TNumerator, TThatDenom> that)
            where TThatDenom : class, IMeasurement<TThatDenom>
            where E : Ratio<E, TNumerator, TThatDenom> {

            var provider = Ratio<TThatDenom, TDenominator>.GetProvider(that.DenominatorProvider, this.DenominatorProvider);
            return provider.CreateMeasurement(this.Value / that.Value, provider.DefaultUnit);
        }

        public double Multiply<E>(Ratio<E, TDenominator, TNumerator> that)
            where E : Ratio<E, TDenominator, TNumerator> {

            Validate.NonNull(that, nameof(that));

            return this.Value * that.Value;
        }

        public new Ratio<TDenominator, TNumerator> Reciprocal() {
            return new Ratio<TDenominator, TNumerator>(
                this.DenominatorProvider.CreateMeasurement(1 / this.Value, this.DenominatorProvider.DefaultUnit), 
                this.NumeratorProvider.DefaultUnit
            );
        }

        public double ToDouble(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(numDef.DivideToRatioUnit(denomDef).ToRatioUnit(this.MeasurementSupplier));
        }

        public Ratio<TNumerator, TDenominator> ToRatio() => new Ratio<TNumerator, TDenominator>(
            this.Value,
            this.MeasurementSupplier.DefaultUnit.ToRatioUnit<TSelf, TNumerator, TDenominator>(),
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

            return this.ToString(numDef.DivideToRatioUnit(denomDef).ToRatioUnit(this.MeasurementSupplier));
        }

        TNumerator IMultipliableMeasurement<TDenominator, TNumerator>.Multiply(TDenominator denominator) {
            var x = MeasurementExtensions.Multiply4(this, this.DenominatorProvider.CreateMeasurement(0, this.DenominatorProvider.DefaultUnit));

            return this.Multiply(denominator);
        }

        //public TNumerator Multiply(IMeasurement<TDenominator> denominator) {
        //    Validate.NonNull(denominator, nameof(denominator));

        //    return this.NumeratorProvider.CreateMeasurement(
        //        this.Value * denominator.ToDouble(denominator.MeasurementSupplier.DefaultUnit),
        //        this.NumeratorProvider.DefaultUnit
        //    );
        //}
    }

    public sealed class Ratio<TNumerator, TDenominator> : Ratio<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator>
            where TNumerator : IMeasurement<TNumerator>
            where TDenominator : IMeasurement<TDenominator> {

        internal Ratio(double amount, Unit<Ratio<TNumerator, TDenominator>> unit, MeasurementSupplier<TNumerator> t1Prov, MeasurementSupplier<TDenominator> t2Prov) : base(amount, unit) {
            this.NumeratorProvider = t1Prov;
            this.DenominatorProvider = t2Prov;
            this.MeasurementSupplier = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public Ratio() { }

        public Ratio(IMeasurement<TNumerator> item1, IMeasurement<TDenominator> item2) : base(item1, item2, GetProvider(item1.MeasurementSupplier, item2.MeasurementSupplier)) {
            this.NumeratorProvider = item1.MeasurementSupplier;
            this.DenominatorProvider = item2.MeasurementSupplier;
            this.MeasurementSupplier = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public override MeasurementSupplier<Ratio<TNumerator, TDenominator>> MeasurementSupplier { get; }

        public override MeasurementSupplier<TDenominator> DenominatorProvider { get; }

        public override MeasurementSupplier<TNumerator> NumeratorProvider { get; }


        private static MeasurementSupplier<Ratio<TNumerator, TDenominator>> provider;

        public static MeasurementSupplier<Ratio<TNumerator, TDenominator>> GetProvider(MeasurementSupplier<TNumerator> numProvider, MeasurementSupplier<TDenominator> denomProvider) {
            if (provider == null) {
                provider = new MeasurementSupplier<Ratio<TNumerator, TDenominator>>((value, unit) => new Ratio<TNumerator, TDenominator>(value, unit, numProvider, denomProvider));
            }

            return provider;
        }

        //private class RatioProvider : CompoundMeasurementSupplier<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator> {
        //    public RatioProvider(MeasurementSupplier<TNumerator> t1Prov, MeasurementSupplier<TDenominator> t2Prov) {
        //        this.Component1Provider = t1Prov;
        //        this.Component2Provider = t2Prov;
        //    }

        //    public override MeasurementSupplier<TNumerator> Component1Provider { get; }

        //    public override MeasurementSupplier<TDenominator> Component2Provider { get; }                

        //    public override Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
        //        return new Ratio<TNumerator, TDenominator>(value, unit, Component1Provider, Component2Provider);
        //    }

        //    protected override IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> GetParsableUnits() => new[] {
        //        this.Component1Provider.ParsableUnits.First().DivideToRatioUnit(this.Component2Provider.ParsableUnits.First())
        //    };
        //}
    }

    public static partial class MeasurementExtensions {
        public static TNumerator Multiply4<TSelf, TNumerator, TDenominator>(this TSelf self, TDenominator denom)
            where TSelf : Ratio<TSelf, TNumerator, TDenominator>
            where TNumerator : IMeasurement<TNumerator>
            where TDenominator : IMeasurement<TDenominator> {

            new Density().Multiply4(Volume.Units.MeterCubed);

            Validate.NonNull(self, nameof(self));
            Validate.NonNull(denom, nameof(denom));

            return default(TNumerator);
        }
    }
}