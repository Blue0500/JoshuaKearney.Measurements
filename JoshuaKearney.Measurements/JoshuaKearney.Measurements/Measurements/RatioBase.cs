using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class RatioBase<TSelf, TNumerator, TDenominator> : Measurement<TSelf>, IMultipliableMeasurement<TDenominator, TNumerator>
        where TSelf : Measurement<TSelf>, new()
        where TNumerator : Measurement, new()
        where TDenominator : Measurement, new() {

        public RatioBase() {
        }

        public RatioBase(double defaultUnits) : base(defaultUnits) {
        }

        public static TSelf From(TNumerator numerator, TDenominator denominator) {
            Validate.NonNull(numerator, nameof(numerator));
            Validate.NonNull(denominator, nameof(denominator));

            return From(
                numerator.DefaultUnits / denominator.DefaultUnits,
                Measurement<TNumerator>.DefaultUnit.DivideToRatio(Measurement<TDenominator>.DefaultUnit).Cast<TSelf>()
            );
        }

        public static TSelf From(double amount, IUnit<TNumerator> numDef, IUnit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return From(amount, numDef.DivideToRatio(denomDef).Cast<TSelf>());
        }

        public static TNumerator operator *(RatioBase<TSelf, TNumerator, TDenominator> ratio, TDenominator denominator) {
            if (ratio == null || denominator == null) {
                return null;
            }

            return ratio.Multiply(denominator);
        }

        public TNumerator Multiply(TDenominator denominator) {
            Validate.NonNull(denominator, nameof(denominator));

            return Measurement<TNumerator>.From(this.DefaultUnits * denominator.DefaultUnits);
        }

        public double ToDouble(IUnit<TNumerator> numDef, IUnit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToDouble(numDef.DivideToRatio(denomDef).Cast<TSelf>());
        }

        public string ToString(IUnit<TNumerator> numDef, IUnit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToString(numDef.DivideToRatio(denomDef).Cast<TSelf>());
        }

        public Ratio<TNumerator, TDenominator> ToTerm() => Measurement<Ratio<TNumerator, TDenominator>>.From(this.DefaultUnits);
    }
}