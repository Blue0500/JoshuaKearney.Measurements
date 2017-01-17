using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    public class ParseException : Exception {
        public static string MessagePrefix { get; } = "Unable to parse measurement: ";
        internal static ParseException TypeConversionError(Type t1, Type t2) => new ParseException(MessagePrefix + $"Cannot convert type '{t1}' to {t2}");
        internal static ParseException UnexpectedCharactersError(string str) => new ParseException(MessagePrefix + $"Unexpected character(s) '{str}'");
        internal static ParseException BinaryOperatorEvaluationFailed(string op, string op1, string op2) => new ParseException(MessagePrefix + $"Evaluation of operator '{op}' failed on operands '{op1}' and '{op2}'");
        internal static ParseException UrnaryOperatorEvaluationFailed(string op, string term) => new ParseException(MessagePrefix + $"Evaluation of operator '{op} failed on operand {term}'");
        internal static ParseException UndefinedUnitDiscovered(string unit) => new ParseException(MessagePrefix + $"The unit '{unit}' is undefined");
        internal static ParseException UnrecognizedSyntaxConstructDiscovered(Type t) => new ParseException(MessagePrefix + $"The construct '{t.ToString()}' was not recognized as valid syntax");
        internal static ParseException NumberParseFailed(char start) => new ParseException(MessagePrefix + $"Failed to parse number starting with '{start}'");
        internal static ParseException UnspecifiedError() => new ParseException(MessagePrefix + "Unspecified error");

        public ParseException(string message) : base(message) {
            Validate.NonNull(message, nameof(message));
        }
    }
}
