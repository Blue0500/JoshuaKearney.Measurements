using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using JoshuaKearney.Measurements.Parser.Lexing;
using JoshuaKearney.Measurements.Parser.Syntax;
using JoshuaKearney.Measurements.Parser.Evaluating;

namespace JoshuaKearney.Measurements.Parser {
    public class MeasurementParser<T> where T : Measurement<T> {
        public MeasurementProvider<T> Provider { get; }

        public IEnumerable<Operator> Operators { get; }

        internal IReadOnlyDictionary<string, IMeasurement> Units { get; }

        private static Lexing.Lexer lex = new Lexing.Lexer();
        private static TokenParser parser = new TokenParser();
        private static EvaluationVisitor interpreter = new EvaluationVisitor();

        public MeasurementParser(MeasurementProvider<T> provider) : this(provider, new Operator[0]) { }

        public MeasurementParser(MeasurementProvider<T> provider, IEnumerable<Operator> ops) {
            this.Provider = provider;

            var units = GetUnits(provider);
            this.Units = units.Item1;
            this.Operators = units.Item2.Concat(ops).Distinct();
        }

        public T Parse(string str) {
            T result;
            ParseException exp;

            if (this.Parse(str, out result, out exp)) {
                return result;
            }
            else {
                throw exp;
            }
        }

        public bool TryParse(string str, out T success) {
            T result;
            ParseException exp;

            if (this.Parse(str, out result, out exp)) {
                success = result;
                return true;
            }
            else {
                success = null;
                return false;
            }
        }

        private bool Parse(string str, out T success, out ParseException failure) {
            IEnumerable<Token> toks;
            AbstractSyntaxTree tree;
            IMeasurement result;

            if (lex.TryGetTokens(str, out toks, out failure)) {
                if (parser.TryParse(toks, out tree, out failure)) {
                    if (interpreter.TryInterpret(tree, this.Operators, this.Units, out result, out failure)) {
                        if (result is T) {
                            success = (T)result;
                            failure = null;
                            return true;
                        }
                        else {
                            failure = ParseException.TypeConversionError(result.GetType(), typeof(T));
                        }
                    }
                }
            }

            success = null;
            failure = failure ?? new ParseException("");
            return false;
        }

        private static Tuple<Dictionary<string, IMeasurement>, IEnumerable<Operator>> GetUnits<E>(MeasurementProvider<E> provider) where E : Measurement<E> {
            Dictionary<string, IMeasurement> ret =
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
                    x => Tuple.Create(x.ToString(), (IMeasurement)x.ToMeasurement())
                )
                .ToDictionary(x => x.Item1, y => y.Item2);

            IEnumerable<Operator> ops = provider.ParseOperators;

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

                var prov1Ret = (Tuple<Dictionary<string, IMeasurement>, IEnumerable<Operator>>)prov1Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider) });
                foreach (var item in prov1Ret.Item1.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov1Ret.Item1[item]);
                    }
                }

                ops = ops.Concat(prov1Ret.Item2).Distinct();

                var prov2Ret = (Tuple<Dictionary<string, IMeasurement>, IEnumerable<Operator>>)prov2Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider) });
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