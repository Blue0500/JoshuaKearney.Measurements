using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace JoshuaKearney.Measurements {

    public static class Ratio {

        public static Ratio<TNumerator, TDenominator> From<TNumerator, TDenominator>(double amount, IUnit<Ratio<TNumerator, TDenominator>> def)
                where TNumerator : Measurement, new()
                where TDenominator : Measurement, new() {
            Validate.NonNull(def, nameof(def));

            return Ratio<TNumerator, TDenominator>.From(amount, def);
        }

        public static Ratio<TNumerator, TDenominator> From<TNumerator, TDenominator>(
                    double amount, IUnit<TNumerator> numDef, IUnit<TDenominator> denomDef)
                where TNumerator : Measurement, new()
                where TDenominator : Measurement, new() {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return Ratio<TNumerator, TDenominator>.From(amount, numDef, denomDef);
        }

        [Parser.Flag]
        public static Ratio<TNumerator, TDenominator> From<TNumerator, TDenominator>(
                TNumerator numerator, TDenominator denominator)
                where TNumerator : Measurement, new()
                where TDenominator : Measurement, new() {
            Validate.NonNull(numerator, nameof(numerator));
            Validate.NonNull(denominator, nameof(denominator));

            return Ratio<TNumerator, TDenominator>.From(numerator, denominator);
        }

        public static Ratio<TNumerator, TDenominator> Parse<TNumerator, TDenominator>(string input)
                where TNumerator : Measurement, new()
                where TDenominator : Measurement, new() {
            Validate.NonNull(input, nameof(input));

            return Ratio<TNumerator, TDenominator>.Parse(input);
        }

        public static bool TryParse<TNumerator, TDenominator>(string input, out Ratio<TNumerator, TDenominator> result)
                where TNumerator : Measurement, new()
                where TDenominator : Measurement, new() {
            Validate.NonNull(input, nameof(input));

            return Ratio<TNumerator, TDenominator>.TryParse(input, out result);
        }
    }

    public sealed class Ratio<TNumerator, TDenominator> : RatioBase<Ratio<TNumerator, TDenominator>, TNumerator, TDenominator>
            where TNumerator : Measurement, new()
            where TDenominator : Measurement, new() {

        private static MeasurementInfo propertySupplier = new MeasurementInfo(
            instanceCreator: x => new Ratio<TNumerator, TDenominator>(x),
            defaultUnit: Measurement<TNumerator>.DefaultUnit.DivideToRatio(Measurement<TDenominator>.DefaultUnit),
            uniqueUnits: new List<IUnit<Ratio<TNumerator, TDenominator>>>()
        );

        public Ratio() {
        }

        internal Ratio(double defaultUnits) : base(defaultUnits) {
        }

        protected override MeasurementInfo Supplier => propertySupplier;

        public Ratio<T, E> Simplify<T, E>(Func<TNumerator, T> numConv, Func<TDenominator, E> denomConv)
                where T : Measurement, new()
                where E : Measurement, new() {
            Validate.NonNull(numConv, nameof(numConv));
            Validate.NonNull(denomConv, nameof(denomConv));

            return Ratio.From(
                numConv(Measurement<TNumerator>.From(this.ToDouble(GetDefaultUnitDefinition()))),
                denomConv(Measurement<TDenominator>.From(1))
            );
        }
    }
}