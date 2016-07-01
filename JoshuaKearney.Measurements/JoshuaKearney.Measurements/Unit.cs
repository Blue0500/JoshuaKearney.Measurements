using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public static partial class Extensions {

        public static IUnit<TNew> Cast<TNew>(this IUnit unit)
                where TNew : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));

            return Unit.Create<TNew>(
                name: unit.Name,
                symbol: unit.Symbol,
                unitsPerDefault: unit.UnitsPerDefault
            );
        }

        public static IUnit<TResult> Cube<TSelf, TResult>(this IUnit<TSelf> unit)
                where TSelf : Measurement, ICubableMeasurement<TResult>, new()
                where TResult : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));

            return Unit.Create<TResult>(
                name: unit.Name + " cubed",
                symbol: unit.Symbol + "³",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        public static IUnit<TResult> Divide<TSelf, TThat, TResult>(this IUnit<TSelf> unit, IUnit<TThat> that)
                where TSelf : Measurement, IDividableMeasurement<TThat, TResult>, new()
                where TThat : Measurement, new()
                where TResult : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<TResult>(that);
        }

        public static IUnit<Ratio<T, TThat>> DivideToRatio<T, TThat>(this IUnit<T> unit, IUnit<TThat> that)
                where T : Measurement, new()
                where TThat : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.DivideTo<Ratio<T, TThat>>(that);
        }

        public static IUnit<TResult> Multiply<TSelf, TThat, TResult>(this IUnit<TSelf> unit, IUnit<TThat> that)
                where TSelf : Measurement, IMultipliableMeasurement<TThat, TResult>, new()
                where TThat : Measurement, new()
                where TResult : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<TResult>(that);
        }

        public static IUnit<Term<T, TThat>> MultiplyToTerm<T, TThat>(this IUnit<T> unit, IUnit<TThat> that)
                where T : Measurement, new()
                where TThat : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));
            Validate.NonNull(that, nameof(that));

            return unit.MultiplyTo<Term<T, TThat>>(that);
        }

        public static IUnit<TResult> Square<TSelf, TResult>(this IUnit<TSelf> unit)
                where TSelf : Measurement, ISquareableMeasurement<TResult>, new()
                where TResult : Measurement, new() {
            Validate.NonNull(unit, nameof(unit));

            return Unit.Create<TResult>(
                name: unit.Name + " squared",
                symbol: unit.Symbol + "²",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        private static IUnit<TResult> CubeTo<TResult>(this IUnit unit)
                where TResult : Measurement, new() {
            return Unit.Create<TResult>(
                name: unit.Name + " cubed",
                symbol: $"({unit.Symbol}^3)",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }

        private static IUnit<TResult> DivideTo<TResult>(this IUnit unit, IUnit that)
                where TResult : Measurement, new() {
            return Unit.Create<TResult>(
                name: $"{unit.Name} per {that.Name}",
                symbol: $"({unit.Symbol}/({that.Symbol}))",
                unitsPerDefault: unit.UnitsPerDefault / that.UnitsPerDefault
            );
        }

        private static IUnit<TResult> MultiplyTo<TResult>(this IUnit unit, IUnit that)
                                                                                        where TResult : Measurement, new() {
            if (unit == that) {
                return unit.SquareTo<TResult>();
            }
            else {
                return Unit.Create<TResult>(
                    name: $"{unit.Name} {that.Name}",
                    symbol: $"({unit.Symbol}*{that.Symbol})",
                    unitsPerDefault: unit.UnitsPerDefault * that.UnitsPerDefault
                );
            }
        }

        private static IUnit<TResult> SquareTo<TResult>(this IUnit unit)
                where TResult : Measurement, new() {
            return Unit.Create<TResult>(
                name: unit.Name + " squared",
                symbol: $"({unit.Symbol}^2)",
                unitsPerDefault: unit.UnitsPerDefault * unit.UnitsPerDefault
            );
        }
    }

    public static class Unit {

        public static IUnit<T> Create<T>(string name, string symbol, double unitsPerDefault)
                where T : Measurement, new() {
            Validate.NonNull(name, nameof(name));
            Validate.NonNull(symbol, nameof(symbol));

            return new UnitImpl<T>(name, symbol, unitsPerDefault);
        }

        public static IPrefixableUnit<T> CreatePrefixable<T>(string name, string symbol, double unitsPerDefault)
                where T : Measurement, new() {
            Validate.NonNull(name, nameof(name));
            Validate.NonNull(symbol, nameof(symbol));

            return new PrefixableUnitImpl<T>(name, symbol, unitsPerDefault);
        }

        private class PrefixableUnitImpl<T> : UnitImpl<T>, IPrefixableUnit, IPrefixableUnit<T> where T : Measurement, new() {

            public PrefixableUnitImpl(string name, string symbol, double unitsPerDefault) : base(name, symbol, unitsPerDefault) {
            }
        }

        private class UnitImpl<T> : IUnit, IUnit<T> where T : Measurement, new() {

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