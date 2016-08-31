using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static class Prefix {

        public static IEnumerable<Unit<T>> All<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
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

        public static Unit<T> Atto<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-18, "atto", "a");
        }

        public static Unit<T> Centi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .01, "centi", "c");
        }

        public static Unit<T> Deca<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10, "deca", "da");
        }

        public static Unit<T> Deci<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .1, "deci", "d");
        }

        public static Unit<T> Exa<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e18, "exa", "E");
        }

        public static Unit<T> Exbi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "exbi", "Ei");
        }

        public static Unit<T> Femto<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-15, "femto", "f");
        }

        public static Unit<T> Gibi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d, "gibi", "Gi");
        }

        public static Unit<T> Giga<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e9, "giga", "G");
        }

        public static Unit<T> Hecto<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 100, "hecto", "h");
        }

        public static Unit<T> Kibi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d, "kibi", "Ki");
        }

        public static Unit<T> Kilo<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1000, "kilo", "k");
        }

        public static Unit<T> Mebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d, "mebi", "Mi");
        }

        public static Unit<T> Mega<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e6, "mega", "M");
        }

        public static Unit<T> Micro<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-6, "micro", "μ");
        }

        public static Unit<T> Milli<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .001, "milli", "m");
        }

        public static Unit<T> Nano<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-9, "nano", "n");
        }

        public static Unit<T> Pebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d, "pebi", "Pi");
        }

        public static Unit<T> Peta<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e15, "peta", "P");
        }

        public static Unit<T> Pico<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e-12, "pico", "p");
        }

        public static Unit<T> Tebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d, "tebi", "Ti");
        }

        public static Unit<T> Tera<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10e12, "tera", "T");
        }

        public static Unit<T> Yobi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "yobi", "Yi");
        }

        public static Unit<T> Zebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "zebi", "Zi");
        }

        private static Unit<T> PrefixIUnit<T>(PrefixableUnit<T> unit, double multiplier, string namePrefix, string symbolPrefix) where T : Measurement<T> {
            return new Unit<T>(
                name: namePrefix + unit.Name,
                symbol: symbolPrefix + unit.Symbol,
                unitsPerDefault: unit.UnitsPerDefault / multiplier
            );
        }
    }
}