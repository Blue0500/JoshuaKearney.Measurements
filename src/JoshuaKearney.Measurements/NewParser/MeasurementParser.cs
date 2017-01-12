using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using JoshuaKearney.Measurements.NewParser.Lexer;
using JoshuaKearney.Measurements.NewParser.Interpreter;

namespace JoshuaKearney.Measurements.NewParser {
    public class MeasurementParser<T> where T : Measurement<T> {
        public MeasurementProvider<T> Provider { get; }

        public IEnumerable<ParsingOperator> Operators { get; }

        internal IReadOnlyDictionary<string, object> Units { get; }

        private static Lexer.Lexer lex = new Lexer.Lexer();
        private static TokenParser parser = new TokenParser();
        private static EvaulateVisitor interpreter = new EvaulateVisitor();

        public MeasurementParser(MeasurementProvider<T> provider) : this(provider, new ParsingOperator[0]) { }

        public MeasurementParser(MeasurementProvider<T> provider, IEnumerable<ParsingOperator> ops) {
            this.Provider = provider;

            var units = GetUnits(provider);
            this.Units = units.Item1;
            this.Operators = units.Item2.Concat(ops).Distinct();
        }

        public T Parse(string str) {
            var res = interpreter.TryInterpret(parser.Parse(lex.GetTokens(str)), this.Operators, this.Units);

            T ret = null;
            res.Match(x => ret = x as T, x => { throw x; });

            return ret;
        }

        public bool TryParse(string str, out T result) {
            var res = interpreter.TryInterpret(parser.Parse(lex.GetTokens(str)), this.Operators, this.Units);

            T ret = null;
            res.Match(obj => ret = obj as T, exp => ret = null);

            result = ret;
            return result != null;
        }

        private static Tuple<Dictionary<string, object>, IEnumerable<ParsingOperator>> GetUnits<E>(MeasurementProvider<E> provider) where E : Measurement<E> {
            Dictionary<string, object> ret =
                provider
                .ParsableUnits
                .SelectMany(x => {
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

            IEnumerable<ParsingOperator> ops = provider.ParseOperators;

            if (provider.GetType().GetTypeInfo().BaseType != null && provider.GetType().GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(CompoundMeasurementProvider<,,>)) {
                MethodInfo info = typeof(MeasurementParser<T>).GetRuntimeMethods().First(x => x.Name == nameof(GetUnits));

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

                var prov1Ret = (Tuple<Dictionary<string, object>, IEnumerable<ParsingOperator>>)prov1Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider) });
                foreach (var item in prov1Ret.Item1.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov1Ret.Item1[item]);
                    }
                }

                ops = ops.Concat(prov1Ret.Item2).Distinct();

                var prov2Ret = (Tuple<Dictionary<string, object>, IEnumerable<ParsingOperator>>)prov2Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider) });
                foreach (var item in prov2Ret.Item1.Keys) {
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
