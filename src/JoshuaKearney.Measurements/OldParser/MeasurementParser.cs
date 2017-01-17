using JoshuaKearney.Measurements.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JoshuaKearney.Measurements.OldParser {

    public sealed class MeasurementParser<T> where T : Measurement<T> {
        private static Regex NumericPattern { get; } = new Regex(@"^\d*\.?\d*$");

        public static IEnumerable<MeasurementRelationship> DefaultRelationships {
            get {
                return new List<MeasurementRelationship>() {
                    MeasurementRelationship.CreateMultiplicative<Distance, Distance, Area>((x, y) => x.Multiply(y)),
                    MeasurementRelationship.CreateMultiplicative<Distance, Area, Volume>((x, y) => x.Multiply(y)),
                    MeasurementRelationship.CreateMultiplicative<Area, Distance, Volume>((x, y) => x.Multiply(y)),
                    MeasurementRelationship.CreateMultiplicative<Distance, Area, Volume>((x, y) => x.Multiply(y)),
                };
            }
        }

        private Dictionary<string, MeasurementToken> dictionary;

        private MeasurementProvider<T> provider;

        public MeasurementParser(MeasurementProvider<T> p) {
                provider = p;
                dictionary = GetUnits(provider);
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

        public bool TryParse(string input, out T result) {
            var res = EvalPostfix(ToPostfix(EvalInfix(Tokenize(input), input), input), input);

            T ret = res.Select(obj => obj, exeption => null);

            result = ret;
            return ret != null;
        }

        private Union<T, MeasurementParseException> EvalPostfix(Union<IEnumerable<Token>, MeasurementParseException> input, string rawInput) {
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
                            UrnaryOperator op = tok as UrnaryOperator;

                            if (toks.Count < 1) {
                                // Expected a measurement, got nothing
                                return new MeasurementParseException<T>($"Unable to evaluate urnary operator '{op.ToString()}'. Expected a measurement, got nothing", rawInput);
                            }

                            MeasurementToken pop = toks.Pop() as MeasurementToken;

                            if (pop == null) {
                                // Expected a measurement, got an operator
                                return new MeasurementParseException<T>($"Unable to evaluate urnary operator '{op.ToString()}'. Expected a measurement, got '{pop.ToString()}'", rawInput);
                            }

                            MeasurementToken eval = op.Evaluate(pop);

                            if (eval == null) {
                                // Could not evaluate urnary operator
                                return new MeasurementParseException<T>($"Unable to evaluate urnary operator '{op.ToString()}' on '{pop.ToString()}'. The measurement is incompatable", rawInput);
                            }

                            toks.Push(eval);
                        }
                        else if (tok is BinaryOperator) {
                            BinaryOperator op = tok as BinaryOperator;

                            if (toks.Count < 2) {
                                // Expected a measurement, got nothing
                                return new MeasurementParseException<T>($"Unable to evaluate binary operator '{op.ToString()}'. Expected two measurements, received {toks.Count}", rawInput);
                            }

                            MeasurementToken pop2 = toks.Pop() as MeasurementToken;
                            MeasurementToken pop1 = toks.Pop() as MeasurementToken;

                            if (pop1 == null) {
                                // Expected a measurement, got an operator
                                return new MeasurementParseException<T>($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, received '{pop1.ToString()}'", rawInput);
                            }

                            if (pop2 == null) {
                                // Expected a measurement, got an operator
                                return new MeasurementParseException<T>($"Unable to evaluate binary operator '{op.ToString()}'. Expected a measurement, received '{pop2.ToString()}'", rawInput);
                            }

                            MeasurementToken eval = op.Evaluate(pop1, pop2);

                            if (eval == null) {
                                // Could not evaluate binary operators
                                return new MeasurementParseException<T>(
                                    $"Unable to evaluate binary operator '{op.ToString()}' on '{pop1.ToString()}' and '{pop2.ToString()}'. The measurements are incompatable", 
                                    rawInput
                                );
                            }

                            toks.Push(eval);
                        }
                    }

                    if (toks.Count > 1) {
                        int i = toks.Count - 1;
                        for (; i > 0; i--) {
                            toks.Push(Operator.Multiply);
                        }

                        return EvalPostfix(new Union<IEnumerable<Token>, MeasurementParseException>(toks.ToArray().Reverse()), rawInput);
                    }

                    object final = (toks.Pop() as MeasurementToken).MeasurementValue;
                    var tFinal = final.GetType();

                    

                    if (!typeof(T).GetTypeInfo().IsAssignableFrom(tFinal.GetTypeInfo())) {
                        if (tFinal == typeof(DoubleMeasurement)) {
                            double d = final as DoubleMeasurement;
                            if (double.IsNaN(d) || double.IsInfinity(d)) {
                                return provider.CreateMeasurement(d, provider.DefaultUnit);
                            }
                        }

                        // Bad type
                        return new MeasurementParseException<T>($"Unable to evaluate expression. Expected  a {typeof(T).ToString()}, received a {tFinal.ToString()}", rawInput);
                    }

                    return new Union<T, MeasurementParseException>(final as T);
                },
                exception => exception
            );
        }

        private Union<IEnumerable<Token>, MeasurementParseException> ToPostfix(Union<IEnumerable<Token>, MeasurementParseException> input, string rawInput) {
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
                                else if (top.Priority >= opTok.Priority) {
                                    ret.Add(operatorStack.Pop());
                                    operatorStack.Push(opTok);
                                }
                                else {
                                    operatorStack.Push(opTok);
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
                        if (!dictionary.ContainsKey(item)) {
                            return new MeasurementParseException<T>($"Unable to recognize unit '{item}'", input);
                        }
                        else {
                            ret.Add(dictionary[item]);
                        }                        
                    }
                }
            }

            return new Union<List<Token>, MeasurementParseException>(ret);
        }

        private Union<IEnumerable<Token>, MeasurementParseException> EvalInfix(Union<List<Token>, MeasurementParseException> input, string rawInput) {
            return input.Select(
                list => {

                    // Add multiplication between units and numbers
                    for (int i = 0; i < list.Count; i++) {
                        Token item = list[i];

                        if (item is UnitToken) {

                            if (i > 0 && list[i - 1] is MeasurementToken) {
                                list.Insert(i, Operator.Multiply);
                                i--;
                            }

                            if (i < list.Count - 1 && list[i + 1] is MeasurementToken) {
                                list.Insert(i + 1, Operator.Multiply);
                            }
                        }                        
                    }

                    return new Union<IEnumerable<Token>, MeasurementParseException>(list);
                },
                exception => exception
            );
        }

        private static Dictionary<string, MeasurementToken> GetUnits<E>(MeasurementProvider<E> provider) where E : Measurement<E> {
            Dictionary<string, MeasurementToken> ret =
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
                    x => Tuple.Create(x.ToString(), (MeasurementToken)(new UnitToken(x.ToMeasurement())))
                )                
                .ToDictionary(x => x.Item1, y => y.Item2);

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

                var prov1Ret = (Dictionary<string, MeasurementToken>)prov1Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component1Provider").GetValue(provider) });
                foreach (var item in prov1Ret.Keys) {
                    if (!ret.ContainsKey(item)) {
                        ret.Add(item, prov1Ret[item]);
                    }
                }

                var prov2Ret = (Dictionary<string, MeasurementToken>)prov2Info.Invoke(null, new[] { provider.GetType().GetRuntimeProperty("Component2Provider").GetValue(provider) });
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