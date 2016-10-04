using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JoshuaKearney.Measurements {

    /// <summary>
    /// A class that is able to parse units out of strings. Compatable with all <see cref="Measurement{T}.ToString()"/> outputs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MeasurementParser<T> where T : Measurement<T> {
        private static Regex NumericPattern = new Regex(@"^\d*\.?\d*$");
        private static Dictionary<string, Func<double, MeasurementToken>> dictionary;
        private IMeasurementProvider<T> provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasurementParser{T}"/> class.
        /// </summary>
        /// <param name="provider">
        /// The measurement provider for this type of measurement. Can be found as static members in measurement classes.
        /// Ex: <see cref="Distance.Provider"/>
        /// </param>
        public MeasurementParser(IMeasurementProvider<T> provider) {
            this.provider = provider;
        }

        /// <summary>
        /// Parses the specified string into a measurement of type <see cref="T"/>. Will throw if the
        /// parse failed.
        /// </summary>
        /// <param name="toParse">The string to parse.</param>
        /// <returns></returns>
        public T Parse(string toParse) {
            Validate.NonNull(toParse, nameof(toParse));

            if (dictionary == null) {
                dictionary = GetUnits(this.provider);
            }

            var res = ParseTokens(ToPostfix(Tokenize(toParse)));

            if (res.Item2 != null || res.Item1 == null) {
                throw res.Item2 ?? new FormatException();
            }
            else {
                return res.Item1;
            }
        }

        /// <summary>
        /// Parses the specified string into a measurement of type <see cref="T"/>. Will set result to <see cref="null"/> if the
        /// parse failed.
        /// </summary>
        /// <param name="toParse">The string to parse.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryParse(string toParse, out T result) {
            Validate.NonNull(toParse, nameof(toParse));

            if (dictionary == null) {
                dictionary = GetUnits(this.provider);
            }

            var res = ParseTokens(ToPostfix(Tokenize(toParse)));

            if (res.Item2 != null || res.Item1 == null) {
                result = null;
                return false;
            }
            else {
                result = res.Item1;
                return true;
            }
        }

        private static Tuple<IEnumerable<Token>, Exception> Tokenize(string input) {
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
                        // Bad Unit exception
                        return Tuple.Create<IEnumerable<Token>, Exception>(null, new FormatException($"Unable to tokenize expression. The unit '{unit}' was not recognized"));
                    }
                }
            }

            return Tuple.Create<IEnumerable<Token>, Exception>(ret, null);
        }

        private static Tuple<T, Exception> ParseTokens(Tuple<IEnumerable<Token>, Exception> input) {
            if (input == null || input.Item1 == null) {
                return Tuple.Create<T, Exception>(null, input?.Item2 ?? new FormatException());
            }

            // The iterator to use when parsing
            IEnumerator<Token> tokensEnum = input.Item1.GetEnumerator();

            // The valid tokens that could make up this Measurement
            Stack<Token> toks = new Stack<Token>();

            while (tokensEnum.MoveNext()) {
                Token tok = tokensEnum.Current;

                if (tok is MeasurementToken) {
                    toks.Push(tok);
                }
                else if (tok is UrnaryOperator) {
                    UrnaryOperator op = tok as UrnaryOperator;

                    if (toks.Count < 1) {
                        // Expected a measurement, got nothing
                        return Tuple.Create<T, Exception>(null, new FormatException($"Unable to evaluate urnary operator '{op.ToString()}'. Expected a measurement, got nothing"));
                    }

                    MeasurementToken pop = toks.Pop() as MeasurementToken;

                    if (pop == null) {
                        // Expected a measurement, got an operator
                        return Tuple.Create<T, Exception>(null, new FormatException($"Unable to evaluate urnary operator '{op.ToString()}'. Expected a measurement, got '{pop.ToString()}'"));
                    }

                    MeasurementToken eval = op.Evaluate(pop);

                    if (eval == null) {
                        // Could not evaluate urnary operator
                        return Tuple.Create<T, Exception>(null, new FormatException($"Unable to evaluate urnary operator '{op.ToString()}' on '{pop.ToString()}'. The measurement is incompatable"));
                    }

                    toks.Push(eval);
                }
                else if (tok is BinaryOperator) {
                    BinaryOperator op = tok as BinaryOperator;

                    if (toks.Count < 2) {
                        // Expected a measurement, got nothing
                        return Tuple.Create<T, Exception>(null, new FormatException($"Unable to evaluate binary operator '{op.ToString()}'. Expected two measurements, got {toks.Count}"));
                    }

                    MeasurementToken pop2 = toks.Pop() as MeasurementToken;
                    MeasurementToken pop1 = toks.Pop() as MeasurementToken;

                    if (pop1 == null) {
                        // Expected a measurement, got an operator
                        return Tuple.Create<T, Exception>(null, new FormatException($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, got '{pop1.ToString()}'"));
                    }

                    if (pop2 == null) {
                        // Expected a measurement, got an operator
                        return Tuple.Create<T, Exception>(null, new FormatException($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, got '{pop2.ToString()}'"));
                    }

                    MeasurementToken eval = op.Evaluate(pop1, pop2);

                    if (eval == null) {
                        // Could not evaluate binary operator
                        return Tuple.Create<T, Exception>(
                            null, new FormatException($"Unable to evaluate binary operator '{op.ToString()}' on '{pop1.ToString()}' and '{pop2.ToString()}'. The measurements are incompatable")
                        );
                    }

                    toks.Push(eval);
                }
            }

            object final = (toks.Pop() as MeasurementToken).MeasurementValue;
            var tFinal = final.GetType();

            if (!typeof(T).GetTypeInfo().IsAssignableFrom(tFinal.GetTypeInfo())) {
                // Bad type
                return Tuple.Create<T, Exception>(
                    null, new FormatException($"Unable to evaluate expression. Expected  a {typeof(T).ToString()}, got a {tFinal.ToString()}")
                );
            }

            return Tuple.Create<T, Exception>((T)final, null);
        }

        private static Tuple<IEnumerable<Token>, Exception> ToPostfix(Tuple<IEnumerable<Token>, Exception> input) {
            if (input == null || input.Item1 == null) {
                return Tuple.Create<IEnumerable<Token>, Exception>(null, input?.Item2 ?? new FormatException());
            }

            Stack<Operator> operatorStack = new Stack<Operator>();
            List<Token> ret = new List<Token>();

            foreach (Token tok in input.Item1) {
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
                            return Tuple.Create<IEnumerable<Token>, Exception>(null, new FormatException("Unable to analyze expression: mismatched parenthensis"));
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
                                        return Tuple.Create<IEnumerable<Token>, Exception>(null, new FormatException("Unable to analyze expression: mismatched parenthensis"));
                                    }
                                }

                                // Catch mismatched parenthensis
                                if (operatorStack.Peek() != Operator.OpenParen) {
                                    return Tuple.Create<IEnumerable<Token>, Exception>(null, new FormatException("Unable to analyze expression: mismatched parenthensis"));
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

            return Tuple.Create<IEnumerable<Token>, Exception>(ret, null);
        }

        private static Dictionary<string, Func<double, MeasurementToken>> GetUnits<E>(IMeasurementProvider<E> provider) where E : Measurement<E> {
            Dictionary<string, Func<double, MeasurementToken>> ret = provider
                .AllUnits
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
                        y => {
                            return new MeasurementToken(provider.CreateMeasurement(y, x));
                        })
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

                var prov1Ret = (Dictionary<string, Func<double, MeasurementToken>>)prov1Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider) });
                foreach (var item in prov1Ret.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov1Ret[item]);
                    }
                }

                var prov2Ret = (Dictionary<string, Func<double, MeasurementToken>>)prov2Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider) });
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