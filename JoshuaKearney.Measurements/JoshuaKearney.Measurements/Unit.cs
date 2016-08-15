using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static IUnit<TResult> Cast<T, TResult>(this IUnit<T> unit)
                where T : Measurement<T>
                where TResult : Measurement<TResult> {
            return Unit.Create<TResult>(
                name: unit.Name,
                symbol: unit.Symbol,
                unitsPerDefault: unit.UnitsPerDefault
            );
        }

        public static IUnit<TResult> Cube<TSelf, TResult>(this IUnit<TSelf> unit)
                where TSelf : Measurement<TSelf>, ICubableMeasurement<TResult>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));

            return Unit.Create<TResult>(
                name: unit.Name + " cubed",
                symbol: unit.Symbol + "³",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        public static IUnit<TResult> Divide<TSelf, TThat, TResult>(this IUnit<TSelf> unit, IUnit<TThat> that)
                where TSelf : Measurement<TSelf>, IDividableMeasurement<TThat, TResult>
                where TThat : Measurement<TThat>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<TSelf, TThat, TResult>(that);
        }

        public static IUnit<Ratio<T, TThat>> DivideToRatio<T, TThat>(this IUnit<T> unit, IUnit<TThat> that)
                where T : Measurement<T>
                where TThat : Measurement<TThat> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<T, TThat, Ratio<T, TThat>>(that);
        }

        public static IUnit<TResult> Multiply<TSelf, TThat, TResult>(this IUnit<TSelf> unit, IUnit<TThat> that)
                where TSelf : Measurement<TSelf>, IMultipliableMeasurement<TThat, TResult>
                where TThat : Measurement<TThat>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<TSelf, TThat, TResult>(that);
        }

        public static IUnit<Term<T, TThat>> MultiplyToTerm<T, TThat>(this IUnit<T> unit, IUnit<TThat> that)
                where T : Measurement<T>
                where TThat : Measurement<TThat> {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<T, TThat, Term<T, TThat>>(that);
        }

        public static IUnit<TResult> Square<TSelf, TResult>(this IUnit<TSelf> unit)
                where TSelf : Measurement<TSelf>, ISquareableMeasurement<TResult>
                where TResult : Measurement<TResult> {
            Validate.NonNull(unit, nameof(unit));

            return Unit.Create<TResult>(
                name: unit.Name + " squared",
                symbol: unit.Symbol + "²",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        private static IUnit<TResult> CubeTo<TIn, TResult>(this IUnit<TIn> unit)
                where TIn : Measurement<TIn>
                where TResult : Measurement<TResult> {
            return Unit.Create<TResult>(
                name: unit.Name + " cubed",
                symbol: $"({unit.Symbol}^3)",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        private static IUnit<TResult> DivideTo<TIn, TIn2, TResult>(this IUnit<TIn> unit, IUnit<TIn2> that)
                where TIn : Measurement<TIn>
                where TIn2 : Measurement<TIn2>
                where TResult : Measurement<TResult> {
            return Unit.Create<TResult>(
                name: $"{unit.Name} per {that.Name}",
                symbol: $"({unit.Symbol}/({that.Symbol}))",
                unitsPerDefault: unit.UnitsPerDefault / that.UnitsPerDefault
            );
        }

        private static IUnit<TResult> MultiplyTo<TIn, TIn2, TResult>(this IUnit<TIn> unit, IUnit<TIn2> that)
                where TIn : Measurement<TIn>
                where TIn2 : Measurement<TIn2>
                where TResult : Measurement<TResult> {
            if (unit == that) {
                return unit.SquareTo<TIn, TResult>();
            }
            else {
                return Unit.Create<TResult>(
                    name: $"{unit.Name} {that.Name}",
                    symbol: $"({unit.Symbol}*{that.Symbol})",
                    unitsPerDefault: unit.UnitsPerDefault * that.UnitsPerDefault
                );
            }
        }

        private static IUnit<TResult> SquareTo<TIn, TResult>(this IUnit<TIn> unit)
                where TIn : Measurement<TIn>
                where TResult : Measurement<TResult> {
            return Unit.Create<TResult>(
                name: unit.Name + " squared",
                symbol: $"({unit.Symbol}^2)",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }
    }

    public static class Unit {

        public static IUnit<T> Create<T>(string name, string symbol, double unitsPerDefault)
                where T : Measurement<T> {
            Validate.NonNull(name, nameof(name));
            Validate.NonNull(symbol, nameof(symbol));

            return new UnitImpl<T>(name, symbol, unitsPerDefault);
        }

        public static IPrefixableUnit<T> CreatePrefixable<T>(string name, string symbol, double unitsPerDefault)
                where T : Measurement<T> {
            Validate.NonNull(name, nameof(name));
            Validate.NonNull(symbol, nameof(symbol));

            return new PrefixableUnitImpl<T>(name, symbol, unitsPerDefault);
        }

        private class PrefixableUnitImpl<T> : UnitImpl<T>, IPrefixableUnit<T> where T : Measurement<T> {

            public PrefixableUnitImpl(string name, string symbol, double unitsPerDefault) : base(name, symbol, unitsPerDefault) {
            }
        }

        private class UnitImpl<T> : IUnit<T> where T : Measurement<T> {

            public UnitImpl(string name, string symbol, double unitsPerDefault) {
                this.Name = name;
                this.Symbol = symbol;
                this.UnitsPerDefault = unitsPerDefault;
                this.AssociatedMeasurement = typeof(T);
            }

            public Type AssociatedMeasurement { get; }

            public string Name { get; }

            public string Symbol { get; }
            public double UnitsPerDefault { get; }

            public override string ToString() {
                if (this.Symbol.Length > 2 && this.Symbol.StartsWith("(") && this.Symbol.EndsWith(")")) {
                    return this.Symbol.Substring(1, this.Symbol.Length - 2);
                }
                else {
                    return this.Symbol;
                }
            }
        }
    }
}