using System;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {

    public static partial class MeasurementExtensions {

        public static double SimplifyToDouble<TSelf, T>(this Ratio<TSelf, T, T> measurement)
            where T : IMeasurement<T>
            where TSelf : Ratio<TSelf, T, T> {

            Validate.NonNull(measurement, nameof(measurement));

            return measurement.Select((x, y) => x.Divide(y));
        }
    }

    public abstract class Ratio<TSelf, TNumerator, TDenominator> : Measurement<TSelf>

        where TSelf : Ratio<TSelf, TNumerator, TDenominator>
        where TNumerator : IMeasurement<TNumerator>
        where TDenominator : IMeasurement<TDenominator> {

        protected Ratio() {
        }

        protected Ratio(IMeasurement<TNumerator> numerator, IMeasurement<TDenominator> denominator, MeasurementProvider<TSelf> provider) : this(
            numerator.ToDouble(numerator.MeasurementProvider.DefaultUnit) / denominator.ToDouble(denominator.MeasurementProvider.DefaultUnit),
            numerator.MeasurementProvider.DefaultUnit.DivideToRatioUnit(denominator.MeasurementProvider.DefaultUnit).ToRatioUnit(provider)
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

        public new Ratio<TDenominator, TNumerator> Reciprocal() {
            return new Ratio<TDenominator, TNumerator>(this.DenominatorProvider.CreateMeasurement(1 / this.Value, this.DenominatorProvider.DefaultUnit), this.NumeratorProvider.DefaultUnit);
        }

        public double ToDouble(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(numDef.DivideToRatioUnit(denomDef).ToRatioUnit(this.MeasurementProvider));
        }

        public Ratio<TNumerator, TDenominator> ToRatio() => new Ratio<TNumerator, TDenominator>(
            this.Value,
            this.MeasurementProvider.DefaultUnit.ToRatioUnit<TSelf, TNumerator, TDenominator>(),
            this.NumeratorProvider,
            this.DenominatorProvider
        );

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

        public Ratio<DoubleMeasurement, TDenominator> Divide(TNumerator measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));
            return this.Select((x, y) => x.Divide(measurement2).DivideToRatio(y));
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
            private readonly Lazy<IEnumerable<Unit<Ratio<TNumerator, TDenominator>>>> parsableUnits;

            public RatioProvider(MeasurementProvider<TNumerator> t1Prov, MeasurementProvider<TDenominator> t2Prov) {
                Validate.NonNull(t1Prov, nameof(t1Prov));
                Validate.NonNull(t2Prov, nameof(t2Prov));

                this.Component1Provider = t1Prov;
                this.Component2Provider = t2Prov;
                this.parsableUnits = new Lazy<IEnumerable<Unit<Ratio<TNumerator, TDenominator>>>>(() => new[] {
                    this.Component1Provider.ParsableUnits.First().DivideToRatioUnit(this.Component2Provider.ParsableUnits.First())
                });
            }

            public override MeasurementProvider<TNumerator> Component1Provider { get; }

            public override MeasurementProvider<TDenominator> Component2Provider { get; }

            public override IEnumerable<Operator> ParseOperators => new Operator[0];

            public override Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                Validate.NonNull(unit, nameof(unit));

                return new Ratio<TNumerator, TDenominator>(value, unit, this.Component1Provider, this.Component2Provider);
            }

            public override IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> ParsableUnits => this.parsableUnits.Value;
        }
    }
}