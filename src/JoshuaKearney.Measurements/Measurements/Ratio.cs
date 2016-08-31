using System;

namespace JoshuaKearney.Measurements {
    //public static class Ratio {
    //    public static Ratio<TNumerator, TDenominator> From<TNumerator, TDenominator>(double amount, IUnit<Ratio<TNumerator, TDenominator>> def)
    //            where TNumerator : Measurement
    //            where TDenominator : Measurement {
    //        Validate.NonNull(def, nameof(def));

    //        return Ratio<TNumerator, TDenominator>.From(amount, def);
    //    }

    //    public static Ratio<TNumerator, TDenominator> From<TNumerator, TDenominator>(
    //                double amount, IUnit<TNumerator> numDef, IUnit<TDenominator> denomDef)
    //            where TNumerator : Measurement
    //            where TDenominator : Measurement {
    //        Validate.NonNull(numDef, nameof(numDef));
    //        Validate.NonNull(denomDef, nameof(denomDef));

    //        return Ratio<TNumerator, TDenominator>.From(amount, numDef, denomDef);
    //    }

    //    [Parser.Flag]
    //    public static Ratio<TNumerator, TDenominator> From<TNumerator, TDenominator>(
    //            TNumerator numerator, TDenominator denominator)
    //            where TNumerator : Measurement
    //            where TDenominator : Measurement {
    //        Validate.NonNull(numerator, nameof(numerator));
    //        Validate.NonNull(denominator, nameof(denominator));

    //        return Ratio<TNumerator, TDenominator>.From(numerator, denominator);
    //    }

    //    public static Ratio<TNumerator, TDenominator> Parse<TNumerator, TDenominator>(string input)
    //            where TNumerator : Measurement
    //            where TDenominator : Measurement {
    //        Validate.NonNull(input, nameof(input));

    //        return Ratio<TNumerator, TDenominator>.Parse(input);
    //    }

    //    public static bool TryParse<TNumerator, TDenominator>(string input, out Ratio<TNumerator, TDenominator> result)
    //            where TNumerator : Measurement
    //            where TDenominator : Measurement {
    //        Validate.NonNull(input, nameof(input));

    //        return Ratio<TNumerator, TDenominator>.TryParse(input, out result);
    //    }
    //}

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

            public Ratio<TNumerator, TDenominator> CreateMeasurement(double value, Unit<Ratio<TNumerator, TDenominator>> unit) {
                return new Ratio<TNumerator, TDenominator>(value, unit, numProv, denomProv);
            }
        }
    }
}