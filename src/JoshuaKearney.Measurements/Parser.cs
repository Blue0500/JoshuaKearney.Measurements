//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace JoshuaKearney.Measurements {
//    internal static class Parser {
//        public static T Parse<T>(string input) where T : Measurement<T> {
//            Validate.NonNull(input, nameof(input));

//            T res;
//            if (TryParse(input, out res)) {
//                return res;
//            }
//            else {
//                throw new FormatException();
//            }
//        }

//        public static bool TryParse<T>(string input, out T result) where T : Measurement<T> {
//            Validate.NonNull(input, nameof(input));

//            return PreProcess(input, out result);
//        }

//        private static Measurement ApplyBinaryOp(Type tInterface, Type tBackup, Measurement x, Measurement y) {
//            Type tFirst = x.GetType();
//            Type tSecond = y.GetType();

//            var tGenericInterface = tFirst
//                .GetTypeInfo()
//                .ImplementedInterfaces
//                .Where(z =>
//                    z.IsConstructedGenericType &&
//                    z.GetGenericTypeDefinition() == tInterface &&
//                    z.GenericTypeArguments.Count() == 2 &&
//                    z.GenericTypeArguments[0] == tSecond)
//                .FirstOrDefault();

//            if (tGenericInterface != null) {
//                var mathMethod = tGenericInterface
//                    .GetTypeInfo()
//                    .DeclaredMethods
//                    .First();

//                return (Measurement)mathMethod.Invoke(x, new[] { y });
//            }
//            else {
//                MethodInfo info = tBackup
//                    .GetTypeInfo()
//                    .GetDeclaredMethods("From")
//                    .First(z => z.GetCustomAttribute(typeof(FlagAttribute)) != null)
//                    .MakeGenericMethod(x.GetType(), y.GetType());

//                return (Measurement)info.Invoke(null, new[] { x, y });
//            }
//        }

//        private static Measurement ApplyUrnaryOp(Type tInterface, Measurement x) {
//            Type tFirst = x.GetType();

//            var tGenericInterface = tFirst
//                .GetTypeInfo()
//                .ImplementedInterfaces
//                .Where(z =>
//                    z.IsConstructedGenericType &&
//                    z.IsConstructedGenericType &&
//                    z.GetGenericTypeDefinition() == tInterface &&
//                    z.GenericTypeArguments.Count() == 1)
//                .FirstOrDefault();

//            if (tGenericInterface != null) {
//                var mathMethod = tGenericInterface
//                    .GetTypeInfo()
//                    .DeclaredMethods
//                    .First();

//                return (Measurement)mathMethod.Invoke(x, new object[] { });
//            }
//            else {
//                return null;
//            }
//        }

//        private static IEnumerable<IUnit> ComposableUnits(Type m) {
//            try {
//                IEnumerable<IUnit> ret = GetMeasurementUnits(m);

//                bool isComplex = IsSubclassOfRawGeneric(typeof(TermBase<,,>), m) || IsSubclassOfRawGeneric(typeof(RatioBase<,,>), m);
//                if (isComplex) {
//                    var generics = m.GetTypeInfo().BaseType.GenericTypeArguments;
//                    return ret.Concat(ComposableUnits(generics[1]).Concat(ComposableUnits(generics[2])));
//                }
//                else {
//                    return ret;
//                }
//            }
//            catch {
//                return new IUnit[] { };
//            }
//        }

//        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
//            while (toCheck != null && toCheck != typeof(object)) {
//                var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
//                if (generic == cur) {
//                    return true;
//                }
//                toCheck = toCheck.GetTypeInfo().BaseType;
//            }
//            return false;
//        }

//        private static bool ParseTokens<T>(List<Token> tokens, out T result) where T : Measurement {
//            // The iterator to use when parsing
//            IEnumerator<Token> tokensEnum = tokens.GetEnumerator();
//            tokensEnum.MoveNext();

//            // Get the original number out
//            double value = (tokensEnum.Current as NumericToken)?.Number ?? 0;

//            // The valid tokens that could make up this Measurement

//            Stack<Token> toks = new Stack<Token>();

//            while (tokensEnum.MoveNext()) {
//                Token tok = tokensEnum.Current;

//                if (tok is MeasurementToken) {
//                    toks.Push(tok);
//                }
//                else if (tok is UrnaryOperator) {
//                    UrnaryOperator op = tok as UrnaryOperator;

//                    if (toks.Count < 1) {
//                        result = null;
//                        return false;
//                    }

//                    MeasurementToken pop1 = toks.Pop() as MeasurementToken;
//                    Measurement eval = op.Evaluate(pop1.Measurement);

