using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    public class MeasurementParser<T> where T : Measurement<T> {
        private Regex NumericPattern = new Regex(@"^\d*\.?\d*$");
        private Dictionary<string, Func<double, MeasurementToken>> dictionary;
        private IMeasurementProvider<T> provider;

        public MeasurementParser(IMeasurementProvider<T> provider) {
            this.provider = provider;
        }

        public T Parse(string toParse) {
            Validate.NonNull(toParse, nameof(toParse));

            if (dictionary == null) {
                dictionary = GetUnits(this.provider);
            }

            return ParseTokens(ToPostfix(Tokenize(toParse)));
        }

        private List<Token> Tokenize(string input) {
            Validate.NonNull(input, nameof(input));

            input = input
                .Replace("^2", "²")
                .Replace("^3", "³")
                .Replace(" ", "");

            List<Token> ret = new List<Token>();
            string notLetters = "*/²³()";
            string current = "";

            foreach (char c in input) {
                if (notLetters.Cast<char>().All(x => x != c)) {
                    current += c;
                }
                else {
                    if (c != '(' && !string.IsNullOrWhiteSpace(current)) {
                        ret.Add(new Token(current));
                        current = "";
                    }

                    switch (c) {
                        case '*': ret.Add(Operator.Multiply); break;
                        case '/': ret.Add(Operator.Divide); break;
                        case '²': ret.Add(Operator.Square); break;
                        case '³': ret.Add(Operator.Cube); break;
                        case '(': ret.Add(Operator.OpenParen); break;
                        case ')': ret.Add(Operator.CloseParen); break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(current)) {
                ret.Add(new Token(current));
            }

            for (int i = 0; i < ret.Count; i++) {
                var token = ret[i];

                if (token.GetType() == typeof(Token)) {
                    string val = token.Value;
                    string strNum = string.Join("", val.ToCharArray().TakeWhile(x => char.IsDigit(x) || x == '.'));

                    string unit = val.Substring(strNum.Length);
                    double value;

                    if (!double.TryParse(strNum, out value)) {
                        value = 1;
                    }

                    if (dictionary.ContainsKey(unit)) {
                        ret[i] = dictionary[unit](value);
                    }
                    else {
                        return null;
                    }
                }
            }

            return ret;
        }

        private T ParseTokens(IEnumerable<Token> tokens) {
            if (tokens == null) {
                return null;
            }

            // The iterator to use when parsing
            IEnumerator<Token> tokensEnum = tokens.GetEnumerator();

            // The valid tokens that could make up this Measurement
            Stack<Token> toks = new Stack<Token>();

            while (tokensEnum.MoveNext()) {
                Token tok = tokensEnum.Current;

                if (tok is MeasurementToken) {
                    toks.Push(tok);
                }
                else if (tok is UrnaryOperator) {
                    UrnaryOperator op = tok as UrnaryOperator;

                    if (toks.Count < 1 || !(toks.Peek() is MeasurementToken)) {
                        return null;
                    }

                    MeasurementToken eval = op.Evaluate(toks.Pop() as MeasurementToken);

                    if (eval == null) {
                        return null;
                    }

                    toks.Push(eval);
                }
                else if (tok is BinaryOperator) {
                    BinaryOperator op = tok as BinaryOperator;

                    if (toks.Count < 2) {
                        return null;
                    }

                    MeasurementToken pop2 = toks.Pop() as MeasurementToken;
                    MeasurementToken pop1 = toks.Pop() as MeasurementToken;

                    if (!(pop1 is MeasurementToken && pop2 is MeasurementToken)) {
                        return null;
                    }

                    MeasurementToken eval = op.Evaluate(pop1, pop2);

                    if (eval == null) {
                        return null;
                    }

                    toks.Push(eval);
                }
            }

            object final = (toks.Pop() as MeasurementToken).MeasurementValue;
            var tFinal = final.GetType();

            if (!typeof(T).GetTypeInfo().IsAssignableFrom(tFinal.GetTypeInfo())) {
                return null;
            }

            return (T)final;
        }

        private List<Token> ToPostfix(IEnumerable<Token> input) {
            Stack<Operator> operatorStack = new Stack<Operator>();
            List<Token> ret = new List<Token>();

            foreach (Token tok in input) {
                // Push operands
                if (!(tok is Operator)) {
                    ret.Add(tok);
                }
                else {
                    Operator opTok = tok as Operator;

                    if (opTok == Operator.OpenParen) {
                        operatorStack.Push(opTok);
                        continue;
                    }

                    // If the stack is empty, put an operator on it
                    if ((operatorStack.Count == 0 || operatorStack.Peek() == Operator.OpenParen) && opTok != Operator.CloseParen) {
                        operatorStack.Push(opTok);
                    }
                    else {
                        // Mismatched parenthensis
                        if (operatorStack.Count == 0) {
                            return null;
                        }

                        // If the top has a higher priority, pop and print it
                        Operator top = operatorStack.Peek();
                        if (top.Priority >= opTok.Priority) {
                            ret.Add(operatorStack.Pop());
                            operatorStack.Push(opTok);
                        }
                        else {
                            if (opTok == Operator.CloseParen) {
                                while (operatorStack.Peek() != Operator.OpenParen) {
                                    ret.Add(operatorStack.Pop());
                                    // Catch nothing else after this
                                    if (operatorStack.Count == 0) {
                                        return null;
                                    }
                                }

                                // Catch mismatched parenthensis
                                if (operatorStack.Peek() != Operator.OpenParen) {
                                    return null;
                                }

                                operatorStack.Pop();
                            }
                            else {
                                operatorStack.Push(opTok);
                            }
                        }
                    }
                }
            }

            while (operatorStack.Count > 0) {
                ret.Add(operatorStack.Pop());
            }

            return ret;
        }

        private Dictionary<string, Func<double, MeasurementToken>> GetUnits<E>(IMeasurementProvider<E> provider) where E : Measurement<E> {
            Dictionary<string, Func<double, MeasurementToken>> ret = provider
                .BaseUnits
                .SelectMany(x => {
                    if (x is PrefixableUnit<E>) {
                        return Prefix.All(x as PrefixableUnit<E>).Concat(new[] { x });
                    }
                    else {
                        return new[] { x };
                    }
                })
                .Select(
                    x => Tuple.Create<string, Func<double, MeasurementToken>>(
                        x.Symbol,
                        y => new MeasurementToken(provider.CreateMeasurement(y, x))
                    )
                )
                .ToDictionary(x => x.Item1, y => y.Item2);

            var complexInferface = typeof(IComplexMeasurementProvider<,>);
            var foundInterface = provider.GetType().GetTypeInfo().ImplementedInterfaces.FirstOrDefault(x => x.GetGenericTypeDefinition() == complexInferface);

            if (foundInterface != null) {
                MethodInfo info = typeof(MeasurementParser<T>).GetRuntimeMethods().First(x => x.Name == nameof(GetUnits));

                object provider1 = provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider);
                object provider2 = provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider);

                MethodInfo prov1Info = info.MakeGenericMethod(new[] { provider1.GetType().GetTypeInfo().ImplementedInterfaces.First(x => x.GetGenericTypeDefinition() == typeof(IMeasurementProvider<>)).GenericTypeArguments[0] });
                MethodInfo prov2Info = info.MakeGenericMethod(new[] { provider2.GetType().GetTypeInfo().ImplementedInterfaces.First(x => x.GetGenericTypeDefinition() == typeof(IMeasurementProvider<>)).GenericTypeArguments[0] });

                var prov1Ret = (Dictionary<string, Func<double, MeasurementToken>>)prov1Info.Invoke(this, new[] { provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider) });
                foreach (var item in prov1Ret.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov1Ret[item]);
                    }
                }

                var prov2Ret = (Dictionary<string, Func<double, MeasurementToken>>)prov2Info.Invoke(this, new[] { provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider) });
                foreach (var item in prov2Ret.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov2Ret[item]);
                    }
                }
            }

            return ret;
        }
    }
}