using System;
using System.Collections.Generic;
using System.Linq;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    public abstract class Ratio<TSelf, TNumerator, TDenominator> : Measurement<TSelf>

        where TSelf : Ratio<TSelf, TNumerator, TDenominator>
        where TNumerator : IMeasurement<TNumerator>
        where TDenominator : IMeasurement<TDenominator> {

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

            //var toMult = that.Reciprocal();

            //return provider.CreateMeasurement(
            //    this.ToDouble(this.MeasurementProvider.ReferenceUnit) / this.NumeratorProvider.ReferenceUnit.DefaultsPerUnit / that.ToDouble(that.MeasurementProvider.ReferenceUnit) * that.NumeratorProvider.ReferenceUnit.DefaultsPerUnit,
            //    new Unit<Ratio<TThatDenom, TDenominator>>(
            //        "",
            //        that.DenominatorProvider.ReferenceUnit.DefaultsPerUnit / this.DenominatorProvider.ReferenceUnit.DefaultsPerUnit,
            //        provider
            //    )
            //);

            //return that.DenominatorProvider.CreateMeasurement(
            //    this.ToDouble(this.MeasurementProvider.ReferenceUnit) * this.NumeratorProvider.ReferenceUnit.DefaultsPerUnit / that.ToDouble(that.MeasurementProvider.ReferenceUnit) / that.,
            //    new Unit<TThatDenom>(
            //        "",
            //        this.MeasurementProvider.ReferenceUnit.DefaultsPerUnit / that.MeasurementProvider.ReferenceUnit.DefaultsPerUnit,
            //        that.DenominatorProvider
            //    )
            //)
            //.DivideToRatio(this.DenominatorProvider.ReferenceUnit);
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

            //return NumeratorProvider.CreateMeasurement(
            //    this.ToDouble(this.MeasurementProvider.ReferenceUnit) * denominator.ToDouble(denominator.MeasurementProvider.ReferenceUnit),
            //    new Unit<TNumerator>(
            //        "",
            //        this.MeasurementProvider.ReferenceUnit.DefaultsPerUnit * denominator.MeasurementProvider.ReferenceUnit.DefaultsPerUnit / NumeratorProvider.ReferenceUnit.DefaultsPerUnit,
            //        NumeratorProvider    
            //    )
            //);
        }

        public Ratio<TDenominator, TNumerator> Reciprocal() {
            return new Ratio<TDenominator, TNumerator>(this.DenominatorProvider.CreateMeasurement(1 / this.Value, this.DenominatorProvider.DefaultUnit), this.NumeratorProvider.DefaultUnit);
        }

        public double ToDouble(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(numDef.DivideToRatioUnit(denomDef).ToRatioUnit(this.MeasurementProvider));
        }

        public Ratio<TNumerator, TDenominator> ToRatio() {
            return new Ratio<TNumerator, TDenominator>(
                this.Value,
                this.MeasurementProvider.DefaultUnit.ToRatioUnit<TSelf, TNumerator, TDenominator>(),
                this.NumeratorProvider,
                this.DenominatorProvider
            );
        }

        public TNew Select<TNew>(Func<TNumerator, TDenominator, TNew> selector) {
            Validate.NonNull(selector, nameof(selector));

            return selector(this.Multiply(this.DenominatorProvider.DefaultUnit), this.DenominatorProvider.DefaultUnit.ToMeasurement());
        }

        public Ratio<T, E> Select<T, E>(Func<TNumerator, T> numSelector, Func<TDenominator, E> denomSelector)
            where T : IMeasurement<T>
            where E : IMeasurement<E> {

            Validate.NonNull(numSelector, nameof(numSelector));
            Validate.NonNull(denomSelector, nameof(denomSelector));

            T ret1 = numSelector(this.Multiply(this.DenominatorProvider.DefaultUnit));
            E ret2 = denomSelector(this.DenominatorProvider.DefaultUnit.ToMeasurement());

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

        public Ratio<DoubleMeasurement, TDenominator> Divide(IMeasurement<TNumerator> measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return this.Select((x, y) => x.Divide(measurement2).DivideToRatio(y));
        }
    }

    public static partial class Measurement {

        public static double SimplifyToDouble<TSelf, T>(this Ratio<TSelf, T, T> measurement)
            where T : IMeasurement<T>
            where TSelf : Ratio<TSelf, T, T> {

            Validate.NonNull(measurement, nameof(measurement));

            return measurement.Select((x, y) => x.Divide(y));
        }
    }
}