//                    if (eval == null) {
//                        result = null;
//                        return false;
//                    }

//                    toks.Push(new MeasurementToken(eval));
//                }
//                else if (tok is BinaryOperator) {
//                    BinaryOperator op = tok as BinaryOperator;

//                    if (toks.Count < 2) {
//                        result = null;
//                        return false;
//                    }

//                    MeasurementToken pop2 = toks.Pop() as MeasurementToken;
//                    MeasurementToken pop1 = toks.Pop() as MeasurementToken;
//                    Measurement eval = op.Evaluate(pop1.Measurement, pop2.Measurement);

//                    if (eval == null) {
//                        result = null;
//                        return false;
//                    }

//                    toks.Push(new MeasurementToken(eval));
//                }
//            }

//            Measurement final = (toks.Pop() as MeasurementToken).Measurement;
//            var tFinal = final.GetType();

//            if (!typeof(T).GetTypeInfo().IsAssignableFrom(tFinal.GetTypeInfo())) {
//                result = null;
//                return false;
//            }

//            result = Measurement<T>.From(
//                final.DefaultUnits * value,
//                Measurement<T>.GetDefaultUnitDefinition()
//            );

//            return true;
//        }

//        private static bool PreProcess<T>(string input, out T result) where T : Measurement {
//            if (input == null) {
//                result = null;
//                return false;
//            }

//            input = input
//                .Replace("^2", "²")
//                .Replace("^3", "³");

//            return Tokenize(input, out result);
//        }

//        private static bool Tokenize<T>(string input, out T result) where T : Measurement {
//            List<Token> ret = new List<Token>();
//            IEnumerable<char> validNums = "123456789.";
//            string numbers = new string(input.TakeWhile(x => validNums.Contains(x)).ToArray());
//            string remaining = input.Substring(numbers.Length).Trim();

//            double parsed;
//            if (!double.TryParse(numbers, out parsed)) {
//                result = null;
//                return false;
//            }
//            else {
//                // All the possible atomic units
//                MethodInfo prefixAllInfo = typeof(Prefix).GetTypeInfo().GetDeclaredMethod("All");

//                IEnumerable<IUnit> units = ComposableUnits(typeof(T))
//                    .SelectMany(x => {
//                        if (x is IPrefixableUnit) {
//                            Type tX = x.GetType();

//                            if (tX.GenericTypeArguments == null || tX.GenericTypeArguments.Count() <= 0) {
//                                return new IUnit[] { };
//                            }

//                            MethodInfo generic = prefixAllInfo.MakeGenericMethod(tX.GenericTypeArguments[0]);
//                            return (IEnumerable<IUnit>)generic.Invoke(null, new[] { x });
//                        }
//                        else {
//                            return new IUnit[] { x };
//                        }
//                    });

//                // Helper to get a matching unit
//                Func<string, IUnit> matchUnit = x => units.FirstOrDefault(y => y.Name == x || y.Symbol.ToString() == x/* || y.Aliases.Any(z => z == x)*/);

//                ret.Add(new NumericToken(parsed));

//                string notLetters = "*/²³()";
//                string current = "";

//                foreach (char c in remaining) {
//                    if (notLetters.All(x => x != c)) {
//                        current += c;
//                    }
//                    else {
//                        if (c != '(' && !string.IsNullOrWhiteSpace(current)) {
//                            IUnit u = matchUnit(current);
//                            if (u == null) {
//                                result = null;
//                                return false;
//                            }

//                            if (u.UnitsPerDefault == 0) {
//                                result = null;
//                                return false;
//                            }

//                            ret.Add(new MeasurementToken(CreateMeasurement(u.AssociatedMeasurement, 1 / u.UnitsPerDefault)));
//                            current = "";
//                        }

//                        switch (c) {
//                            case '*': ret.Add(Token.Multiply); break;
//                            case '/': ret.Add(Token.Divide); break;
//                            case '²': ret.Add(Token.Square); break;
//                            case '³': ret.Add(Token.Cube); break;
//                            case '(': ret.Add(Token.OpenParen); break;
//                            case ')': ret.Add(Token.CloseParen); break;
//                        }
//                    }
//                }

//                if (!string.IsNullOrWhiteSpace(current)) {
//                    IUnit u = matchUnit(current);
//                    if (u == null) {
//                        result = null;
//                        return false;
//                    }

//                    ret.Add(new MeasurementToken(CreateMeasurement(u.AssociatedMeasurement, 1 / u.UnitsPerDefault)));
//                }

//                return ToPostfix(ret, out result);
//            }
//        }

//        private static Measurement CreateMeasurement(Type t, double units) {
//            return ((Measurement)Activator.CreateInstance(t)).InternalMeasurementInfo.CreateInstance(units);
//        }

