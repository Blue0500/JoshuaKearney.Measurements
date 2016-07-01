using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public abstract class TermBase<TSelf, T1, T2> : Measurement<TSelf>, IDividableMeasurement<T2, T1>
        where TSelf : TermBase<TSelf, T1, T2>, new()
        where T1 : Measurement, new()
        where T2 : Measurement, new() {

        public TermBase() {
        }

        protected TermBase(double amount) : base(amount) {
        }

        public static TSelf From(T1 item1, T2 item2) {
            Validate.NonNull(item1, nameof(item1));
            Validate.NonNull(item2, nameof(item2));

            return From(
                item1.DefaultUnits * item2.DefaultUnits,
                Measurement<T1>.DefaultUnit.MultiplyToTerm(Measurement<T2>.DefaultUnit).Cast<TSelf>()
            );
        }

        public static TSelf From(double amount, IUnit<T1> item1Def, IUnit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item2Def, nameof(item2Def));

            return From(amount, item1Def.MultiplyToTerm(item2Def).Cast<TSelf>());
        }

        public static TSelf From(double amount, IUnit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return From(amount, unit.Cast<TSelf>());
        }

        public static T1 operator /(TermBase<TSelf, T1, T2> term, T2 term2) {
            if (term == null || term2 == null) {
                return null;
            }

            return term.Divide(term2);
        }

        public T1 Divide(T2 that) {
            Validate.NonNull(that, nameof(that));

            return this.DivideToFirst(that);
        }

        public double ToDouble(IUnit<T1> item1Def, IUnit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item2Def, nameof(item2Def));

            return this.ToDouble(item1Def.MultiplyToTerm(item2Def).Cast<TSelf>());
        }

        public double ToDouble(IUnit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.ToDouble(unit.Cast<TSelf>());
        }

        public string ToString(IUnit<T1> item1Def, IUnit<T2> item2Def) {
            Validate.NonNull(item1Def, nameof(item1Def));
            Validate.NonNull(item1Def, nameof(item2Def));

            return this.ToString(item1Def.MultiplyToTerm(item2Def).Cast<TSelf>());
        }

        public string ToString(IUnit<Term<T1, T2>> unit) {
            Validate.NonNull(unit, nameof(unit));
            return this.ToString(unit.Cast<TSelf>());
        }

        public Term<T1, T2> ToTerm() => Measurement<Term<T1, T2>>.From(this.DefaultUnits);

        protected T1 DivideToFirst(T2 that) {
            Validate.NonNull(that, nameof(that));

            return Measurement<T1>.From(this.DefaultUnits / that.DefaultUnits);
        }

        protected T2 DivideToSecond(T1 that) {
            Validate.NonNull(that, nameof(that));

            return Measurement<T2>.From(this.DefaultUnits / that.DefaultUnits);
        }
    }
}