using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public static class Prefix {

        public static IEnumerable<IUnit<T>> All<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));

            yield return Exa(unit); yield return Peta(unit); yield return Tera(unit);
            yield return Giga(unit); yield return Mega(unit); yield return Kilo(unit);
            yield return Hecto(unit); yield return Deca(unit); yield return Deci(unit);
            yield return Centi(unit); yield return Milli(unit); yield return Micro(unit);
            yield return Nano(unit); yield return Pico(unit); yield return Femto(unit);
            yield return Atto(unit); yield return Yobi(unit); yield return Zebi(unit);
            yield return Exbi(unit); yield return Pebi(unit); yield return Tebi(unit);
            yield return Gibi(unit); yield return Mebi(unit); yield return Kibi(unit);
        }

        public static IUnit<T> Atto<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-18, "atto", "a");
        }

        public static IUnit<T> Centi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .01, "centi", "c");
        }

        public static IUnit<T> Deca<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10, "deca", "da");
        }

        public static IUnit<T> Deci<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .1, "deci", "d");
        }

        public static IUnit<T> Exa<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e18, "exa", "E");
        }

        public static IUnit<T> Exbi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "exbi", "Ei");
        }

        public static IUnit<T> Femto<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-15, "femto", "f");
        }

        public static IUnit<T> Gibi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d, "gibi", "Gi");
        }

        public static IUnit<T> Giga<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e9, "giga", "G");
        }

        public static IUnit<T> Hecto<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 100, "hecto", "h");
        }

        public static IUnit<T> Kibi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d, "kibi", "Ki");
        }

        public static IUnit<T> Kilo<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1000, "kilo", "k");
        }

        public static IUnit<T> Mebi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d, "mebi", "Mi");
        }

        public static IUnit<T> Mega<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e6, "mega", "M");
        }

        public static IUnit<T> Micro<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-6, "micro", "μ");
        }

        public static IUnit<T> Milli<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .001, "milli", "m");
        }

        public static IUnit<T> Nano<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-9, "nano", "n");
        }

        public static IUnit<T> Pebi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d, "pebi", "Pi");
        }

        public static IUnit<T> Peta<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e15, "peta", "P");
        }

        public static IUnit<T> Pico<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-12, "pico", "p");
        }

        public static IUnit<T> Tebi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d, "tebi", "Ti");
        }

        public static IUnit<T> Tera<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e12, "tera", "T");
        }

        public static IUnit<T> Yobi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "yobi", "Yi");
        }

        public static IUnit<T> Zebi<T>(IPrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "zebi", "Zi");
        }

        private static IUnit<T> PrefixIUnit<T>(IPrefixableUnit<T> unit, double multiplier, string namePrefix, string symbolPrefix) where T : Measurement<T> {
            return Unit.Create<T>(
                name: namePrefix + unit.Name,
                symbol: symbolPrefix + unit.Symbol,
                unitsPerDefault: unit.UnitsPerDefault / multiplier
            );
        }
    }
}