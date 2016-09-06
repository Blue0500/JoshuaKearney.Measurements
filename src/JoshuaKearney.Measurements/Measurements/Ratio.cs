using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class Ratio<TNumerator, TDenominator> : RatioBase<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator>
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
            private readonly IMeasurementProvider<TNumerator> numProv;
            private readonly IMeasurementProvider<TDenominator> denomProv;

            public RatioProvider(IMeasurementProvider<TNumerator> t1Prov, IMeasurementProvider<TDenominator> t2Prov) {
                this.numProv = t1Prov;
                this.denomProv = t2Prov;
                this.DefaultUnit = t1Prov.DefaultUnit.DivideToRatio(t2Prov.DefaultUnit);
            }

            public Unit<Ratio<TNumerator, TDenominator>> DefaultUnit { get; }

            public IEnumerable<Unit<Ratio<TNumerator, TDenominator>>> BaseUnits => new Unit<Ratio<TNumerator, TDenominator>>[] { };

            public Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                return new Ratio<TNumerator, TDenominator>(value, unit, numProv, denomProv);
            }
        }
    }
}