namespace JoshuaKearney.Measurements {

    public class MeasurementParser<T> where T : Measurement<T> {

        private static bool PreProcess(string input, out T result) {
            if (input == null) {
                result = null;
                return false;
            }

            input = input
                .Replace("^2", "²")
                .Replace("^3", "³");

            result = null;
            return false;
            //return Tokenize(input, out result);
        }

        //private static bool Tokenize(string input, out T result) {
        //    List<Token> ret = new List<Token>();
        //    IEnumerable<char> validNums = "123456789.";
        //    string numbers = new string(input.TakeWhile(x => validNums.Contains(x)).ToArray());
        //    string remaining = input.Substring(numbers.Length).Trim();

        //    double parsed;
        //    if (!double.TryParse(numbers, out parsed)) {
        //        result = null;
        //        return false;
        //    }
        //    else {
        //        //// All the possible atomic units
        //        //MethodInfo prefixAllInfo = typeof(Prefix).GetTypeInfo().GetDeclaredMethod("All");

        //        //IEnumerable<IUnit> units = ComposableUnits(typeof(T))
        //        //    .SelectMany(x => {
        //        //        if (x is IPrefixableUnit) {
        //        //            Type tX = x.GetType();

        //        //            if (tX.GenericTypeArguments == null || tX.GenericTypeArguments.Count() <= 0) {
        //        //                return new IUnit[] { };
        //        //            }

        //        //            MethodInfo generic = prefixAllInfo.MakeGenericMethod(tX.GenericTypeArguments[0]);
        //        //            return (IEnumerable<IUnit>)generic.Invoke(null, new[] { x });
        //        //        }
        //        //        else {
        //        //            return new IUnit[] { x };
        //        //        }
        //        //    });

        //        // Helper to get a matching unit
        //        //Func<string, IUnit> matchUnit = x => units.FirstOrDefault(y => y.Name == x || y.Symbol.ToString() == x/* || y.Aliases.Any(z => z == x)*/);

        //        ret.Add(new NumericToken(parsed));

        //        string notLetters = "*/²³()";
        //        string current = "";

        //        foreach (char c in remaining) {
        //            if (notLetters.All(x => x != c)) {
        //                current += c;
        //            }
        //            else {
        //                if (c != '(' && !string.IsNullOrWhiteSpace(current)) {
        //                    IUnit u = matchUnit(current);
        //                    if (u == null) {
        //                        result = null;
        //                        return false;
        //                    }

        //                    if (u.UnitsPerDefault == 0) {
        //                        result = null;
        //                        return false;
        //                    }

        //                    ret.Add(new MeasurementToken(CreateMeasurement(u.AssociatedMeasurement, 1 / u.UnitsPerDefault)));
        //                    current = "";
        //                }

        //                switch (c) {
        //                    case '*': ret.Add(Token.Multiply); break;
        //                    case '/': ret.Add(Token.Divide); break;
        //                    case '²': ret.Add(Token.Square); break;
        //                    case '³': ret.Add(Token.Cube); break;
        //                    case '(': ret.Add(Token.OpenParen); break;
        //                    case ')': ret.Add(Token.CloseParen); break;
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrWhiteSpace(current)) {
        //            IUnit u = matchUnit(current);
        //            if (u == null) {
        //                result = null;
        //                return false;
        //            }

        //            ret.Add(new MeasurementToken(CreateMeasurement(u.AssociatedMeasurement, 1 / u.UnitsPerDefault)));
        //        }

        //        return ToPostfix(ret, out result);
        //    }
        //}

        //private class BinaryOperator : Operator {
        //    public BinaryOperator(string value, int priority, Func<dynamic, dynamic, dynamic> eval) : base(value, priority) {
        //        this.Evaluate = eval;
        //    }

        //    public Func<dynamic, dynamic, dynamic> Evaluate { get; }
        //}

        //private class MeasurementToken : Token {
        //    public MeasurementToken(dynamic unit) : base("MeasurementToken") {
        //        this.Measurement = unit;
        //    }

        //    public dynamic Measurement { get; }
        //}

        //private class NumericToken : Token {
        //    public NumericToken(double num) : base(num.ToString()) {
        //        this.Number = num;
        //    }

        //    public double Number { get; }
        //}

        //private class Operator : Token {
        //    public Operator(string value, int priority) : base(value) {
        //        this.Priority = priority;
        //    }

        //    // Higher is higher priortiy
        //    public int Priority { get; }
        //}

        //private abstract class Token {
        //    public Token(string value) {
        //        this.Value = value;
        //    }

        //    public static Operator CloseParen { get; } = new Operator(")", 100);

        //    //public static UrnaryOperator Cube { get; } = new UrnaryOperator("³", 10, x => ApplyUrnaryOp(typeof(ICubableMeasurement<>), x));
        //    //public static BinaryOperator Divide { get; } = new BinaryOperator("/", 5, (x, y) => ApplyBinaryOp(typeof(IDividableMeasurement<,>), typeof(Ratio), x, y));
        //    //public static BinaryOperator Multiply { get; } = new BinaryOperator("*", 5, (x, y) => ApplyBinaryOp(typeof(IMultipliableMeasurement<,>), typeof(Term), x, y));
        //    public static Operator OpenParen { get; } = new Operator("(", 100);

        //    //public static UrnaryOperator Square { get; } = new UrnaryOperator("²", 10, x => ApplyUrnaryOp(typeof(ISquareableMeasurement<>), x));
        //    public string Value { get; }

        //    public override string ToString() => this.Value;
        //}

        //private class UrnaryOperator : Operator {
        //    public UrnaryOperator(string value, int priority, Func<dynamic, dynamic> eval) : base(value, priority) {
        //        this.Evaluate = eval;
        //    }

        //    public Func<dynamic, dynamic> Evaluate { get; }
        //}
    }
}