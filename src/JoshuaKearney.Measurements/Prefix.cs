using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public static class Prefix {

        /// <summary>
        /// Returns an IEnumerable with every prefixed unit, based on the specified unit.
        /// </summary>
        /// <typeparam name="T">The type of measurement</typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Represents 1e-18 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Atto<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e-18, "atto", "a");
        }

        /// <summary>
        /// Represents .01 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Centi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .01, "centi", "c");
        }

        /// <summary>
        /// Represents 10 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Deca<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 10, "deca", "da");
        }

        /// <summary>
        /// Represents .1 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Deci<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .1, "deci", "d");
        }

        /// <summary>
        /// Represents 1e18 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Exa<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e18, "exa", "E");
        }

        /// <summary>
        /// Represents 2^60 or 1024^6 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Exbi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "exbi", "Ei");
        }

        /// <summary>
        /// Represents 1e-15 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Femto<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e-15, "femto", "f");
        }

        /// <summary>
        /// Represents 2^30 or 1024^3 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Gibi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d, "gibi", "Gi");
        }

        /// <summary>
        /// Represents 10e9 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Giga<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e9, "giga", "G");
        }

        /// <summary>
        /// Represents 100 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Hecto<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 100, "hecto", "h");
        }

        /// <summary>
        /// Represents 2^10 or 1024 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Kibi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d, "kibi", "Ki");
        }

        /// <summary>
        /// Represents 1000 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Kilo<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1000, "kilo", "k");
        }

        /// <summary>
        /// Represents 2^20 or 1024^2 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Mebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d, "mebi", "Mi");
        }

        /// <summary>
        /// Represents 1e6 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Mega<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e6, "mega", "M");
        }

        /// <summary>
        /// Represents 1e-6 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Micro<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e-6, "micro", "μ");
        }

        /// <summary>
        /// Represents .001 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Milli<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, .001, "milli", "m");
        }

        /// <summary>
        /// Represents 1e-9 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Nano<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e-9, "nano", "n");
        }

        /// <summary>
        /// Represents 2^50 or 1024^5 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Pebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d, "pebi", "Pi");
        }

        /// <summary>
        /// Represents 1e15 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Peta<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e15, "peta", "P");
        }

        /// <summary>
        /// Represents 1e-12 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Pico<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e-12, "pico", "p");
        }

        /// <summary>
        /// Represents 2^40 or 1024^4 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Tebi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d, "tebi", "Ti");
        }

        /// <summary>
        /// Represents 1e12 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Tera<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1e12, "tera", "T");
        }

        /// <summary>
        /// Represents 2^80 or 1024^8 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static Unit<T> Yobi<T>(PrefixableUnit<T> unit) where T : Measurement<T> {
            Validate.NonNull(unit, nameof(unit));
            return PrefixIUnit(unit, 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d * 1024d, "yobi", "Yi");
        }

        /// <summary>
        /// Represents 2^70 or 1024^7 base units
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
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