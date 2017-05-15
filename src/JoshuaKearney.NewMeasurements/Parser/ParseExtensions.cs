using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.Parser {
    internal static class ParseExtensions {
        public static string GetSymbolString(this BinaryOperatorType type) {
            switch (type) {
                case BinaryOperatorType.Multiplication: return "*";
                case BinaryOperatorType.Division: return "/";
                case BinaryOperatorType.Addition: return "+";
                case BinaryOperatorType.Subtraction: return "-";
                case BinaryOperatorType.Exponation: return "^";
                default: return string.Empty;
            }
        }

        public static string GetSymbolString(this UrnaryOperatorType type) {
            switch (type) {
                case UrnaryOperatorType.Negation: return "-";
                case UrnaryOperatorType.Positation: return "+";
                default: return string.Empty;
            }
        }
    }
}