//        private static IEnumerable<IUnit> GetMeasurementUnits(Type t) {
//            return ((Measurement)Activator.CreateInstance(t)).InternalMeasurementInfo.UniqueUnits;
//        }

//        private static bool ToPostfix<T>(IEnumerable<Token> input, out T result) where T : Measurement {
//            Stack<Operator> operatorStack = new Stack<Operator>();
//            List<Token> ret = new List<Token>();

//            foreach (Token tok in input) {
//                // Push operands
//                if (!(tok is Operator)) {
//                    ret.Add(tok);
//                }
//                else {
//                    Operator opTok = tok as Operator;

//                    if (opTok == Token.OpenParen) {
//                        operatorStack.Push(opTok);
//                        continue;
//                    }

//                    // If the stack is empty, put an operator on it
//                    if ((operatorStack.Count == 0 || operatorStack.Peek() == Token.OpenParen) && opTok != Token.CloseParen) {
//                        operatorStack.Push(opTok);
//                    }
//                    else {
//                        // Mismatched parenthensis
//                        if (operatorStack.Count == 0) {
//                            result = null;
//                            return false;
//                        }

//                        // If the top has a higher priority, pop and print it
//                        Operator top = operatorStack.Peek();
//                        if (top.Priority > opTok.Priority) {
//                            ret.Add(operatorStack.Pop());
//                            operatorStack.Push(opTok);
//                        }
//                        else {
//                            if (opTok == Token.CloseParen) {
//                                while (operatorStack.Peek() != Token.OpenParen) {
//                                    ret.Add(operatorStack.Pop());
//                                }

//                                // Catch mismatched parenthensis
//                                if (operatorStack.Peek() != Token.OpenParen) {
//                                    result = null;
//                                    return false;
//                                }

//                                operatorStack.Pop();
//                            }
//                            else {
//                                operatorStack.Push(opTok);
//                            }
//                        }
//                    }
//                }
//            }

//            while (operatorStack.Count > 0) {
//                if (operatorStack.Peek() == Token.OpenParen || operatorStack.Peek() == Token.CloseParen) {
//                    result = null;
//                    return false;
//                }

//                ret.Add(operatorStack.Pop());
//            }

//            return ParseTokens(ret, out result);
//        }

//        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
//        internal sealed class FlagAttribute : Attribute {
//            public FlagAttribute() {
//            }
//        }

//        private class BinaryOperator : Operator {
//            public BinaryOperator(string value, int priority, Func<Measurement, Measurement, Measurement> eval) : base(value, priority) {
//                this.Evaluate = eval;
//            }

//            public Func<Measurement, Measurement, Measurement> Evaluate { get; }
//        }

//        private class MeasurementToken : Token {
//            public MeasurementToken(Measurement unit) : base("MeasurementToken") {
//                this.Measurement = unit;
//            }

//            public Measurement Measurement { get; }
//        }

//        private class NumericToken : Token {
//            public NumericToken(double num) : base(num.ToString()) {
//                this.Number = num;
//            }

//            public double Number { get; }
//        }

//        private class Operator : Token {
//            public Operator(string value, int priority) : base(value) {
//                this.Priority = priority;
//            }

//            // Higher is higher priortiy
//            public int Priority { get; }
//        }

//        private abstract class Token {
//            public Token(string value) {
//                this.Value = value;
//            }

//            public static Operator CloseParen { get; } = new Operator(")", 100);

//            public static UrnaryOperator Cube { get; } = new UrnaryOperator("³", 10, x => ApplyUrnaryOp(typeof(ICubableMeasurement<>), x));

//            public static BinaryOperator Divide { get; } = new BinaryOperator("/", 5, (x, y) => ApplyBinaryOp(typeof(IDividableMeasurement<,>), typeof(Ratio), x, y));

//            public static BinaryOperator Multiply { get; } = new BinaryOperator("*", 5, (x, y) => ApplyBinaryOp(typeof(IMultipliableMeasurement<,>), typeof(Term), x, y));

//            public static Operator OpenParen { get; } = new Operator("(", 100);

//            public static UrnaryOperator Square { get; } = new UrnaryOperator("²", 10, x => ApplyUrnaryOp(typeof(ISquareableMeasurement<>), x));

//            public string Value { get; }

//            public override string ToString() => this.Value;
//        }

//        private class UrnaryOperator : Operator {
//            public UrnaryOperator(string value, int priority, Func<Measurement, Measurement> eval) : base(value, priority) {
//                this.Evaluate = eval;
//            }

//            public Func<Measurement, Measurement> Evaluate { get; }
//        }
//    }
//}