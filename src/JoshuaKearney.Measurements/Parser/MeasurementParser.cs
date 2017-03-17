using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using JoshuaKearney.Measurements.Parser.Lexing;

namespace JoshuaKearney.Measurements.Parser {
    public class MeasurementParser<T> where T : IMeasurement<T> {
        private MeasurementProvider<T> Provider { get; }
        private IEnumerable<Operator> Operators { get; }
        private IReadOnlyDictionary<string, object> Units { get; }

        private static Lexer Lexer { get; } = new Lexer();
        private static EvaluationParser EvalParser { get; } = new EvaluationParser();

        public MeasurementParser(MeasurementProvider<T> provider, params Operator[] operators) : this(provider, (IEnumerable<Operator>)operators) { }

        public MeasurementParser(MeasurementProvider<T> provider, IEnumerable<Operator> ops) {
            Validate.NonNull(provider, nameof(provider));
            Validate.NonNull(ops, nameof(ops));

            this.Provider = provider;
            var info = GetProviderInfo(provider);
            this.Units = info.Item1
                .Concat(new Dictionary<string, object>() {
                    { "NaN", provider.NaN },
                    { "Infinity", provider.PositiveInfinity },
                    { "PositiveInfinity", provider.PositiveInfinity },
                    { "NegativeInfinity", provider.PositiveInfinity }
                })
                .Concat(DoubleMeasurement.Provider.ParsableUnits.Select(x => new KeyValuePair<string, object>(x.ToString(), x.ToMeasurement())))
                .ToDictionary(x => x.Key, x => x.Value);

            this.Operators = info.Item2.Concat(ops).Concat(DoubleMeasurement.Provider.ParseOperators).Distinct();
        }

        public T Parse(string str) {
            Validate.NonNull(str, nameof(str));

            if (this.Parse(str, out T result, out ParseException exp)) {
                return result;
            }
            else {
                throw exp;
            }
        }

        public bool TryParse(string str, out T success) {
            Validate.NonNull(str, nameof(str));


            if (this.Parse(str, out success, out ParseException exp)) {
                return true;
            }
            else {
                return false;
            }
        }

        private bool Parse(string str, out T success, out ParseException failure) {
            Validate.NonNull(str, nameof(str));


            if (Lexer.TryGetTokens(str, out IEnumerable<Token> toks, out failure)) {
                if (EvalParser.TryParse(toks, this.Operators, this.Units, out object result, out failure)) {
                    if (result is T) {
                        success = (T)result;
                        failure = null;
                        return true;
                    }
                    else {
                        failure = ParseException.TypeConversionError(result.ToString(), typeof(T).ToString());
                    }
                }
            }

            success = default(T);
            failure = failure ?? ParseException.UnspecifiedError();
            return false;
        }

        private static Tuple<Dictionary<string, object>, IEnumerable<Operator>> GetProviderInfo<E>(MeasurementProvider<E> provider) where E : IMeasurement<E> {
            Validate.NonNull(provider, nameof(provider));

            Dictionary<string, object> ret =
                provider
                .ParsableUnits                .SelectMany(x => {
                    if (x is PrefixableUnit<E>) {
                        return Prefix.All(x as PrefixableUnit<E>).Concat(new[] { x });
                    }
                    else {
                        return new[] { x };
                    }
                })
                .Distinct()
                .Select(
                    x => Tuple.Create(x.ToString(), (object)x.ToMeasurement())
                )
                .ToDictionary(x => x.Item1, y => y.Item2);

            IEnumerable<Operator> ops = provider.ParseOperators;

            if (provider.GetType().GetTypeInfo().BaseType != null && provider.GetType().GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(CompoundMeasurementProvider<,,>)) {
                MethodInfo info = typeof(MeasurementParser<T>).GetRuntimeMethods().First(x => x.Name == nameof(GetProviderInfo));

                object provider1 = provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider);
                object provider2 = provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider);

                MethodInfo prov1Info = info.MakeGenericMethod(new[] {
                    provider1
                    .GetType()
                    .GetTypeInfo()
                    .BaseType
                    .GenericTypeArguments[0]
                });

                MethodInfo prov2Info = info.MakeGenericMethod(new[] {
                    provider2
                    .GetType()
                    .GetTypeInfo()
                    .BaseType
                    .GenericTypeArguments[0]
                });

                var prov1Ret = (Tuple<Dictionary<string, object>, IEnumerable<Operator>>)prov1Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider) });
                foreach (string item in prov1Ret.Item1.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov1Ret.Item1[item]);
                    }
                }

                ops = ops.Concat(prov1Ret.Item2).Distinct();

                var prov2Ret = (Tuple<Dictionary<string, object>, IEnumerable<Operator>>)prov2Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider) });
                foreach (string item in prov2Ret.Item1.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov2Ret.Item1[item]);
                    }
                }

                ops = ops.Concat(prov2Ret.Item2).Distinct();
            }

            return Tuple.Create(ret, ops);
        }
    }
}