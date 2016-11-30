using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JoshuaKearney.Measurements {

    public sealed class MeasurementParser<T> where T : Measurement<T> {
        private static Regex NumericPattern { get; } = new Regex(@"^\d*\.?\d*$");

        private static Dictionary<string, Func<double, MeasurementToken>> dictionary;

        private static IMeasurementProvider<T> provider;

        public MeasurementParser(IMeasurementProvider<T> p) {
            if (provider == null) {
                provider = p;
            }

            if (dictionary == null) {
                dictionary = GetUnits(provider);
            }
        }

        public T Parse(string input) {
            var res = EvalPostfix(ToPostfix(EvalInfix(Tokenize(input), input), input), input);
            T ret = default(T);

            res.Match(
                x => ret = x,
                y => { throw y; }
            );

            return ret;
        }

        private static Union<T, MeasurementParseException> EvalPostfix(Union<IEnumerable<Token>, MeasurementParseException> input, string rawInput) {
            return input.Select(
                list => {
                    // The iterator to use when parsing
                    IEnumerator<Token> tokensEnum = list.GetEnumerator();

                    // The valid tokens that could make up this Measurement
                    Stack<Token> toks = new Stack<Token>();

                    while (tokensEnum.MoveNext()) {
                        Token tok = tokensEnum.Current;

                        if (tok is MeasurementToken) {
                            toks.Push(tok);
                        }
                        else if (tok is UrnaryOperator) {
                            return new MeasurementParseException<T>($"Unable to evaluate: Unexpected urnary operator '{tok}'", rawInput);
                            //return new InvalidOperationException($"Unable to evaluate: Unexpected urnary operator '{tok}'");
                            //UrnaryOperator op = tok as UrnaryOperator;

                            //if (toks.Count < 1) {
                            //    // Expected a measurement, got nothing
                            //    return new Union<T, Exception>(new FormatException($"Unable to evaluate urnary operator '{op.ToString()}'. Expected a measurement, got nothing"));
                            //}

                            //MeasurementToken pop = toks.Pop() as MeasurementToken;

                            //if (pop == null) {
                            //    // Expected a measurement, got an operator
                            //    return new Union<T, Exception>(new FormatException($"Unable to evaluate urnary operator '{op.ToString()}'. Expected a measurement, got '{pop.ToString()}'"));
                            //}

                            //MeasurementToken eval = op.Evaluate(pop);

                            //if (eval == null) {
                            //    // Could not evaluate urnary operator
                            //    return new Union<T, Exception>(new FormatException($"Unable to evaluate urnary operator '{op.ToString()}' on '{pop.ToString()}'. The measurement is incompatable"));
                            //}

                            //toks.Push(eval);
                        }
                        else if (tok is BinaryOperator) {
                            BinaryOperator op = tok as BinaryOperator;

                            if (toks.Count < 2) {
                                // Expected a measurement, got nothing
                                return new MeasurementParseException<T>($"Unable to evaluate binary operator '{op.ToString()}'. Expected two measurements, received {toks.Count}", rawInput);
                               // return new FormatException($"Unable to evaluate binary operator '{op.ToString()}'. Expected two measurements, got {toks.Count}");
                            }

                            MeasurementToken pop2 = toks.Pop() as MeasurementToken;
                            MeasurementToken pop1 = toks.Pop() as MeasurementToken;

                            if (pop1 == null) {
                                // Expected a measurement, got an operator
                                return new MeasurementParseException<T>($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, received '{pop1.ToString()}'", rawInput);
                                //return new FormatException($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, got '{pop1.ToString()}'");
                            }

                            if (pop2 == null) {
                                // Expected a measurement, got an operator
                                return new MeasurementParseException<T>($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, received '{pop2.ToString()}'", rawInput);
                                //return new FormatException($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, got '{pop2.ToString()}'");
                            }

                            MeasurementToken eval = op.Evaluate(pop1, pop2);

                            if (eval == null) {
                                // Could not evaluate binary operators
                                return new MeasurementParseException<T>(
                                    $"Unable to evaluate binary operator '{op.ToString()}' on '{pop1.ToString()}' and '{pop2.ToString()}'. The measurements are incompatable", 
                                    rawInput
                                );
                                //return new FormatException($"Unable to evaluate binary operator '{op.ToString()}' on '{pop1.ToString()}' and '{pop2.ToString()}'. The measurements are incompatable");
                            }

                            toks.Push(eval);
                        }
                    }

                    object final = (toks.Pop() as MeasurementToken).MeasurementValue;
                    var tFinal = final.GetType();

                    if (toks.Count > 0) {
                        return new MeasurementParseException<T>($"Unable to evaluate expression: more operators are required to evaluate all values. Values '{string.Join(", ", toks)}' are unevaluated.", rawInput);
                    }

                    if (!typeof(T).GetTypeInfo().IsAssignableFrom(tFinal.GetTypeInfo())) {
                        if (tFinal == typeof(DoubleMeasurement)) {
                            double d = final as DoubleMeasurement;
                            if (double.IsNaN(d) || double.IsInfinity(d)) {
                                return provider.CreateMeasurementWithDefaultUnits(d);
                            }
                        }

                        // Bad type
                        return new MeasurementParseException<T>($"Unable to evaluate expression. Expected  a {typeof(T).ToString()}, received a {tFinal.ToString()}", rawInput);
                        //return new FormatException($"Unable to evaluate expression. Expected  a {typeof(T).ToString()}, received a {tFinal.ToString()}");
                    }

                    return new Union<T, MeasurementParseException>(final as T);
                },
                exception => exception
            );
        }

        private static Union<IEnumerable<Token>, MeasurementParseException> ToPostfix(Union<IEnumerable<Token>, MeasurementParseException> input, string rawInput) {
            return input.Select(
                list => {
                    Stack<Operator> operatorStack = new Stack<Operator>();
                    List<Token> ret = new List<Token>();

                    foreach (Token tok in list) {
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
                                    return new MeasurementParseException<T>("Unable to analyze expression: mismatched parenthensis", rawInput);
                                    //return new Union<IEnumerable<Token>, Exception>(new FormatException("Unable to analyze expression: mismatched parenthensis"));
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
                                                return new MeasurementParseException<T>("Unable to analyze expression: mismatched parenthensis", rawInput);
                                                //return new Union<IEnumerable<Token>, Exception>(new FormatException("Unable to analyze expression: mismatched parenthensis"));
                                            }
                                        }

                                        // Catch mismatched parenthensis
                                        if (operatorStack.Peek() != Operator.OpenParen) {
                                            return new MeasurementParseException<T>("Unable to analyze expression: mismatched parenthensis", rawInput);
                                           // return new Union<IEnumerable<Token>, Exception>(new FormatException("Unable to analyze expression: mismatched parenthensis"));
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
                },
                exception => input
            );
        }

        private Union<List<Token>, MeasurementParseException> Tokenize(string input) {
            Validate.NonNull(input, nameof(input));

            input = input
                .Replace("^2", "²")
                .Replace("^3", "³")
                .Replace(" ", "");

            List<string> split = new List<string>();
            bool isCurrentNum = false;
            string current = "";
            string operators = "²³/*()";

            foreach (char c in input) {
                if (char.IsDigit(c) || c == '.') {
                    if (isCurrentNum || string.IsNullOrWhiteSpace(current)) {
                        current += c;
                    }
                    else {
                        split.Add(current);
                        current = c.ToString();
                    }

                    isCurrentNum = true;
                }
                else if (!operators.ToCharArray().Any(x => x == c)) {
                    if (isCurrentNum && !string.IsNullOrWhiteSpace(current)) {
                        split.Add(current);
                        current = c.ToString();
                    }
                    else {
                        current += c;
                    }

                    isCurrentNum = false;
                }
                else {
                    if (!string.IsNullOrWhiteSpace(current)) {
                        split.Add(current);
                        current = "";
                    }

                    split.Add(c.ToString());
                    isCurrentNum = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(current)) {
                split.Add(current);
            }

            List<Token> ret = new List<Token>();
            foreach (string item in split) {
                // Operator
                if (operators.ToCharArray().Any(x => x.ToString() == item)) {
                    if (item == "²") {
                        ret.Add(Operator.Square);
                    }
                    else if (item == "³") {
                        ret.Add(Operator.Cube);
                    }
                    else if (item == "*") {
                        ret.Add(Operator.Multiply);
                    }
                    else if (item == "/") {
                        ret.Add(Operator.Divide);
                    }
                    else if (item == "(") {
                        ret.Add(Operator.OpenParen);
                    }
                    else if (item == ")") {
                        ret.Add(Operator.CloseParen);
                    }
                    else {
                        return new Union<List<Token>, MeasurementParseException>(
                            new MeasurementParseException<T>($"Unable to recognize operator '{item}'", input)
                        );
                    }
                }
                // Number
                else if (item.ToCharArray().All(x => char.IsDigit(x) || x == '.')) {
                    double d;
                    if (!double.TryParse(item, out d)) {
                        return new Union<List<Token>, MeasurementParseException>(
                            new MeasurementParseException<T>($"Unable to parse double '{item}'", input)    
                        );
                    }

                    ret.Add(new DoubleMeasurementToken(new DoubleMeasurement(d)));
                }
                // Unit
                else {
                    if (item == double.NaN.ToString()) {
                        ret.Add(new DoubleMeasurementToken(new DoubleMeasurement(double.NaN)));
                    }
                    else {
                        ret.Add(new UnitToken(item));
                    }
                }
            }

            return new Union<List<Token>, MeasurementParseException>(ret);
        }

        private Union<IEnumerable<Token>, MeasurementParseException> EvalInfix(Union<List<Token>, MeasurementParseException> input, string rawInput) {
            return input.Select(
                list => {
                    // Apply squares and cubes to measurements
                    for (int i = 0; i < list.Count; i++) {
                        var item = list[i];

                        if (item is UrnaryOperator) {
                            UrnaryOperator op = item as UrnaryOperator;

                            if (i > 0 && list[i - 1] is UnitToken) {
                                UnitToken tok = list[i - 1] as UnitToken;

                                list.RemoveAt(i);

                                int times = (op == Operator.Square ? 2 : 3);
                                for (int c = 0; c < times - 1; c++) {

                                    list.Insert(i, tok);
                                    list.Insert(i, Operator.Multiply);
                                }

                                //if (res == null) {
                                //    return new MeasurementParseException<T>($"Unable to apply urnary operator '{op}' on unit '{tok.MeasurementValue}'", rawInput, string.Join("", list));
                                //}
                                //else {
                                //    list[i - 1] = res;
                                //    list.RemoveAt(i);
                                //    i--;
                                //}
                            }
                            else {
                                return new MeasurementParseException<T>($"Unable to apply urnary operator '{op}', no preceding unit", rawInput);
                            }
                        }
                    }


                    // Apply measurement labels to preceding doubles
                    for (int i = 0; i < list.Count; i++) {
                        var item = list[i];

                        // Top-level tokens are always measurement labels at this step
                        if (item is UnitToken) {
                            // Label value
                            string unit = list[i].Value;
                            if (!dictionary.ContainsKey(unit)) {
                                return new MeasurementParseException<T>($"Unable to recognize unit '{unit}'", rawInput);
                            }

                            // The token before is a double token, in which case the measurement must be applied to it
                            if (i > 0 && list[i - 1] is DoubleMeasurementToken) {
                                double d = (list[i - 1] as DoubleMeasurementToken).DoubleValue;

                                list[i - 1] = dictionary[unit](d);
                                list.RemoveAt(i);
                                i--;
                            }
                            else { // Else a implicit 1 is added
                                list[i] = dictionary[unit](1);
                            }
                        }
                    }                 

                    return new Union<IEnumerable<Token>, MeasurementParseException>(list);
                },
                exception => exception
            );
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