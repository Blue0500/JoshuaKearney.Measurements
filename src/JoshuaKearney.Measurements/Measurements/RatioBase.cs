namespace JoshuaKearney.Measurements {

    public abstract class RatioBase<TSelf, TNumerator, TDenominator> :
        Measurement<TSelf>,
        IMultipliableMeasurement<TDenominator, TNumerator>

        where TSelf : RatioBase<TSelf, TNumerator, TDenominator>
        where TNumerator : Measurement<TNumerator>
        where TDenominator : Measurement<TDenominator> {

        protected RatioBase() {
        }

        protected RatioBase(TNumerator numerator, TDenominator denominator) : this(
            numerator.DefaultUnits / denominator.DefaultUnits,
            numerator.MeasurementProvider.DefaultUnit.DivideToRatio(denominator.MeasurementProvider.DefaultUnit).Cast<Ratio<TNumerator, TDenominator>, TSelf>()
        ) { }

        protected RatioBase(double amount, Unit<TSelf> unit) : base(amount, unit) {
        }

        protected RatioBase(double amount, Unit<TNumerator> numDef, Unit<TDenominator> denomDef) : this(
            amount,
            numDef.DivideToRatio(denomDef).Cast<Ratio<TNumerator, TDenominator>, TSelf>()
        ) { }

        protected abstract IMeasurementProvider<TDenominator> DenominatorProvider { get; }
        protected abstract IMeasurementProvider<TNumerator> NumeratorProvider { get; }

        public static TNumerator operator *(RatioBase<TSelf, TNumerator, TDenominator> ratio, TDenominator denominator) {
            if (ratio == null || denominator == null) {
                return null;
            }

            return ratio.Multiply(denominator);
        }

        public TDenominator Divide<E, F>(RatioBase<E, TNumerator, F> that)
                where F : TermBase<F, TDenominator, TDenominator>
                where E : RatioBase<E, TNumerator, F> {
            return this.DenominatorProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits / that.DefaultUnits);
        }

        public Ratio<TThatDenom, TDenominator> DivideToRatio<TThatDenom, E>(RatioBase<E, TNumerator, TThatDenom> that)
                                where TThatDenom : Measurement<TThatDenom>
                where E : RatioBase<E, TNumerator, TThatDenom> {
            return that.DenominatorProvider.CreateMeasurement(this.DefaultUnits, that.DenominatorProvider.DefaultUnit).DivideToRatio(
                this.DenominatorProvider.CreateMeasurementWithDefaultUnits(that.DefaultUnits)
            );
        }

        public TNumerator Multiply(TDenominator denominator) {
            Validate.NonNull(denominator, nameof(denominator));

            return NumeratorProvider.CreateMeasurementWithDefaultUnits(this.DefaultUnits * denominator.DefaultUnits);
        }

        public double Multiply<E>(RatioBase<E, TDenominator, TNumerator> that)
                where E : RatioBase<E, TDenominator, TNumerator> {
            return this.DefaultUnits * that.DefaultUnits;
        }

        public Ratio<TDenominator, TNumerator> Reciprocal() {
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

        public Ratio<TNumerator, TDenominator> ToRatio() => new Ratio<TNumerator, TDenominator>(
            this.DefaultUnits,
            this.MeasurementProvider.DefaultUnit.Cast<TSelf, Ratio<TNumerator, TDenominator>>(),
            NumeratorProvider,
            DenominatorProvider
        );

        public string ToString(Unit<TNumerator> numDef, Unit<TDenominator> denomDef) {
            Validate.NonNull(numDef, nameof(numDef));
            Validate.NonNull(denomDef, nameof(denomDef));

            return this.ToString(numDef.DivideToRatio(denomDef).Cast<Ratio<TNumerator, TDenominator>, TSelf>());
        }
    }
}