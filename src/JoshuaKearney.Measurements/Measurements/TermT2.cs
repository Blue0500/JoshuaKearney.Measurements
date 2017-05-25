using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoshuaKearney.Measurements {
    public sealed partial class Term<T1, T2> : Term<Term<T1, T2>, T1, T2>
            where T1 : IMeasurement<T1>
            where T2 : IMeasurement<T2> {

        internal Term(double amount, Unit<Term<T1, T2>> unit, MeasurementProvider<T1> t1Prov, MeasurementProvider<T2> t2Prov) : base(amount, unit) {
            Validate.NonNull(t1Prov, nameof(t1Prov));
            Validate.NonNull(t2Prov, nameof(t2Prov));

            this.Item1Provider = t1Prov;
            this.Item2Provider = t2Prov;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public Term(IMeasurement<T1> item1, IMeasurement<T2> item2) : base(item1, item2, GetProvider(item1.MeasurementProvider, item2.MeasurementProvider)) {
            this.Item1Provider = item1.MeasurementProvider;
            this.Item2Provider = item2.MeasurementProvider;
            this.MeasurementProvider = GetProvider(this.Item1Provider, this.Item2Provider);
        }

        public override MeasurementProvider<Term<T1, T2>> MeasurementProvider { get; }

        public override MeasurementProvider<T1> Item1Provider { get; }

        public override MeasurementProvider<T2> Item2Provider { get; }

        public new T1 DivideToFirst(IMeasurement<T2> measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return base.DivideToFirst(measurement2);
        }

        public new T2 DivideToSecond(IMeasurement<T1> first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        private static MeasurementProvider<Term<T1, T2>> provider;

        public static MeasurementProvider<Term<T1, T2>> GetProvider(MeasurementProvider<T1> numProvider, MeasurementProvider<T2> denomProvider) {
            Validate.NonNull(numProvider, nameof(numProvider));
            Validate.NonNull(denomProvider, nameof(denomProvider));

            if (provider == null) {
                provider = new TermProvider(numProvider, denomProvider);
            }

            return provider;
        }

        private class TermProvider : CompoundMeasurementProvider<Term<T1, T2>, T1, T2> {
            public TermProvider(MeasurementProvider<T1> t1Prov, MeasurementProvider<T2> t2Prov) {
                Validate.NonNull(t1Prov, nameof(t1Prov));
                Validate.NonNull(t2Prov, nameof(t2Prov));

                this.Component1Provider = t1Prov;
                this.Component2Provider = t2Prov;
            }

            public override MeasurementProvider<T1> Component1Provider { get; }

            public override MeasurementProvider<T2> Component2Provider { get; }

            public override Term<T1, T2> CreateMeasurement(double value, Unit<Term<T1, T2>> unit) {
                Validate.NonNull(unit, nameof(unit));

                return new Term<T1, T2>(value, unit, this.Component1Provider, this.Component2Provider);
            }

            protected override IEnumerable<Operator> GetOperators() => new Operator[0];

            protected override IEnumerable<Unit<Term<T1, T2>>> GetParsableUnits() =>
                new[] { this.Component1Provider.ParsableUnits.First().MultiplyToTermUnit(this.Component2Provider.ParsableUnits.First()) };
        }
    }
}
