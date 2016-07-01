using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace JoshuaKearney.Measurements {

    public static class Term {

        public static Term<T1, T2> From<T1, T2>(double amount, IUnit<Term<T1, T2>> def)
                where T1 : Measurement, new()
                where T2 : Measurement, new() {
            Validate.NonNull(def, nameof(def));

            return Term<T1, T2>.From(amount, def);
        }

        public static Term<T1, T2> From<T1, T2>(double amount, IUnit<T1> item1Def, IUnit<T2> item2Def)
                where T1 : Measurement, new()
                where T2 : Measurement, new() {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item2Def, nameof(item2Def));

            return Term<T1, T2>.From(amount, item1Def, item2Def);
        }

        [Parser.Flag]
        public static Term<T1, T2> From<T1, T2>(T1 item1, T2 item2)
                where T1 : Measurement, new()
                where T2 : Measurement, new() {
            Validate.NonNull(item1, nameof(item1));
            Validate.NonNull(item2, nameof(item2));

            return Term<T1, T2>.From(item1, item2);
        }

        public static Term<T1, T2> Parse<T1, T2>(string input)
                where T1 : Measurement, new()
                where T2 : Measurement, new() {
            Validate.NonNull(input, nameof(input));

            return Term<T1, T2>.Parse(input);
        }

        public static bool TryParse<T1, T2>(string input, out Term<T1, T2> result)
                where T1 : Measurement, new()
                where T2 : Measurement, new() {
            Validate.NonNull(input, nameof(input));

            return Term<T1, T2>.TryParse(input, out result);
        }
    }

    public sealed partial class Term<T1, T2> : TermBase<Term<T1, T2>, T1, T2>
            where T1 : Measurement, new()
            where T2 : Measurement, new() {

        private static MeasurementInfo propertySupplier = new MeasurementInfo(
            instanceCreator: x => new Term<T1, T2>(x),
            defaultUnit: Measurement<T1>.DefaultUnit.MultiplyToTerm(Measurement<T2>.DefaultUnit),
            uniqueUnits: new List<IUnit<Term<T1, T2>>>()
        );

        public Term() {
        }

        private Term(double units) : base(units) {
        }

        protected override MeasurementInfo Supplier { get; } = propertySupplier;

        public new T1 DivideToFirst(T2 second) {
            Validate.NonNull(second, nameof(second));

            return base.DivideToFirst(second);
        }

        public new T2 DivideToSecond(T1 first) {
            Validate.NonNull(first, nameof(first));

            return base.DivideToSecond(first);
        }

        public Term<T, E> Simplify<T, E>(Func<T1, T> t1Conv, Func<T2, E> t2Conv)
                                where T : Measurement, new()
                where E : Measurement, new() {
            Validate.NonNull(t1Conv, nameof(t1Conv));
            Validate.NonNull(t2Conv, nameof(t2Conv));

            return Term.From(
                t1Conv(Measurement<T1>.From(this.ToDouble(GetDefaultUnitDefinition()))),
                t2Conv(Measurement<T2>.From(1))
            );
        }
    }
}