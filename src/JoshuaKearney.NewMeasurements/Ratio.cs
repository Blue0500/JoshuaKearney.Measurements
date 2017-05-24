using System;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public abstract class Ratio<TSelf, TNumerator, TDenominator> : Measurement<TSelf>
        where TSelf : Ratio<TSelf, TNumerator, TDenominator>
        where TNumerator : IMeasurement<TNumerator>
        where TDenominator : IMeasurement<TDenominator> {

        protected Ratio() {
        }

        protected Ratio(IMeasurement<TNumerator> numerator, IMeasurement<TDenominator> denominator, MeasurementProvider<TSelf> provider) : this(
            numerator.ToDouble(numerator.MeasurementProvider.DefaultUnit) / denominator.ToDouble(denominator.MeasurementProvider.DefaultUnit),
            ConvertUnit(numerator.MeasurementProvider.DefaultUnit, denominator.MeasurementProvider.DefaultUnit, provider)
        ) { }

        protected Ratio(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        public abstract MeasurementProvider<TDenominator> DenominatorProvider { get; }

        public abstract MeasurementProvider<TNumerator> NumeratorProvider { get; }

        public static implicit operator Ratio<TNumerator, TDenominator>(Ratio<TSelf, TNumerator, TDenominator> ratio) {
            if (ratio == null) {
                return null;
            }

            return ratio.ToRatio();
        }

        public TDenominator Divide<E, F>(Ratio<E, TNumerator, F> that)
            where F : Term<F, TDenominator, TDenominator>
            where E : Ratio<E, TNumerator, F> {

            Validate.NonNull(that, nameof(that));

            return this.DenominatorProvider.CreateMeasurement(
                this.Value / that.Value,
                this.DenominatorProvider.DefaultUnit
            );
        }

        public Ratio<TThatDenom, TDenominator> DivideToRatio<TThatDenom, E>(Ratio<E, TNumerator, TThatDenom> that)
            where TThatDenom : IMeasurement<TThatDenom>
            where E : Ratio<E, TNumerator, TThatDenom> {

            Validate.NonNull(that, nameof(that));

            var provider = Ratio<TThatDenom, TDenominator>.GetProvider(that.DenominatorProvider, this.DenominatorProvider);
            return provider.CreateMeasurement(this.Value / that.Value, provider.DefaultUnit);
        }

        public double Multiply<E>(Ratio<E, TDenominator, TNumerator> that)
                        where E : Ratio<E, TDenominator, TNumerator> {

            Validate.NonNull(that, nameof(that));

            return this.Value * that.Value;
        }

        public TNumerator Multiply(IMeasurement<TDenominator> denominator) {
            Validate.NonNull(denominator, nameof(denominator));

            return this.NumeratorProvider.CreateMeasurement(
                this.Value * denominator.ToDouble(denominator.MeasurementProvider.DefaultUnit),
                this.NumeratorProvider.DefaultUnit    
            );
        }

        public Ratio<TDenominator, TNumerator> Reciprocal() {
            return new Ratio<TDenominator, TNumerator>(this.DenominatorProvider.CreateMeasurement(1 / this.Value, this.DenominatorProvider.DefaultUnit), this.NumeratorProvider.DefaultUnit);
        }

        public double ToDouble(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(ConvertUnit(numDef.DivideToRatioUnit(denomDef), this.MeasurementProvider));
        }

        public Ratio<TNumerator, TDenominator> ToRatio() => this.Select((num, denom) => num.DivideToRatio(denom));

        public TNew Select<TNew>(Func<TNumerator, TDenominator, TNew> selector) {
            Validate.NonNull(selector, nameof(selector));

            return selector(this.Multiply(this.DenominatorProvider.DefaultUnit), this.DenominatorProvider.DefaultUnit);
        }

        public Ratio<T, E> Select<T, E>(Func<TNumerator, T> numSelector, Func<TDenominator, E> denomSelector)
                 where T : IMeasurement<T>
                 where E : IMeasurement<E> {
            Validate.NonNull(numSelector, nameof(numSelector));
            Validate.NonNull(denomSelector, nameof(denomSelector));

            T ret1 = numSelector(this.Multiply(this.DenominatorProvider.DefaultUnit));
            E ret2 = denomSelector(this.DenominatorProvider.DefaultUnit);

            return new Ratio<T, E>(ret1, ret2);
        }

        public Ratio<DoubleMeasurement, TDenominator> Divide(IMeasurement<TNumerator> meas) { 
            return this.Select((num, denom) => num.Divide(meas).DivideToRatio(denom));
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

            return this.ToString(ConvertUnit(numDef.DivideToRatioUnit(denomDef), this.MeasurementProvider));
        }

        private static Unit<Ratio<TNumerator, TDenominator>> ConvertUnit(Unit<TSelf> unit, MeasurementProvider<TNumerator> provider1, MeasurementProvider<TDenominator> provider2) {
            return new Unit<Ratio<TNumerator, TDenominator>>(unit.Symbol, unit.DefaultsPerUnit, Ratio<TNumerator, TDenominator>.GetProvider(provider1, provider2));
        }

        private static Unit<TSelf> ConvertUnit(Unit<Ratio<TNumerator, TDenominator>> unit, MeasurementProvider<TSelf> provider) {
            return new Unit<TSelf>(unit.Symbol, unit.DefaultsPerUnit, provider);
        }

        private static Unit<TSelf> ConvertUnit(Unit<TNumerator> unit1, Unit<TDenominator> unit2, MeasurementProvider<TSelf> provider) {

            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(unit2, nameof(unit2));

            return new Unit<TSelf>(
                $"{unit1.Symbol}/{unit2.Symbol}",
                unit1.DefaultsPerUnit / unit2.DefaultsPerUnit,
                provider
            );
        }
    }

    public sealed class Ratio<TNumerator, TDenominator> : Ratio<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator>
            where TNumerator : IMeasurement<TNumerator>
            where TDenominator : IMeasurement<TDenominator> {

        internal Ratio(double amount, Unit<Ratio<TNumerator, TDenominator>> unit, MeasurementProvider<TNumerator> t1Prov, MeasurementProvider<TDenominator> t2Prov) : base(amount, unit) {
            Validate.NonNull(t1Prov, nameof(t1Prov));
            Validate.NonNull(t2Prov, nameof(t2Prov));

            this.NumeratorProvider = t1Prov;
            this.DenominatorProvider = t2Prov;
            this.MeasurementProvider = GetProvider(this.NumeratorProvider, this.DenominatorProvider);
        }

        public Ratio(IMeasurement<TNumerator> item1, IMeasurement<TDenominator> item2) : base(item1, item2, GetProvider(item1.MeasurementProvider, item2.MeasurementProvider)) {
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

        private class RatioProvider : CompoundMeasurementProvider<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator> {
            private readonly Lazy<Unit<Ratio<TNumerator, TDenominator>>> defaultUnit;

            public override MeasurementProvider<TNumerator> Component1Provider { get; }
            public override MeasurementProvider<TDenominator> Component2Provider { get; }

            public RatioProvider(MeasurementProvider<TNumerator> t1Prov, MeasurementProvider<TDenominator> t2Prov) {
                Validate.NonNull(t1Prov, nameof(t1Prov));
                Validate.NonNull(t2Prov, nameof(t2Prov));

                this.Component1Provider = t1Prov;
                this.Component2Provider = t2Prov;

                this.defaultUnit = new Lazy<Unit<Ratio<TNumerator, TDenominator>>>(() => t1Prov.DefaultUnit.DivideToRatioUnit(t2Prov.DefaultUnit));
            }

            public override IEnumerable<Operator> ParseOperators => new Operator[] {
               // Operator.CreateMultiplication<Ratio<TNumerator, TDenominator>, TDenominator, TNumerator>((x, y) => x.Multiply(y))
            };

            public override IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> ParsableUnits => new[] { this.defaultUnit.Value };

            public override Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                Validate.NonNull(unit, nameof(unit));

                return new Ratio<TNumerator, TDenominator>(value, unit, this.Component1Provider, this.Component2Provider);
            }
        }
    }

    public static partial class MeasurementExtensions {
        public static Unit<Ratio<T1, T2>> DivideToRatioUnit<T1, T2>(this Unit<T1> unit1, Unit<T2> unit2)
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2> {

            Validate.NonNull(unit1, nameof(unit1));
            Validate.NonNull(unit2, nameof(unit2));

            return new Unit<Ratio<T1, T2>>(
                $"{unit1.Symbol}/{unit2.Symbol}",
                unit1.DefaultsPerUnit / unit2.DefaultsPerUnit,
                Ratio<T1, T2>.GetProvider(unit1.MeasurementProvider, unit2.MeasurementProvider)
            );
        }
    }
}