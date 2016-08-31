using System;

namespace JoshuaKearney.Measurements {

    public sealed partial class Term<T1, T2> : TermBase<Term<T1, T2>, T1, T2>
            where T1 : Measurement<T1>
            where T2 : Measurement<T2> {

        public Term(double amount, Unit<Term<T1, T2>> unit, IMeasurementProvider<T1> t1Prov, IMeasurementProvider<T2> t2Prov) : base(amount, unit) {
            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementProvider = new TermProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term(T1 item1, T2 item2) : base(item1, item2) {
            this.Item1Provider = item1.MeasurementProvider;
            this.Item2Provider = item2.MeasurementProvider;
            this.MeasurementProvider = new TermProvider(this.Item1Provider, this.Item2Provider);
        }

        public override IMeasurementProvider<Term<T1, T2>> MeasurementProvider { get; }

        protected override IMeasurementProvider<T1> Item1Provider { get; }

        protected override IMeasurementProvider<T2> Item2Provider { get; }

        public new T1 DivideToFirst(T2 second) {
            Validate.NonNull(second, nameof(second));

            return base.DivideToFirst(second);
        }

        public new T2 DivideToSecond(T1 first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        public Term<T, E> Simplify<T, E>(Func<T1, T> t1Conv, Func<T2, E> t2Conv)
                where T : Measurement<T>
                where E : Measurement<E> {
            Validate.NonNull(t1Conv, nameof(t1Conv));
            Validate.NonNull(t2Conv, nameof(t2Conv));

            T ret1 = t1Conv(Item1Provider.CreateMeasurementWithDefaultUnits(this.ToDouble(this.MeasurementProvider.DefaultUnit)));
            E ret2 = t2Conv(Item2Provider.CreateMeasurementWithDefaultUnits(1));

            return new Term<T, E>(ret1, ret2);
        }

        private class TermProvider : IMeasurementProvider<Term<T1, T2>> {
            private readonly IMeasurementProvider<T1> t1Prov;
            private readonly IMeasurementProvider<T2> t2Prov;

            public TermProvider(IMeasurementProvider<T1> t1Prov, IMeasurementProvider<T2> t2Prov) {
                this.t1Prov = t1Prov;
                this.t2Prov = t2Prov;
                this.DefaultUnit = t1Prov.DefaultUnit.MultiplyToTerm(t2Prov.DefaultUnit);
            }

            public Unit<Term<T1, T2>> DefaultUnit { get; }

            public Term<T1, T2> CreateMeasurement(double value, Unit<Term<T1, T2>> unit) {
                return new Term<T1, T2>(value, unit, t1Prov, t2Prov);
            }
        }
    }
}